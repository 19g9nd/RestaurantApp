using Microsoft.EntityFrameworkCore;
using RestaurauntApp.Data;
using RestaurauntApp.DTOS;
using RestaurauntApp.Models;
using RestaurauntApp.Models.Other;
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

        public async Task<Order> GetUncompleteOrderWithItems(string userName) //для того чтобы в корзине не отображались завершенные заказы так как сущность корзины = заказу
        {
            var order = await context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.UserName == userName && o.OrderState == EnumOrderState.waiting);
            return order;
        }
        public async Task<bool> AddToOrder(OrderItemDTO orderItemDTO, string userName)
        {
            try
            {
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

        public async Task<bool> RemoveFromOrder(int itemId, string userName)
        {
            try
            {
                // Поиск заказа пользователя
                var order = await context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.UserName == userName);

                if (order == null)
                {
                    Console.WriteLine($"Order not found for user: {userName}");
                    return false; // Заказ не найден
                }

                // Поиск элемента заказа для удаления
                var existingOrderItem = order.OrderItems.FirstOrDefault(oi => oi.Id == itemId);

                if (existingOrderItem == null)
                {
                    Console.WriteLine($"Item not found in the order");
                    return false; // Элемент не найден в заказе
                }

                // Вычисление изменения в общей стоимости заказа
                decimal totalPriceChange = existingOrderItem.Price * existingOrderItem.Quantity;

                // Удаление элемента заказа
                order.OrderItems.Remove(existingOrderItem);
                context.OrderItems.Remove(existingOrderItem); // Удаляем из контекста, чтобы также удалить из базы данных

                // Обновление общей стоимости заказа
                order.TotalPrice -= totalPriceChange;

                // Сохранение изменений
                await context.SaveChangesAsync();

                return true; // Успешно удалено
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing item from order: {ex.Message}");
                return false;
            }
        }


    }
}