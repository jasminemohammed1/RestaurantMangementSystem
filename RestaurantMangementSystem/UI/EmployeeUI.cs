using RestaurantMangementSystem.Data;
using RestaurantMangementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.UI
{
    public static class EmployeeUI
    {
        public static void Run()
        {
            while (true)
            {
                ConsoleHelper.PrintHeader("Employee Management");
                ConsoleHelper.PrintOption("1", "List All Employees");
                ConsoleHelper.PrintOption("2", "View Employee Details");
                ConsoleHelper.PrintOption("3", "Add New Employee");
                ConsoleHelper.PrintOption("4", "Assign Employee to Branch");
                ConsoleHelper.PrintOption("5", "Delivery Staff");
                ConsoleHelper.PrintOption("6", "Shift Schedules");
                ConsoleHelper.PrintOption("0", "Back");

                switch (ConsoleHelper.ReadString("Choice"))
                {
                    case "1": ListEmployees(); break;
                    case "2": ViewEmployee(); break;
                    case "3": AddEmployee(); break;
                    case "4": AssignToBranch(); break;
                    case "5": DeliveryStaffMenu(); break;
                    case "6": ShiftMenu(); break;
                    case "0": return;
                    default: ConsoleHelper.Error("Invalid option."); break;
                }
            }
        }

        private static void ListEmployees()
        {
            ConsoleHelper.PrintHeader("All Employees");
            Console.WriteLine($"\n   {"ID",-5} {"Name",-24} {"Role",-16} {"Salary",-10} {"Branch"}");
            Console.WriteLine("   " + new string('─', 65));
            foreach (var e in DataBase.Employees)
            {
                var branch = DataBase.Branches.FirstOrDefault(b => b.BranchId == e.PrimaryBranchId);
                Console.WriteLine($"   {e.EmployeeId,-5} {e.FullName,-24} {e.GetRole(),-16} {e.Salary,-10:C} {branch?.Name ?? "—"}");
            }
            ConsoleHelper.Wait();
        }

        private static void ViewEmployee()
        {
            ConsoleHelper.PrintHeader("Employee Details");
            int id = ConsoleHelper.ReadInt("Employee ID");
            var e = DataBase.Employees.FirstOrDefault(x => x.EmployeeId == id);
            if (e is null) { ConsoleHelper.Error("Employee not found."); ConsoleHelper.Wait(); return; }

            ConsoleHelper.PrintSubHeader("Profile");
            ConsoleHelper.Row("ID:", e.EmployeeId);
            ConsoleHelper.Row("Name:", e.FullName);
            ConsoleHelper.Row("Role:", e.GetRole());
            ConsoleHelper.Row("Salary:", e.Salary.ToString("C"));
            ConsoleHelper.Row("Date of Hire:", e.DateOfHire.ToString("dd MMM yyyy"));
            ConsoleHelper.Row("Contact:", e.ContactInfo);
            ConsoleHelper.Row("Primary Branch:",
                DataBase.Branches.FirstOrDefault(b => b.BranchId == e.PrimaryBranchId)?.Name ?? "—");

            ConsoleHelper.PrintSubHeader("Assigned Branches");
            foreach (var bid in e.AssignedBranchIds)
            {
                var b = DataBase.Branches.FirstOrDefault(x => x.BranchId == bid);
                ConsoleHelper.Info(b?.Name ?? $"Branch #{bid}");
            }
            ConsoleHelper.Wait();
        }

        private static void AddEmployee()
        {
            ConsoleHelper.PrintHeader("Add New Employee");
            ConsoleHelper.PrintSubHeader("Select Role");
            ConsoleHelper.PrintOption("1", "Chef");
            ConsoleHelper.PrintOption("2", "Waiter");
            ConsoleHelper.PrintOption("3", "Cashier");
            ConsoleHelper.PrintOption("4", "Branch Manager");

            int role = ConsoleHelper.ReadInt("Role", 1, 4);

            string name = ConsoleHelper.ReadString("Full Name");
            decimal salary = ConsoleHelper.ReadDecimal("Salary");
            string contact = ConsoleHelper.ReadString("Contact Info");

            // List branches
            foreach (var b in DataBase.Branches) ConsoleHelper.Info($"[{b.BranchId}] {b.Name}");
            int branchId = ConsoleHelper.ReadInt("Primary Branch ID");
            if (!DataBase.Branches.Any(b => b.BranchId == branchId))
            { ConsoleHelper.Error("Branch not found."); ConsoleHelper.Wait(); return; }

            int newId = DataBase.NextEmployeeId++;
            Employee emp = role switch
            {
                1 => new Chef(),
                2 => new Waiter(),
                3 => new Cashier(),
                _ => new BranchManager()
            };
            emp.EmployeeId = newId;
            emp.FullName = name;
            emp.Salary = salary;
            emp.DateOfHire = DateTime.Today;
            emp.ContactInfo = contact;
            emp.PrimaryBranchId = branchId;
            emp.AssignedBranchIds.Add(branchId);

            DataBase.Employees.Add(emp);
            ConsoleHelper.Success($"Employee '{name}' added with ID {newId}.");
            ConsoleHelper.Wait();
        }

        private static void AssignToBranch()
        {
            ConsoleHelper.PrintHeader("Assign Employee to Branch");
            int empId = ConsoleHelper.ReadInt("Employee ID");
            var emp = DataBase.Employees.FirstOrDefault(e => e.EmployeeId == empId);
            if (emp is null) { ConsoleHelper.Error("Employee not found."); ConsoleHelper.Wait(); return; }

            foreach (var b in DataBase.Branches) ConsoleHelper.Info($"[{b.BranchId}] {b.Name}");
            int branchId = ConsoleHelper.ReadInt("Branch ID");
            if (!DataBase.Branches.Any(b => b.BranchId == branchId))
            { ConsoleHelper.Error("Branch not found."); ConsoleHelper.Wait(); return; }

            if (emp.AssignedBranchIds.Contains(branchId))
            { ConsoleHelper.Warning("Employee is already assigned to this branch."); ConsoleHelper.Wait(); return; }

            emp.AssignedBranchIds.Add(branchId);
            ConsoleHelper.Success($"{emp.FullName} assigned to branch #{branchId}.");
            ConsoleHelper.Wait();
        }

        private static void DeliveryStaffMenu()
        {
            while (true)
            {
                ConsoleHelper.PrintHeader("Delivery Staff");
                ConsoleHelper.PrintOption("1", "List Delivery Staff");
                ConsoleHelper.PrintOption("2", "Add Delivery Staff");
                ConsoleHelper.PrintOption("0", "Back");

                switch (ConsoleHelper.ReadString("Choice"))
                {
                    case "1":
                        ConsoleHelper.PrintHeader("Delivery Staff List");
                        Console.WriteLine($"\n   {"ID",-5} {"Name",-22} {"Vehicle",-14} {"License",-12} {"Area",-18} {"Branch",-16} {"Available"}");
                        Console.WriteLine("   " + new string('─', 95));
                        foreach (var s in DataBase.DeliveryStaffList)
                        {
                            var b = DataBase.Branches.FirstOrDefault(x => x.BranchId == s.BranchId);
                            Console.ForegroundColor = s.IsAvailable ? ConsoleColor.White : ConsoleColor.DarkGray;
                            Console.WriteLine($"   {s.StaffId,-5} {s.FullName,-22} {s.VehicleType,-14} {s.LicenseNumber,-12} {s.AssignedArea,-18} {b?.Name ?? "—",-16} {(s.IsAvailable ? "Yes" : "No")}");
                            Console.ResetColor();
                        }
                        ConsoleHelper.Wait();
                        break;
                    case "2":
                        ConsoleHelper.PrintHeader("Add Delivery Staff");
                        int newId = DataBase.DeliveryStaffList.Any() ? DataBase.DeliveryStaffList.Max(s => s.StaffId) + 1 : 1;
                        string dname = ConsoleHelper.ReadString("Full Name");
                        string vtype = ConsoleHelper.ReadString("Vehicle Type");
                        string lic = ConsoleHelper.ReadString("License Number");
                        string area = ConsoleHelper.ReadString("Assigned Area");
                        foreach (var b in DataBase.Branches) ConsoleHelper.Info($"[{b.BranchId}] {b.Name}");
                        int bid = ConsoleHelper.ReadInt("Branch ID");
                        DataBase.DeliveryStaffList.Add(new DeliveryStaff
                        {
                            StaffId = newId,
                            FullName = dname,
                            VehicleType = vtype,
                            LicenseNumber = lic,
                            AssignedArea = area,
                            BranchId = bid,
                            IsAvailable = true
                        });
                        ConsoleHelper.Success($"Delivery staff '{dname}' added.");
                        ConsoleHelper.Wait();
                        break;
                    case "0": return;
                }
            }
        }

        private static void ShiftMenu()
        {
            while (true)
            {
                ConsoleHelper.PrintHeader("Shift Schedules");
                ConsoleHelper.PrintOption("1", "View Shifts by Branch");
                ConsoleHelper.PrintOption("2", "Add Shift");
                ConsoleHelper.PrintOption("0", "Back");

                switch (ConsoleHelper.ReadString("Choice"))
                {
                    case "1":
                        foreach (var b in DataBase.Branches) ConsoleHelper.Info($"[{b.BranchId}] {b.Name}");
                        int bId = ConsoleHelper.ReadInt("Branch ID");
                        var shifts = DataBase.ShiftSchedules.Where(s => s.BranchId == bId).ToList();
                        ConsoleHelper.PrintHeader($"Shifts — Branch #{bId}");
                        if (!shifts.Any()) { ConsoleHelper.Warning("No shifts found."); }
                        else
                        {
                            Console.WriteLine($"\n   {"ID",-5} {"Employee",-24} {"Date",-14} {"Slot"}");
                            Console.WriteLine("   " + new string('─', 55));
                            foreach (var s in shifts)
                            {
                                var emp = DataBase.Employees.FirstOrDefault(e => e.EmployeeId == s.EmployeeId);
                                Console.WriteLine($"   {s.ScheduleId,-5} {emp?.FullName ?? "?",-24} {s.Date:dd MMM yyyy,-14} {s.TimeSlot}");
                            }
                        }
                        ConsoleHelper.Wait();
                        break;
                    case "2":
                        foreach (var b in DataBase.Branches) ConsoleHelper.Info($"[{b.BranchId}] {b.Name}");
                        int branchId = ConsoleHelper.ReadInt("Branch ID");
                        ListEmployees();
                        int empId = ConsoleHelper.ReadInt("Employee ID");
                        string slot = ConsoleHelper.ReadString("Time Slot (e.g. 08:00-16:00)");
                        DataBase.ShiftSchedules.Add(new ShiftSchedule
                        {
                            ScheduleId = DataBase.NextScheduleId++,
                            BranchId = branchId,
                            EmployeeId = empId,
                            Date = DateTime.Today,
                            TimeSlot = slot
                        });
                        ConsoleHelper.Success("Shift added.");
                        ConsoleHelper.Wait();
                        break;
                    case "0": return;
                }
            }
        }
    }
    }

