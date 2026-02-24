<div align="center">

# 🍽️ Restaurant App

**A full-stack restaurant ordering & management platform**  
**built with ASP.NET Core MVC + PostgreSQL**

[![Live Demo](https://img.shields.io/badge/🚀%20Live%20Demo-Visit%20App-orange?style=for-the-badge)](https://restaurantapp-1-5knc.onrender.com)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core%20MVC-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white)

---

https://github.com/user-attachments/assets/0c1f1b37-09d1-47a4-bb80-d87018207b39

</div>

---

## ✨ Features

### 🧾 Restaurant Menu
- Browse dishes with names, descriptions, images, and prices
- Organized by categories for easy navigation

### 🛒 Cart (API-driven)
- Add, remove, and adjust item quantities — stored in `localStorage` and synced to the backend via REST API
- Real-time total price fetched from the server on every update

### 🎟️ Coupons & Discounts
- Enter a discount code at checkout to apply a price reduction
- Invalid or already-used codes are rejected with a friendly error (powered by **SweetAlert2**)
- Admins can create and manage coupon codes through the admin panel

### 📦 Order Tracking
- Order status progresses through stages: **Placed → Preparing → Ready → Delivered**
- Customers can follow their order's live status at any time

### 🔐 Admin Panel
- Manage menu items (create, edit, delete)
- View and update all incoming orders
- Manage coupon codes and their usage
- Full order history overview

### ⚙️ Other
- Auto-applies database migrations on startup (`Program.cs`)
- Custom middleware pipeline
- Clean architecture with **Repository pattern**, **Services layer**, and **DTOs**

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Language | C# |
| Framework | ASP.NET Core MVC |
| ORM | Entity Framework Core |
| Database | PostgreSQL (via **Npgsql**) |
| Frontend | Razor Views / Bootstrap / Vanilla JS |
| Cart Storage | `localStorage` + REST API sync |
| Alerts | SweetAlert2 |
| Hosting | Render |

---

## 🏗️ Architecture

```
RestaurauntApp/
├── Controllers/        # MVC Controllers (Cart, Menu, Orders, Admin...)
├── Views/              # Razor views (.cshtml)
├── Models/             # Domain models (Order, MenuItem, Coupon...)
├── DTOS/               # Data Transfer Objects
├── Data/               # EF Core DbContext
├── Migrations/         # EF Core database migrations
├── Repositories/       # Repository pattern (data access layer)
├── Services/           # Business logic layer
├── Extensions/         # Service registration & app extensions
├── Middlewares/        # Custom middleware
├── wwwroot/            # Static files (CSS, JS, images)
└── Program.cs          # Entry point — auto-applies migrations on startup
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) `8.0+`
- A PostgreSQL instance (local or remote)

### Installation

```bash
# 1. Clone the repository
git clone https://github.com/19g9nd/RestaurantApp.git
cd RestaurauntApp
```

```bash
# 2. Set your PostgreSQL connection string in appsettings.Development.json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=restaurantdb;Username=postgres;Password=yourpassword"
}
```

```bash
# 3. Restore dependencies
dotnet restore

# 4. Run — migrations are applied automatically on startup
dotnet run
```

Open [https://localhost:5001](https://localhost:5001) in your browser.

> 💡 No need to run `dotnet ef database update` manually — the app handles it on startup.

---

## 🛒 How the Cart Works

The cart uses a hybrid client/server approach:

1. Items are saved to **`localStorage`** for instant UI feedback
2. On cart page load, the full cart syncs to the server via `POST /Cart/AddToOrder`
3. Total price is calculated server-side and fetched via `GET /Cart/GetTotalPrice`
4. Individual items are removed via `DELETE /Cart/RemoveFromOrder`
5. Discount codes are validated server-side via `POST /Cart/ApplyDiscount`

---

## 🌐 Live Demo

**[restaurantapp-1-5knc.onrender.com](https://restaurantapp-1-5knc.onrender.com)**

---

## 📄 License

This project is open source and available under the [MIT License](LICENSE).

---

<div align="center">
  Built with ❤️ using C#, ASP.NET Core MVC & PostgreSQL
  <br/><br/>
  <a href="https://restaurantapp-1-5knc.onrender.com">🍕 Try it live!</a>
</div>
