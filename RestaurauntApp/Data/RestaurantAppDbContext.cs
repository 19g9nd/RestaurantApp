using Microsoft.EntityFrameworkCore;
using RestaurauntApp.Models;
using RestaurauntApp.Services.Classes;

namespace RestaurauntApp.Data
{
    public class RestaurantAppDbContext : DbContext
    {
        public RestaurantAppDbContext(DbContextOptions<RestaurantAppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<LogEntry> logEntries { get; set; }
    }
}