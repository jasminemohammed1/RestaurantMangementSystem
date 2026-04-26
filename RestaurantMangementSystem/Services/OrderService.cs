using RestaurantMangementSystem.Data;
using RestaurantMangementSystem.Enums;
using RestaurantMangementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RestaurantMangementSystem.Services
{
    public static class OrderService
    {
        public static (bool Success , string Message , int OrderId) PlaceOrder(
            int BranchId, int CustomerId , int EmployeeId , OrderType OrderType,
            string ? DeliveryAddress,
            List<(int ItemId , int Qnt , string? Notes , List<int>AddOnIds)> items)   
        {
            // Rule: Only Waiters and Cashiers can place orders
            var employee = DataBase.Employees.FirstOrDefault(e => e.EmployeeId == EmployeeId);
            if(employee == null)
            {
                return ( false, "This Employee is not Found.", 0);
            }
            if(employee is not Waiter && employee is not Cashier)
            {
                return (false, "Error Only Waiter and Cashier Can Place Orders.", 0);
            }

             // Rule: Employee must be assigned to that branch
             if(!employee.AssignedBranchIds.Contains(BranchId))
            
                  return (false, "This Employee is not Assigned in that branch.", 0);
            
            // Customer check
            var customer = DataBase.Customers.FirstOrDefault(x => x.CustomerId == CustomerId);
            if (customer is null)
                return (false, "This Customer Doesnot Exist.", 0);

            // Branch check
            if (!DataBase.Branches.Any(x => x.BranchId == BranchId))
                return (false, "This Branch Doesnot Exist.", 0);
            // Rule: Delivery needs address
            if (OrderType == OrderType.Delivery && string.IsNullOrEmpty(DeliveryAddress))
                return (false, "Delivery orders require a delivery address.", 0);

            // Rule: At least one item
            if (items is null || items.Count == 0)
                return (false, "Order must contain at least one item.", 0);

            // Build order items, validate availability and pricing
            var orderItems = new List<OrderItem>();
            decimal total = 0;
            int nextSeq = DataBase.NextOrderItemId;
            foreach(var(itemId , Qnt, Notes , AddonIds)in items)
            {
                if (Qnt < 0)
                    return (false, $"Item Quantity must be grater than zero ({itemId})",0);

                // Branch availability check
                var BranchMenueItem = DataBase.BranchMenuItems.FirstOrDefault(x => x.BranchId == BranchId && x.ItemId == itemId);
                if(BranchMenueItem is null || !BranchMenueItem.IsAvailable)
                    return (false, $"Menu item '{itemId}' is not available at this branch.", 0);
                var MenueItem = DataBase.MenuItems.FirstOrDefault(x =>x.ItemId == itemId);
                  if(MenueItem is null )
                    return (false, $"Menu item '{itemId}' not found.", 0);

                // Branch price override
                decimal unitPrice = BranchMenueItem.PriceOverride ?? MenueItem.BasePrice;
                // Add-on prices
                foreach( var addOnId in AddonIds)
                {
                    var addon = MenueItem.AddOns.FirstOrDefault(x =>x.AddOnId == addOnId);
                    if(addon is null )
                        return (false, $"Add-on '{addOnId}' not found for item '{MenueItem.Name}'.", 0);
                    unitPrice += addon.ExtraPrice;
                }
                orderItems.Add(new OrderItem
                {
                    OrderItemId = nextSeq++,
                    SpecialNotes = Notes,
                    ItemId = itemId,
                    SelectedAddonIDs = AddonIds,
                    Quantity = Qnt,
                    UnitPrice = unitPrice,
                });
                total += Qnt * unitPrice;




            }
            var Order = new Order()
            {
                OrderId = DataBase.NextOrderId++,
                BranchId = BranchId,
                CustomerId = CustomerId,
                OrderType = OrderType,
                DateTime = DateTime.Now,
                HandledByEmployeeId = EmployeeId,
                DeliveryAddress = DeliveryAddress,
                OrderStatus = OrderStatus.Pending,
                Items = orderItems,
                TotalAmount = total,
            };
            DataBase.NextOrderItemId = nextSeq;
            DataBase.Orders.Add(Order);
            // Create delivery record for delivery orders
            if(OrderType == OrderType.Delivery)
            {
                DataBase.Deliveries.Add(new Delivery()
                {
                    DeliveryId = DataBase.NextDeliveryId++,
                    DeliveryAddress = DeliveryAddress,
                    DeliveryStatus = DeliveryStatus.AwaitingAssignment,
                    OrderId = Order.OrderId

                });

            }
            return (true, "Order placed successfully.", Order.OrderId);
        




    }

        public static (bool sucesss , string message) StartPreparing(int orderId , int cheifId , int ?managerOverrideId = null)
        {
            var order = Data.DataBase.Orders.FirstOrDefault(x=>x.OrderId == orderId);
            if (order is null)
                return (false, "Order not found");
            if (order.OrderStatus != OrderStatus.Pending)
                return (false, "Order must be in pending state");
            var employee = Data.DataBase.Employees.FirstOrDefault(x=>x.EmployeeId== cheifId);
            if (employee is null || employee is not Chef)
                return (false, "Employee must be a cheif");
            if (!employee.AssignedBranchIds.Contains(order.BranchId))
                return (false, "Cheif must be at same branch as the order");
            bool isSufficient = InventoryService.IsSufficient(order.BranchId, order.Items);
            if(!isSufficient)
            {
                if (managerOverrideId is null)
                    return (false, "InSufficient Stock");
                var manager = Data.DataBase.Employees.FirstOrDefault(x => x.EmployeeId == managerOverrideId);
                if (manager is null || manager is not BranchManager)
                    return (false, "Access Denied");
                if (!manager.AssignedBranchIds.Contains(order.BranchId))
                    return (false, "Manager must be at the same branch as the order");
                order.ManagerOverrideId = managerOverrideId;
                order.IsManagerOverridenUsed = true;
            }
            InventoryService.Dedcut(order.BranchId, order.Items);
            order.OrderStatus = OrderStatus.Preparing;
            return (true, "Order start preparing");
        }
        public static (bool sucess , string message) ServeOrder(int orderId , int cheifId)
        {
            var order = Data.DataBase.Orders.FirstOrDefault(x => x.OrderId == orderId);
            if (order is null)
                return (false, "Order not found");
            if (order.OrderStatus != OrderStatus.Preparing)
                return (false, "Order must be in preparing state");
            var emp = DataBase.Employees.FirstOrDefault(x => x.EmployeeId == cheifId);
            if (emp is null || emp is not Chef)
                return (false, "Employee must be a cheif");
            if (!emp.AssignedBranchIds.Contains(order.BranchId))
                return (false, "Cheif must be at same branch as order");
            order.OrderStatus = OrderStatus.Served;
            return (true, "Order marked as served");
        }
        public static (bool sucess , string message)  ProcessPayment(int orderId , int cashierId , PaymentType paymentType)
        {
            var order = DataBase.Orders.FirstOrDefault(x=>x.OrderId == orderId);
            if (order is null) return (false, "Order not found");
            if (order.OrderStatus != OrderStatus.Served)
                return (false, "Must be served Order");
            var cashier = DataBase.Employees.FirstOrDefault(x=>x.EmployeeId == cashierId);
            if (cashier is null || cashier is not Cashier)
                return (false, "Must be a chashier");
            if (!cashier.AssignedBranchIds.Contains(order.BranchId))
                return (false, "Cashier must be at same branch as the order");
            order.PaymentMethod = paymentType;
            order.OrderStatus = OrderStatus.Completed;
            var Customer = DataBase.Customers.FirstOrDefault(x => x.CustomerId == order.CustomerId);
            int eared = (int)Math.Floor(order.TotalAmount);
            if (Customer is not null)
                Customer.LoyaltyPoints += eared;
            return (true, $"Payment Processed {paymentType} . Customer Earned {eared} Loyalty Points");
        }
        public static (bool sucesss , string message) CancelOrder(int OrderId , int employeeId)
        {
            var Order = DataBase.Orders.FirstOrDefault(e => e.OrderId == OrderId);
            if (Order is null)
                return (false, "Order not found");

            if (Order.OrderStatus == OrderStatus.Completed)
                return (false, "Order Cant be in Cancel Completed Order");
            if (Order.OrderStatus == OrderStatus.Canceled)
                return (false, "Order already Cancelled");

            var Employee = DataBase.Employees.FirstOrDefault(x => x.EmployeeId == employeeId);
            if (Employee is null)
                return (false, "Employee not found");
            if (!Employee.AssignedBranchIds.Contains(Order.BranchId))
                return (false, "Emplyee is not assigned to this branch");
            if(Employee is Waiter || Employee is Cashier)
            {
                if(Order.OrderStatus!=OrderStatus.Preparing)
                {
                    return (false, "Waiter or Cashier can only cancel pending Orders");
                }
            }
            else if (Employee is BranchManager)
            {
                if(Order.OrderType == OrderType.Delivery)
                {
                    var Delivery = DataBase.Deliveries.FirstOrDefault(x=>x.OrderId == OrderId);
                    if(Delivery is not null && Delivery.DeliveryStatus == DeliveryStatus.OnTheWay && Delivery.DeliveryStaffId.HasValue)
                    {
                        return (false,"Cant Cancel : Order is already out for Delivery .");
                    }
                }
            }
            else
            {
                return (false, "You Dont have Pernission To Cancel Orders");
            }
            Order.OrderStatus = OrderStatus.Canceled;
            return (true, "The Order Cancelled Sucessfully");


        }
    }
}
