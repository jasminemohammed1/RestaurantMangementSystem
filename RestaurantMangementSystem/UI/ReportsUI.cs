using RestaurantMangementSystem.Data;
using RestaurantMangementSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.UI
{
    public static  class ReportsUI
    {
        public static void Run()
        {
            while (true)
            {
                ConsoleHelper.PrintHeader("Reports & Analytics");
                ConsoleHelper.PrintOption("1", "Revenue Summary by Branch");
                ConsoleHelper.PrintOption("2", "Order Status Summary");
                ConsoleHelper.PrintOption("3", "Top Menu Items");
                ConsoleHelper.PrintOption("4", "Customer Loyalty Leaderboard");
                ConsoleHelper.PrintOption("5", "Inventory Status Overview");
                ConsoleHelper.PrintOption("6", "Feedback Rating Summary");
                ConsoleHelper.PrintOption("0", "Back");

                switch (ConsoleHelper.ReadString("Choice"))
                {
                    case "1": RevenueSummary(); break;
                    case "2": OrderStatusSummary(); break;
                    case "3": TopMenuItems(); break;
                    case "4": LoyaltyLeaderboard(); break;
                    case "5": InventoryOverview(); break;
                    case "6": FeedbackSummary(); break;
                    case "0": return;
                    default: ConsoleHelper.Error("Invalid option."); break;
                }
            }
        }

        private static void RevenueSummary()
        {
            ConsoleHelper.PrintHeader("Revenue Summary by Branch");
            Console.WriteLine($"\n   {"Branch",-28} {"Orders",-8} {"Completed",-12} {"Revenue",-14} {"Avg Order"}");
            Console.WriteLine("   " + new string('─', 70));

            foreach (var b in DataBase.Branches)
            {
                var orders = DataBase.Orders.Where(o => o.BranchId == b.BranchId).ToList();
                var completed = orders.Where(o => o.OrderStatus == OrderStatus.Completed).ToList();
                decimal revenue = completed.Sum(o => o.TotalAmount);
                decimal avg = completed.Any() ? revenue / completed.Count : 0;
                Console.WriteLine($"   {b.Name,-28} {orders.Count,-8} {completed.Count,-12} {revenue,-14:C} {avg:C}");
            }
            Console.WriteLine($"\n   {"TOTAL",-28} {DataBase.Orders.Count,-8} {DataBase.Orders.Count(o => o.OrderStatus == OrderStatus.Completed),-12} {DataBase.Orders.Where(o => o.OrderStatus == OrderStatus.Completed).Sum(o => o.TotalAmount):C}");
            ConsoleHelper.Wait();
        }

        private static void OrderStatusSummary()
        {
            ConsoleHelper.PrintHeader("Order Status Summary");
            var statuses = Enum.GetValues<OrderStatus>();
            Console.WriteLine($"\n   {"Status",-15} {"Count",-8} {"Bar"}");
            Console.WriteLine("   " + new string('─', 45));
            int total = DataBase.Orders.Count;
            foreach (var s in statuses)
            {
                int count = DataBase.Orders.Count(o => o.OrderStatus == s);
                int bar = total > 0 ? (int)(count * 30.0 / total) : 0;
                var color = s switch
                {
                    OrderStatus.Completed => ConsoleColor.Green,
                    OrderStatus.Canceled => ConsoleColor.DarkRed,
                    OrderStatus.Served => ConsoleColor.Yellow,
                    OrderStatus.Preparing => ConsoleColor.Cyan,
                    _ => ConsoleColor.White
                };
                Console.Write($"   {s,-15} {count,-8} ");
                Console.ForegroundColor = color;
                Console.WriteLine(new string('█', bar));
                Console.ResetColor();
            }
            ConsoleHelper.Wait();
        }

        private static void TopMenuItems()
        {
            ConsoleHelper.PrintHeader("Top Menu Items by Quantity Sold");
            var sold = DataBase.Orders
                .Where(o => o.OrderStatus == OrderStatus.Completed)
                .SelectMany(o => o.Items)
                .GroupBy(i => i.ItemId)
                .Select(g => (ItemId: g.Key, Qty: g.Sum(i => i.Quantity), Revenue: g.Sum(i => i.Quantity * i.UnitPrice)))
                .OrderByDescending(x => x.Qty).Take(10).ToList();

            if (!sold.Any()) { ConsoleHelper.Warning("No completed orders yet."); ConsoleHelper.Wait(); return; }

            Console.WriteLine($"\n   {"#",-4} {"Item",-24} {"Qty Sold",-10} {"Revenue"}");
            Console.WriteLine("   " + new string('─', 50));
            int rank = 1;
            foreach (var (itemId, qty, revenue) in sold)
            {
                var mi = DataBase.MenuItems.FirstOrDefault(m => m.ItemId == itemId);
                Console.WriteLine($"   {rank++,-4} {mi?.Name ?? "?",-24} {qty,-10} {revenue:C}");
            }
            ConsoleHelper.Wait();
        }

        private static void LoyaltyLeaderboard()
        {
            ConsoleHelper.PrintHeader("Customer Loyalty Leaderboard");
            var sorted = DataBase.Customers.OrderByDescending(c => c.LoyaltyPoints).ToList();
            Console.WriteLine($"\n   {"#",-4} {"Customer",-24} {"Points"}");
            Console.WriteLine("   " + new string('─', 35));
            int rank = 1;
            foreach (var c in sorted)
            {
                Console.ForegroundColor = rank == 1 ? ConsoleColor.Yellow : rank == 2 ? ConsoleColor.Gray : rank == 3 ? ConsoleColor.DarkYellow : ConsoleColor.White;
                Console.WriteLine($"   {rank++,-4} {c.FullName,-24} {c.LoyaltyPoints}");
                Console.ResetColor();
            }
            ConsoleHelper.Wait();
        }

        private static void InventoryOverview()
        {
            ConsoleHelper.PrintHeader("Inventory Status Overview");
            foreach (var b in DataBase.Branches)
            {
                ConsoleHelper.PrintSubHeader(b.Name);
                foreach (var ing in DataBase.Ingredients)
                {
                    var inv = DataBase.BranchInventories.FirstOrDefault(i => i.BranchId == b.BranchId && i.IngredientId == ing.IngredientId);
                    double qty = inv?.CurrentQuantity ?? 0;
                    Console.ForegroundColor = qty <= 0 ? ConsoleColor.Red : qty < 2 ? ConsoleColor.Yellow : ConsoleColor.Green;
                    Console.WriteLine($"   {ing.Name,-22} {qty:F2} {ing.Unit}");
                    Console.ResetColor();
                }
            }
            ConsoleHelper.Wait();
        }

        private static void FeedbackSummary()
        {
            ConsoleHelper.PrintHeader("Feedback Rating Summary");
            if (!DataBase.Feedbacks.Any()) { ConsoleHelper.Warning("No feedback yet."); ConsoleHelper.Wait(); return; }

            double avg = DataBase.Feedbacks.Average(f => f.Rate);
            Console.WriteLine($"\n   Total Feedback Entries : {DataBase.Feedbacks.Count}");
            Console.WriteLine($"   Average Rating         : {avg:F2} / 5  {ConsoleHelper.Stars((int)Math.Round(avg))}");
            Console.WriteLine();
            for (int r = 5; r >= 1; r--)
            {
                int cnt = DataBase.Feedbacks.Count(f => f.Rate == r);
                int bar = (int)(cnt * 25.0 / DataBase.Feedbacks.Count);
                Console.Write($"   {r}★  ");
                Console.ForegroundColor = r >= 4 ? ConsoleColor.Green : r == 3 ? ConsoleColor.Yellow : ConsoleColor.DarkRed;
                Console.WriteLine(new string('█', bar) + $"  {cnt}");
                Console.ResetColor();
            }
            ConsoleHelper.Wait();
        }
    }
    }

