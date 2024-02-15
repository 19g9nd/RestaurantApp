using Microsoft.EntityFrameworkCore;
using RestaurauntApp.Data;
using RestaurauntApp.DTOS;
using RestaurauntApp.Models;
using RestaurauntApp.Repositories.Base;

namespace RestaurauntApp.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly RestaurantAppDbContext context;

        public OrderRepository(RestaurantAppDbContext context)
        {
            this.context = context;
        }

        public async Task<Order> GetOrderWithItems(string userName)
        {
            var order = await context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.UserName == userName);
            return order;
        }

        public async Task<bool> AddToOrder(OrderItemDTO orderItemDTO, string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                {
                    throw new Exception("User name is null or empty.");
                }

                // Проверка существует ли уже заказ у пользователя
                var order = await context.Orders
                   .Include(o => o.OrderItems)
                   .FirstOrDefaultAsync(o => o.UserName == userName);


                if (order == null)
                {
                    // Если заказа нет, создаем новый
                    order = new Order { UserName = userName };
                    context.Orders.Add(order);
                }

                var orderItem = new OrderItem
                {
                    MenuItemId = orderItemDTO.MenuItemId,
                    Quantity = orderItemDTO.Quantity,
                    Name = orderItemDTO.Name,
                    Price = orderItemDTO.Price,
                    UserName = userName
                };

                var existingOrderItem = order.OrderItems.FirstOrDefault(oi => oi.MenuItemId == orderItemDTO.MenuItemId);

                if (existingOrderItem != null)
                {
                    existingOrderItem.Quantity += orderItemDTO.Quantity;
                }
                else
                {
                    order.OrderItems.Add(orderItem);
                }

                order.TotalPrice += (orderItemDTO.Price * orderItemDTO.Quantity);
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding item to order: {ex.Message}");
                return false;
            }
        }

    }
}