using RestaurantMangementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Data
{
    public static class DataBase
    {
        public static List<Branch> Branches { get; } = new();
        public static List<Employee> Employees { get; } = new();
        public static List<DeliveryStaff> DeliveryStaffList { get; } = new();
        public static List<Customer> Customers { get; } = new();
        public static List<MenuItem> MenuItems { get; } = new();
        public static List<BranchMenuItem> BranchMenuItems { get; } = new();
        public static List<ShiftSchedule> ShiftSchedules { get; } = new();
        public static List<Order> Orders { get; } = new();
        public static List<Ingredient> Ingredients { get; } = new();
        public static List<BranchInventory> BranchInventories { get; } = new();
        public static List<RecipeItem> RecipeItems { get; } = new();
        public static List<Delivery> Deliveries { get; } = new();
        public static List<Feedback> Feedbacks { get; } = new();

        public static int NextOrderId { get; set; } = 1;
        public static int NextOrderItemId { get; set; } = 1;
        public static int NextDeliveryId { get; set; } = 1;
        public static int NextFeedbackId { get; set; } = 1;
        public static int NextCustomerId { get; set; } = 10;
        public static int NextEmployeeId { get; set; } = 10;
        public static int NextScheduleId { get; set; } = 1;

        public static void Seed()
        {
            // ─── Branches ───────────────────────────────────────────────────────────
            Branches.AddRange(new[]
            {
            new Branch { BranchId = 1, Name = "Downtown Branch",  Address = "123 Main St, Cairo",    ContactNumber = "02-555-0101", OpeningHours = "08:00–23:00", ManagerId = 5 },
            new Branch { BranchId = 2, Name = "Mall Branch",      Address = "456 Mall Rd, Giza",     ContactNumber = "02-555-0202", OpeningHours = "10:00–22:00", ManagerId = 6 }
        });

            // ─── Employees ──────────────────────────────────────────────────────────
            Employees.AddRange(new Employee[]
            {
            new Chef         { EmployeeId = 1, FullName = "Ali Hassan",      Salary = 5000, DateOfHire = new DateTime(2020,  1, 15), ContactInfo = "ali@restaurant.com",    PrimaryBranchId = 1, AssignedBranchIds = new() { 1 } },
            new Waiter       { EmployeeId = 2, FullName = "Sara Ahmed",      Salary = 3000, DateOfHire = new DateTime(2021,  3, 10), ContactInfo = "sara@restaurant.com",   PrimaryBranchId = 1, AssignedBranchIds = new() { 1 } },
            new Cashier      { EmployeeId = 3, FullName = "Mohamed Khaled",  Salary = 3500, DateOfHire = new DateTime(2021,  5, 20), ContactInfo = "mo@restaurant.com",     PrimaryBranchId = 1, AssignedBranchIds = new() { 1 } },
            new Chef         { EmployeeId = 4, FullName = "Nour Ibrahim",    Salary = 5500, DateOfHire = new DateTime(2019,  8,  5), ContactInfo = "nour@restaurant.com",   PrimaryBranchId = 2, AssignedBranchIds = new() { 2 } },
            new BranchManager{ EmployeeId = 5, FullName = "Karim Youssef",   Salary = 8000, DateOfHire = new DateTime(2018,  2,  1), ContactInfo = "karim@restaurant.com",  PrimaryBranchId = 1, AssignedBranchIds = new() { 1 } },
            new BranchManager{ EmployeeId = 6, FullName = "Dina Samir",      Salary = 8500, DateOfHire = new DateTime(2018,  6, 15), ContactInfo = "dina@restaurant.com",   PrimaryBranchId = 2, AssignedBranchIds = new() { 2 } },
            new Waiter       { EmployeeId = 7, FullName = "Omar Fares",      Salary = 3000, DateOfHire = new DateTime(2022,  1,  1), ContactInfo = "omar@restaurant.com",   PrimaryBranchId = 2, AssignedBranchIds = new() { 2 } },
            new Cashier      { EmployeeId = 8, FullName = "Layla Nabil",     Salary = 3500, DateOfHire = new DateTime(2022,  4, 10), ContactInfo = "layla@restaurant.com",  PrimaryBranchId = 2, AssignedBranchIds = new() { 2 } },
            new Chef         { EmployeeId = 9, FullName = "Reem Fathy",      Salary = 5200, DateOfHire = new DateTime(2023,  2,  1), ContactInfo = "reem@restaurant.com",   PrimaryBranchId = 1, AssignedBranchIds = new() { 1, 2 } }
            });
            NextEmployeeId = 10;

            // ─── Delivery Staff ─────────────────────────────────────────────────────
            DeliveryStaffList.AddRange(new[]
            {
            new DeliveryStaff { StaffId = 1, FullName = "Tarek Mansour", VehicleType = "Motorcycle", LicenseNumber = "LIC-001", AssignedArea = "Downtown",     BranchId = 1, IsAvailable = true },
            new DeliveryStaff { StaffId = 2, FullName = "Rania Hassan",  VehicleType = "Bicycle",    LicenseNumber = "LIC-002", AssignedArea = "Mall District", BranchId = 2, IsAvailable = true }
        });

            // ─── Customers ──────────────────────────────────────────────────────────
            Customers.AddRange(new[]
            {
            new Customer { CustomerId = 1, FullName = "Ahmed Sayed",  PhoneNumber = "010-1111111", Email = "ahmed@email.com", LoyaltyPoints = 150 },
            new Customer { CustomerId = 2, FullName = "Mona Adel",    PhoneNumber = "010-2222222", Email = "mona@email.com",  LoyaltyPoints = 80  },
            new Customer { CustomerId = 3, FullName = "Hassan Ali",   PhoneNumber = "010-3333333", Email = "hassan@email.com",LoyaltyPoints = 0   }
        });
            NextCustomerId = 4;

            // ─── Add-Ons ─────────────────────────────────────────────────────────────
            var extraCheese = new AddOn { AddOnId = 1, Name = "Extra Cheese", ExtraPrice = 5 };
            var largeSize = new AddOn { AddOnId = 2, Name = "Large Size", ExtraPrice = 10 };
            var extraSauce = new AddOn { AddOnId = 3, Name = "Extra Sauce", ExtraPrice = 3 };

            // ─── Menu Items ──────────────────────────────────────────────────────────
            MenuItems.AddRange(new[]
            {
            new MenuItem { ItemId = 1, Name = "Spring Rolls",    Description = "Crispy veggie rolls",        BasePrice = 25, Category = "Appetizer",    AddOns = new() { extraSauce } },
            new MenuItem { ItemId = 2, Name = "Grilled Chicken", Description = "Charcoal grilled chicken",   BasePrice = 75, Category = "Main Course",  AddOns = new() { extraCheese, largeSize } },
            new MenuItem { ItemId = 3, Name = "Chocolate Cake",  Description = "Rich chocolate dessert",     BasePrice = 40, Category = "Dessert" },
            new MenuItem { ItemId = 4, Name = "Fresh Juice",     Description = "Seasonal fruit juice",       BasePrice = 20, Category = "Beverage",     AddOns = new() { largeSize } },
            new MenuItem { ItemId = 5, Name = "Caesar Salad",    Description = "Classic caesar salad",       BasePrice = 35, Category = "Appetizer" },
            new MenuItem { ItemId = 6, Name = "Beef Burger",     Description = "Juicy beef patty burger",    BasePrice = 65, Category = "Main Course",  AddOns = new() { extraCheese, extraSauce } }
        });

            // ─── Branch Menu Items ───────────────────────────────────────────────────
            BranchMenuItems.AddRange(new[]
            {
            // Branch 1 — Caesar Salad unavailable; Grilled Chicken has price override
            new BranchMenuItem { BranchId = 1, ItemId = 1, IsAvailable = true  },
            new BranchMenuItem { BranchId = 1, ItemId = 2, IsAvailable = true,  PriceOverride = 80 },
            new BranchMenuItem { BranchId = 1, ItemId = 3, IsAvailable = true  },
            new BranchMenuItem { BranchId = 1, ItemId = 4, IsAvailable = true  },
            new BranchMenuItem { BranchId = 1, ItemId = 5, IsAvailable = false },
            new BranchMenuItem { BranchId = 1, ItemId = 6, IsAvailable = true  },
            // Branch 2 — Beef Burger unavailable; Chocolate Cake has price override
            new BranchMenuItem { BranchId = 2, ItemId = 1, IsAvailable = true  },
            new BranchMenuItem { BranchId = 2, ItemId = 2, IsAvailable = true  },
            new BranchMenuItem { BranchId = 2, ItemId = 3, IsAvailable = true,  PriceOverride = 45 },
            new BranchMenuItem { BranchId = 2, ItemId = 4, IsAvailable = true  },
            new BranchMenuItem { BranchId = 2, ItemId = 5, IsAvailable = true  },
            new BranchMenuItem { BranchId = 2, ItemId = 6, IsAvailable = false }
        });

            // ─── Ingredients ─────────────────────────────────────────────────────────
            Ingredients.AddRange(new[]
            {
            new Ingredient { IngredientId = 1, Name = "Chicken",      Unit = "kg"     },
            new Ingredient { IngredientId = 2, Name = "Flour",        Unit = "kg"     },
            new Ingredient { IngredientId = 3, Name = "Tomato Sauce", Unit = "liter"  },
            new Ingredient { IngredientId = 4, Name = "Cheese",       Unit = "kg"     },
            new Ingredient { IngredientId = 5, Name = "Beef Patty",   Unit = "piece"  },
            new Ingredient { IngredientId = 6, Name = "Lettuce",      Unit = "kg"     },
            new Ingredient { IngredientId = 7, Name = "Chocolate",    Unit = "kg"     }
        });

            // ─── Branch Inventory ─────────────────────────────────────────────────────
            BranchInventories.AddRange(new[]
            {
            new BranchInventory { BranchId = 1, IngredientId = 1, CurrentQuantity = 20 },
            new BranchInventory { BranchId = 1, IngredientId = 2, CurrentQuantity = 15 },
            new BranchInventory { BranchId = 1, IngredientId = 3, CurrentQuantity = 10 },
            new BranchInventory { BranchId = 1, IngredientId = 4, CurrentQuantity =  8 },
            new BranchInventory { BranchId = 1, IngredientId = 5, CurrentQuantity = 30 },
            new BranchInventory { BranchId = 1, IngredientId = 6, CurrentQuantity =  5 },
            new BranchInventory { BranchId = 1, IngredientId = 7, CurrentQuantity =  6 },
            new BranchInventory { BranchId = 2, IngredientId = 1, CurrentQuantity = 25 },
            new BranchInventory { BranchId = 2, IngredientId = 2, CurrentQuantity = 20 },
            new BranchInventory { BranchId = 2, IngredientId = 3, CurrentQuantity = 12 },
            new BranchInventory { BranchId = 2, IngredientId = 4, CurrentQuantity = 10 },
            new BranchInventory { BranchId = 2, IngredientId = 5, CurrentQuantity = 25 },
            new BranchInventory { BranchId = 2, IngredientId = 6, CurrentQuantity =  8 },
            new BranchInventory { BranchId = 2, IngredientId = 7, CurrentQuantity =  4 }
        });

            // ─── Recipes ─────────────────────────────────────────────────────────────
            // Spring Rolls (1): Flour + Sauce + Lettuce
            RecipeItems.AddRange(new[]
            {
            new RecipeItem { MenuItemId = 1, IngredientId = 2, QuantityRequired = 0.10 },
            new RecipeItem { MenuItemId = 1, IngredientId = 3, QuantityRequired = 0.05 },
            new RecipeItem { MenuItemId = 1, IngredientId = 6, QuantityRequired = 0.05 },
            // Grilled Chicken (2): Chicken
            new RecipeItem { MenuItemId = 2, IngredientId = 1, QuantityRequired = 0.30 },
            // Chocolate Cake (3): Flour + Chocolate
            new RecipeItem { MenuItemId = 3, IngredientId = 2, QuantityRequired = 0.20 },
            new RecipeItem { MenuItemId = 3, IngredientId = 7, QuantityRequired = 0.10 },
            // Fresh Juice (4): no tracked ingredients
            // Caesar Salad (5): Lettuce + Cheese
            new RecipeItem { MenuItemId = 5, IngredientId = 6, QuantityRequired = 0.15 },
            new RecipeItem { MenuItemId = 5, IngredientId = 4, QuantityRequired = 0.05 },
            // Beef Burger (6): Beef Patty + Lettuce + Sauce
            new RecipeItem { MenuItemId = 6, IngredientId = 5, QuantityRequired = 1.00 },
            new RecipeItem { MenuItemId = 6, IngredientId = 6, QuantityRequired = 0.05 },
            new RecipeItem { MenuItemId = 6, IngredientId = 3, QuantityRequired = 0.03 }
        });

            // ─── Shift Schedules (sample) ─────────────────────────────────────────────
            ShiftSchedules.AddRange(new[]
            {
            new ShiftSchedule { ScheduleId = 1, BranchId = 1, EmployeeId = 1, Date = DateTime.Today, TimeSlot = "08:00–16:00" },
            new ShiftSchedule { ScheduleId = 2, BranchId = 1, EmployeeId = 2, Date = DateTime.Today, TimeSlot = "10:00–18:00" },
            new ShiftSchedule { ScheduleId = 3, BranchId = 1, EmployeeId = 3, Date = DateTime.Today, TimeSlot = "12:00–20:00" }
        });
            NextScheduleId = 4;
        }
    }
}
