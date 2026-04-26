# 🍽️ Restaurant Management System

This is a **Restaurant Management System** built using C# 🧑‍💻 following strong **Object-Oriented Programming (OOP)** principles. The project simulates a real restaurant workflow including branches, employees, customers, menu items, orders, inventory management, and delivery operations.

Instead of using a real database, the system is built using an **in-memory data layer** 🗄️ inside a central `DataBase` class that acts as a fake database. This makes the project lightweight and focused on backend logic, system design, and architecture rather than external database configuration.

The main focus of this project is applying **Separation of Concerns** 🧩 to organize the system into clean layers, making the code easy to maintain, extend, and understand.

The architecture is divided into:

📦 **Models**
Represent the core entities of the system such as `Employee`, `Chef`, `Customer`, `MenuItem`, `Order`, `Branch`, and others. These classes define the structure of the restaurant system.

⚙️ **Services**
Contain the business logic of the application. For example, `InventoryService` handles stock calculation, ingredient deduction, and checking availability based on recipes and orders.

🔢 **Enums**
Used to improve code readability and replace magic values. For example, `OrderStatus` defines states like Pending, Preparing, Served, Completed, and Canceled.

🗄️ **Data (Fake Database)**
A static `DataBase` class holds all system data using in-memory collections such as Lists. It also includes a `Seed()` method that initializes fake data for branches, employees, customers, menu items, inventory, and recipes to simulate a real working system.

The system also demonstrates **inheritance and polymorphism** 🧠 through employee types like `Chef`, `Waiter`, `Cashier`, and `BranchManager`, all derived from a base `Employee` class. Each type has its own behavior such as overriding the `GetRole()` method.

One of the key parts of the system is the **Inventory Management logic** 📊. It calculates required ingredients based on order items and recipes, checks branch stock availability, identifies shortages, and deducts inventory after order processing. This simulates a real restaurant supply system.

Overall, this project demonstrates how to build a structured backend system using pure C# with OOP principles, clean separation of concerns, and realistic business logic simulation 🚀. It focuses on design, logic implementation, and system architecture rather than external frameworks or databases.
<img width="687" height="547" alt="Screenshot 2026-04-26 153239" src="https://github.com/user-attachments/assets/29d4062b-10b7-44a8-b320-0253f42bbbda" />


<img width="1299" height="734" alt="Screenshot 2026-04-26 153644" src="https://github.com/user-attachments/assets/eb280149-5694-4185-9e40-d0ce5055d1ba" />

<img width="584" height="783" alt="Screenshot 2026-04-26 153344" src="https://github.com/user-attachments/assets/73fe5f8d-9143-445d-8671-f6da8a70009e" />
  


<img width="1046" height="680" alt="Screenshot 2026-04-26 152710" src="https://github.com/user-attachments/assets/e1bc5837-aed7-4f22-85f6-298d0348ca2b" />


<img width="880" height="467" alt="Screenshot 2026-04-26 150238" src="https://github.com/user-attachments/assets/4662c445-057c-4742-bbfd-5fb63410755b" />

