using RestaurantMangementSystem.Data;
using RestaurantMangementSystem.Enums;
using RestaurantMangementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

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
    }
}
