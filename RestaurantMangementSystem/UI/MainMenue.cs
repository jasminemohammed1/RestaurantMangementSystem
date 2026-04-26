using RestaurantMangementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.UI
{
   public static  class MainMenue
    {
         public static void Run()
        {
            Console.OutputEncoding = Encoding.UTF8;
            while(true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine();
                Console.WriteLine("  ╔══════════════════════════════════════════════════════╗");
                Console.WriteLine("  ║         RESTAURANT MANAGEMENT SYSTEM                 ║");
                Console.WriteLine("  ║                  Main Menu                           ║");
                Console.WriteLine("  ╚══════════════════════════════════════════════════════╝");
                Console.ResetColor();
                Console.WriteLine();
                ConsoleHelper.PrintOption("1", "Branch Management");
                ConsoleHelper.PrintOption("2", "Employee Management");
                ConsoleHelper.PrintOption("3", "Customer Management");
                ConsoleHelper.PrintOption("4", "Menu Management");
                ConsoleHelper.PrintOption("5", "Order Management");
                ConsoleHelper.PrintOption("6", "Delivery Management");
                ConsoleHelper.PrintOption("7", "Inventory Management");
                ConsoleHelper.PrintOption("8", "Feedback");
                ConsoleHelper.PrintOption("9", "Reports & Analytics");
                Console.WriteLine();
                ConsoleHelper.PrintOption("0", "Exit");

                var choice = ConsoleHelper.ReadString("Choice");
                switch(choice)
                {
                    case "1": BranchUi.Run(); break;
                    case "2": EmployeeUI.Run(); break;
                    case "3": CustomerUi.Run(); break;
                    case "4": MenueUI.Run(); break;
                    case "5": OrderUI.Run(); break;
                    case "6": DeliveryUI.Run(); break;
                    case "7": InventoryUI.Run(); break;
                    case "8": FeedBackUI.Run(); break;
                    case "9": ReportsUI.Run(); break;
                    case "0":
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\n  Goodbye!\n");
                        Console.ResetColor();
                        return;
                    default:
                        ConsoleHelper.Error("Invalid option. Please choose 0–9.");
                        ConsoleHelper.Wait();
                        break;
                }

            }
        }
    }
}
