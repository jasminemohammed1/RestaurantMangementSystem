using RestaurantMangementSystem.Data;
using RestaurantMangementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.UI
{
    public static class MenueUI
    {
        public static void Run()
        {
            while (true)
            {
                ConsoleHelper.PrintHeader("Menu Management");
                ConsoleHelper.PrintOption("1", "List All Menu Items");
                ConsoleHelper.PrintOption("2", "View Item Details");
                ConsoleHelper.PrintOption("3", "Add Menu Item");
                ConsoleHelper.PrintOption("4", "Add Add-On to Item");
                ConsoleHelper.PrintOption("5", "Manage Ingredients & Recipes");
                ConsoleHelper.PrintOption("0", "Back");

                switch (ConsoleHelper.ReadString("Choice"))
                {
                    case "1": ListMenuItems(); break;
                    case "2": ViewItem(); break;
                    case "3": AddItem(); break;
                    case "4": AddAddOn(); break;
                    case "5": RecipeMenu(); break;
                    case "0": return;
                    default: ConsoleHelper.Error("Invalid option."); break;
                }
            }
        }

        public static void ListMenuItems()
        {
            ConsoleHelper.PrintHeader("All Menu Items");
            Console.WriteLine($"\n   {"ID",-5} {"Name",-22} {"Category",-14} {"Price",-10} {"Add-Ons"}");
            Console.WriteLine("   " + new string('─', 65));
            foreach (var m in DataBase.MenuItems)
            {
                string addOns = m.AddOns.Any() ? string.Join(", ", m.AddOns.Select(a => a.Name)) : "—";
                Console.WriteLine($"   {m.ItemId,-5} {m.Name,-22} {m.Category,-14} {m.BasePrice,-10:C} {addOns}");
            }
            ConsoleHelper.Wait();
        }

        private static void ViewItem()
        {
            ConsoleHelper.PrintHeader("Menu Item Details");
            int id = ConsoleHelper.ReadInt("Item ID");
            var m = DataBase.MenuItems.FirstOrDefault(x => x.ItemId == id);
            if (m is null) { ConsoleHelper.Error("Item not found."); ConsoleHelper.Wait(); return; }

            ConsoleHelper.Row("ID:", m.ItemId);
            ConsoleHelper.Row("Name:", m.Name);
            ConsoleHelper.Row("Description:", m.Description);
            ConsoleHelper.Row("Category:", m.Category);
            ConsoleHelper.Row("Base Price:", m.BasePrice.ToString("C"));

            if (m.AddOns.Any())
            {
                ConsoleHelper.PrintSubHeader("Add-Ons");
                foreach (var a in m.AddOns)
                    ConsoleHelper.Info($"[{a.AddOnId}] {a.Name,-20} +{a.ExtraPrice:C}");
            }

            ConsoleHelper.PrintSubHeader("Recipe (Ingredients per Serving)");
            var recipes = DataBase.RecipeItems.Where(r => r.MenuItemId == id).ToList();
            if (!recipes.Any()) { ConsoleHelper.Info("No tracked ingredients."); }
            else
                foreach (var r in recipes)
                {
                    var ing = DataBase.Ingredients.FirstOrDefault(i => i.IngredientId == r.IngredientId);
                    ConsoleHelper.Info($"{ing?.Name ?? "?",-20} {r.QuantityRequired} {ing?.Unit}");
                }

            ConsoleHelper.PrintSubHeader("Branch Availability");
            foreach (var b in DataBase.Branches)
            {
                var bmi = DataBase.BranchMenuItems.FirstOrDefault(x => x.BranchId == b.BranchId && x.ItemId == id);
                string avail = bmi?.IsAvailable == true ? "Available" : "Unavailable";
                string price = bmi?.PriceOverride.HasValue == true ? $"Override: {bmi.PriceOverride:C}" : "Base price";
                ConsoleHelper.Info($"{b.Name,-25} {avail,-12} {price}");
            }
            ConsoleHelper.Wait();
        }

        private static void AddItem()
        {
            ConsoleHelper.PrintHeader("Add New Menu Item");
            int newId = DataBase.MenuItems.Any() ? DataBase.MenuItems.Max(m => m.ItemId) + 1 : 1;
            string name = ConsoleHelper.ReadString("Name");
            string desc = ConsoleHelper.ReadString("Description");
            decimal price = ConsoleHelper.ReadDecimal("Base Price");
            ConsoleHelper.Info("Categories: Appetizer | Main Course | Dessert | Beverage");
            string cat = ConsoleHelper.ReadString("Category");

            DataBase.MenuItems.Add(new MenuItem { ItemId = newId, Name = name, Description = desc, BasePrice = price, Category = cat });

            // Auto-create branch menu entries
            foreach (var b in DataBase.Branches)
                DataBase.BranchMenuItems.Add(new BranchMenuItem { BranchId = b.BranchId, ItemId = newId, IsAvailable = true });

            ConsoleHelper.Success($"Menu item '{name}' added with ID {newId}. Available at all branches by default.");
            ConsoleHelper.Wait();
        }

        private static void AddAddOn()
        {
            ConsoleHelper.PrintHeader("Add Add-On to Menu Item");
            int itemId = ConsoleHelper.ReadInt("Menu Item ID");
            var item = DataBase.MenuItems.FirstOrDefault(m => m.ItemId == itemId);
            if (item is null) { ConsoleHelper.Error("Item not found."); ConsoleHelper.Wait(); return; }

            int newId = item.AddOns.Any() ? item.AddOns.Max(a => a.AddOnId) + 1 : 1;
            string name = ConsoleHelper.ReadString("Add-On Name");
            decimal extra = ConsoleHelper.ReadDecimal("Extra Price");
            item.AddOns.Add(new AddOn { AddOnId = newId, Name = name, ExtraPrice = extra });
            ConsoleHelper.Success($"Add-on '{name}' added to {item.Name}.");
            ConsoleHelper.Wait();
        }

        private static void RecipeMenu()
        {
            while (true)
            {
                ConsoleHelper.PrintHeader("Ingredients & Recipes");
                ConsoleHelper.PrintOption("1", "List Ingredients");
                ConsoleHelper.PrintOption("2", "Add Ingredient");
                ConsoleHelper.PrintOption("3", "Add Recipe Line");
                ConsoleHelper.PrintOption("0", "Back");

                switch (ConsoleHelper.ReadString("Choice"))
                {
                    case "1":
                        ConsoleHelper.PrintHeader("Ingredients");
                        Console.WriteLine($"\n   {"ID",-5} {"Name",-22} {"Unit"}");
                        Console.WriteLine("   " + new string('─', 35));
                        foreach (var i in DataBase.Ingredients)
                            Console.WriteLine($"   {i.IngredientId,-5} {i.Name,-22} {i.Unit}");
                        ConsoleHelper.Wait();
                        break;
                    case "2":
                        ConsoleHelper.PrintHeader("Add Ingredient");
                        int newId = DataBase.Ingredients.Any() ? DataBase.Ingredients.Max(i => i.IngredientId) + 1 : 1;
                        string iname = ConsoleHelper.ReadString("Name");
                        ConsoleHelper.Info("Units: kg | liter | piece");
                        string unit = ConsoleHelper.ReadString("Unit");
                        DataBase.Ingredients.Add(new Ingredient { IngredientId = newId, Name = iname, Unit = unit });
                        ConsoleHelper.Success($"Ingredient '{iname}' added.");
                        ConsoleHelper.Wait();
                        break;
                    case "3":
                        ConsoleHelper.PrintHeader("Add Recipe Line");
                        ListMenuItems();
                        int itemId = ConsoleHelper.ReadInt("Menu Item ID");
                        int ingId = ConsoleHelper.ReadInt("Ingredient ID");
                        Console.Write("\n  Quantity per Serving: ");
                        if (double.TryParse(Console.ReadLine(), out double qty))
                        {
                            DataBase.RecipeItems.Add(new RecipeItem { MenuItemId = itemId, IngredientId = ingId, QuantityRequired = qty });
                            ConsoleHelper.Success("Recipe line added.");
                        }
                        else ConsoleHelper.Error("Invalid quantity.");
                        ConsoleHelper.Wait();
                        break;
                    case "0": return;
                }
            }
        }
    }
    }

