using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurauntApp.Models;
using RestaurauntApp.Models.Other;
using RestaurauntApp.Services.Classes;

namespace RestaurauntApp.Data
{
    public class RestaurantAppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public RestaurantAppDbContext(DbContextOptions<RestaurantAppDbContext> options) : base(options) { }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<LogEntry> logEntries { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
    }
}