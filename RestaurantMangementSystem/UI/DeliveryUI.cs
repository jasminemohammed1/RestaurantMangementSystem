using RestaurantMangementSystem.Data;
using RestaurantMangementSystem.Enums;
using RestaurantMangementSystem.Models;
using RestaurantMangementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.UI
{
    public static class DeliveryUI
    {
        public static void Run()
        {
            while (true)
            {
                ConsoleHelper.PrintHeader("Delivery Management");
                ConsoleHelper.PrintOption("1", "List All Deliveries");
                ConsoleHelper.PrintOption("2", "Assign Delivery Staff");
                ConsoleHelper.PrintOption("3", "Mark as Delivered");
                ConsoleHelper.PrintOption("4", "Mark as Failed");
                ConsoleHelper.PrintOption("0", "Back");

                switch (ConsoleHelper.ReadString("Choice"))
                {
                    case "1": ListDeliveries(); break;
                    case "2": AssignStaff(); break;
                    case "3": MarkDelivered(); break;
                    case "4": MarkFailed(); break;
                    case "0": return;
                    default: ConsoleHelper.Error("Invalid option."); break;
                }
            }
        }

        private static void ListDeliveries()
        {
            ConsoleHelper.PrintHeader("All Deliveries");
            if (!DataBase.Deliveries.Any()) { ConsoleHelper.Warning("No deliveries found."); ConsoleHelper.Wait(); return; }

            Console.WriteLine($"\n   {"Del#",-6} {"Ord#",-6} {"Address",-28} {"Staff",-20} {"Status",-22} {"Time"}");
            Console.WriteLine("   " + new string('─', 95));
            foreach (var d in DataBase.Deliveries)
            {
                var staff = d.DeliveryStaffId.HasValue
                    ? DataBase.DeliveryStaffList.FirstOrDefault(s => s.StaffId == d.DeliveryStaffId)?.FullName ?? "?"
                    : "Unassigned";
                var color = d.DeliveryStatus switch
                {
                    DeliveryStatus.Deliverd => ConsoleColor.Green,
                    DeliveryStatus.Failed => ConsoleColor.DarkRed,
                    DeliveryStatus.OnTheWay => ConsoleColor.Yellow,
                    _ => ConsoleColor.White
                };
                Console.ForegroundColor = color;
                Console.WriteLine($"   {d.DeliveryId,-6} {d.OrderId,-6} {d.DeliveryAddress,-28} {staff,-20} {d.DeliveryTime,-22} {d.DeliveryTime?.ToString("HH:mm") ?? "—"}");
                Console.ResetColor();
                if (d.FailureReason is not null)
                    ConsoleHelper.Info($"     Reason: {d.FailureReason}");
            }
            ConsoleHelper.Wait();
        }

        private static void AssignStaff()
        {
            ConsoleHelper.PrintHeader("Assign Delivery Staff");

            // Show served delivery orders
            var servedDeliveries = DataBase.Orders
                .Where(o => o.OrderType == OrderType.Delivery && o.OrderStatus == OrderStatus.Served).ToList();

            if (!servedDeliveries.Any())
            { ConsoleHelper.Warning("No served delivery orders awaiting assignment."); ConsoleHelper.Wait(); return; }

            ConsoleHelper.PrintSubHeader("Served Delivery Orders");
            Console.WriteLine($"\n   {"Order#",-8} {"Branch",-22} {"Customer",-22} {"Total"}");
            Console.WriteLine("   " + new string('─', 60));
            foreach (var o in servedDeliveries)
            {
                var cust = DataBase.Customers.FirstOrDefault(c => c.CustomerId == o.CustomerId);
                var branch = DataBase.Branches.FirstOrDefault(b => b.BranchId == o.BranchId);
                Console.WriteLine($"   {o.OrderId,-8} {branch?.Name ?? "?",-22} {cust?.FullName ?? "?",-22} {o.TotalAmount:C}");
            }

            int orderId = ConsoleHelper.ReadInt("Order ID");

            // Show available delivery staff for that branch
            var order = DataBase.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order is null) { ConsoleHelper.Error("Order not found."); ConsoleHelper.Wait(); return; }

            ConsoleHelper.PrintSubHeader("Available Delivery Staff (Same Branch)");
            var avail = DataBase.DeliveryStaffList
                .Where(s => s.BranchId == order.BranchId && s.IsAvailable).ToList();
            if (!avail.Any()) { ConsoleHelper.Warning("No available delivery staff for this branch."); ConsoleHelper.Wait(); return; }
            foreach (var s in avail)
                ConsoleHelper.Info($"[{s.StaffId}] {s.FullName,-22} {s.VehicleType,-14} Area: {s.AssignedArea}");

            int staffId = ConsoleHelper.ReadInt("Delivery Staff ID");

            ConsoleHelper.PrintSubHeader("Assigned By (Waiter / Cashier / Branch Manager)");
            foreach (var e in DataBase.Employees.Where(e => e is Waiter || e is Cashier || e is BranchManager))
                ConsoleHelper.Info($"[{e.EmployeeId}] {e.FullName,-22} {e.GetRole()}");
            int empId = ConsoleHelper.ReadInt("Employee ID");

            var (success, msg) = DeliveryService.AssignStaff(orderId, staffId, empId);
            if (success) ConsoleHelper.Success(msg); else ConsoleHelper.Error(msg);
            ConsoleHelper.Wait();
        }

        private static void MarkDelivered()
        {
            ConsoleHelper.PrintHeader("Mark Delivery as Delivered");

            var active = DataBase.Deliveries.Where(d => d.DeliveryStatus == DeliveryStatus.OnTheWay).ToList();
            if (!active.Any()) { ConsoleHelper.Warning("No deliveries currently on the way."); ConsoleHelper.Wait(); return; }

            ConsoleHelper.PrintSubHeader("Active Deliveries");
            foreach (var d in active)
            {
                var staff = DataBase.DeliveryStaffList.FirstOrDefault(s => s.StaffId == d.DeliveryStaffId);
                ConsoleHelper.Info($"[Delivery #{d.DeliveryId}] Order #{d.OrderId} — Driver: {staff?.FullName ?? "?"}  →  {d.DeliveryAddress}");
            }

            int deliveryId = ConsoleHelper.ReadInt("Delivery ID");
            var del = DataBase.Deliveries.FirstOrDefault(d => d.DeliveryId == deliveryId);
            if (del is null) { ConsoleHelper.Error("Delivery not found."); ConsoleHelper.Wait(); return; }

            ConsoleHelper.Info($"Assigned staff: [{del.DeliveryStaffId}] {DataBase.DeliveryStaffList.FirstOrDefault(s => s.StaffId == del.DeliveryStaffId)?.FullName}");
            int staffId = ConsoleHelper.ReadInt("Your Staff ID (must match assigned)");

            var (success, msg) = DeliveryService.MarkDelivered(deliveryId, staffId);
            if (success) ConsoleHelper.Success(msg); else ConsoleHelper.Error(msg);
            ConsoleHelper.Wait();
        }

        private static void MarkFailed()
        {
            ConsoleHelper.PrintHeader("Mark Delivery as Failed");

            var active = DataBase.Deliveries.Where(d => d.DeliveryStatus == DeliveryStatus.OnTheWay).ToList();
            if (!active.Any()) { ConsoleHelper.Warning("No deliveries currently on the way."); ConsoleHelper.Wait(); return; }

            ConsoleHelper.PrintSubHeader("Active Deliveries");
            foreach (var d in active)
            {
                var staff = DataBase.DeliveryStaffList.FirstOrDefault(s => s.StaffId == d.DeliveryStaffId);
                ConsoleHelper.Info($"[Delivery #{d.DeliveryId}] Order #{d.OrderId} — Driver: {staff?.FullName ?? "?"}");
            }

            int deliveryId = ConsoleHelper.ReadInt("Delivery ID");
            int staffId = ConsoleHelper.ReadInt("Your Staff ID");
            string reason = ConsoleHelper.ReadString("Failure Reason");

            var (success, msg) = DeliveryService.MarkFailed(deliveryId, staffId, reason);
            if (success) ConsoleHelper.Success(msg); else ConsoleHelper.Error(msg);
            ConsoleHelper.Wait();
        }
    }
    }

