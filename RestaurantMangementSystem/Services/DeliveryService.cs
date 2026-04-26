using RestaurantMangementSystem.Enums;
using RestaurantMangementSystem.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Services
{
    public static class DeliveryService
    {
        public static (bool success, string message) AssignStaff(int OrderId , int DeliveryStaffId , int AssignedByEmployeeId)
        {
            var order = Data.DataBase.Orders.FirstOrDefault(x =>x.OrderId == OrderId);
            if (order is null)
                return (false, "Order not Found");
            if (order.OrderStatus != Enums.OrderStatus.Served)
                return (false, "Delivery Staff Can only assigned to Served Orders");
            if (order.OrderType != OrderType.Delivery)
                return (false, "this order is not delivery Order");
            var employee = Data.DataBase.Employees.FirstOrDefault(x => x.EmployeeId == AssignedByEmployeeId);
            if (employee is null)
                return (false, "The Employee not found");
            if (!employee.AssignedBranchIds.Contains(order.BranchId))
                return (false, "The Employee is not Assigned to this branch");
            if (employee is not BranchManager && employee is not Cashier && employee is not Waiter)
                return (false, "Only Waiters, Cashier or Branch Manager Can Assign Delivery Staff.");
            var staff = Data.DataBase.DeliveryStaffList.FirstOrDefault(x => x.StaffId == DeliveryStaffId);
            if (staff is null)
                return (false, "Delivery Staff is not found");
            if (!staff.IsAvailable)
                return (false, "Delivery Staff is not Available");
            if (staff.BranchId != order.BranchId)
                return (false, "The Delivery staff must be in the same branch as order");


            var delivery = Data.DataBase.Deliveries.FirstOrDefault(x=>x.OrderId == OrderId);
            if (delivery is null)
                return (false, "Delivery Record is not found"); 
            delivery.DeliveryStaffId = DeliveryStaffId;
            delivery.DeliveryStatus = DeliveryStatus.OnTheWay;
            staff.IsAvailable = true;
            return (true, "On The way...");




        }
        public static (bool success,string message) MarkFailed(int DeliveryId , int DeliveryStaffId, string Reason )
        {
            var delivery = Data.DataBase.Deliveries.FirstOrDefault(x=>x.DeliveryId == DeliveryId);
            if (delivery is null)
                return (false, "Delivery Record is not Found");
            if (delivery.DeliveryStaffId != DeliveryStaffId)
                return (false, "is not your delivery");
            if (string.IsNullOrEmpty(Reason))
                return (false, "Failure reason must be provided");
            delivery.DeliveryStatus = DeliveryStatus.Failed;
            delivery.FailureReason = Reason;
            var order = Data.DataBase.Orders.FirstOrDefault(x => x.OrderId == delivery.OrderId);
            if (order is not null)
                order.OrderStatus = OrderStatus.Canceled;
            return (true, "Order marked failed");
        }
        public static (bool sucesss , string message) MarkDelivered(int DeliveryId , int DeliveryStaffId)
        {
            var delivery = Data.DataBase.Deliveries.FirstOrDefault(x=>x.DeliveryId==DeliveryId);
            if (delivery is null) return (false, "Delivery record is not found");
            if (delivery.DeliveryStaffId != DeliveryStaffId)
                return (false, "not your delivery");
            if (delivery.DeliveryStatus != DeliveryStatus.OnTheWay)
                return (false, "only deliveries that are on the way can be delivered");
            delivery.DeliveryStatus = DeliveryStatus.Deliverd;
            delivery.DeliveryTime = DateTime.Now;
            var order = Data.DataBase.Orders.FirstOrDefault(x=>x.OrderId == delivery.OrderId);
            if(order is not null)
            {
                order.OrderStatus = OrderStatus.Completed;
                var customer = Data.DataBase.Customers.FirstOrDefault(x => x.CustomerId == order.CustomerId);
                if(customer is  not null) 
                customer.LoyaltyPoints += (int)Math.Floor(order.TotalAmount);
            }
            var staff = Data.DataBase.DeliveryStaffList.FirstOrDefault(x => x.StaffId == DeliveryStaffId);
            if(staff is not null) 
                staff.IsAvailable = true;
            return (true, "Marked As Delivered");

                
        }

    }
}
