using System.Security.Claims;
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
                // Проверка существует ли уже заказ у пользователя
                var order = await context.Orders
                   .Include(o => o.OrderItems)
                   .FirstOrDefaultAsync(o => o.UserName == userName && o.OrderState == EnumOrderState.waiting);


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
                System.Console.WriteLine(orderItem.Name);
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

        public async Task<bool> Checkout(CheckoutDTO checkoutModel, string userName)
        {
            try
            {
                var checkout = new Checkout()
                {
                    Username = userName,
                    FirstName = checkoutModel.FirstName,
                    LastName = checkoutModel.LastName,
                    Email = checkoutModel.Email,
                    Phone = checkoutModel.Phone,
                    PickupTime = checkoutModel.PickupTime,
                    CardNumber = checkoutModel.CardNumber,
                    CVV = checkoutModel.CVV,
                    Expiry = checkoutModel.Expiry
                };
                // Добавляем чекаут в контекст базы данных
                await context.Checkouts.AddAsync(checkout);
                await context.SaveChangesAsync();

                //находис заказ и меняем его статус чтобы он не отображался в корзине
                var order = await context.Orders
             .Include(o => o.OrderItems)
             .FirstOrDefaultAsync(o => o.UserName == userName && o.OrderState == EnumOrderState.waiting);

                order.CheckoutId = checkout.Id;
                order.OrderState = EnumOrderState.in_process;
                // Сохраняем изменения заказа
                await context.SaveChangesAsync();
                return true; // Успешно оформлен заказ
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false; // Ошибка при оформлении заказа
            }
        }
        public async Task<List<Order>> GetOrdersWithItems(ClaimsPrincipal user)
        {
            try
            {
                if (user.IsInRole("Admin"))
                {
                    return await context.Orders.Include(o => o.OrderItems).ToListAsync();
                }
                else if (user.Identity.IsAuthenticated)
                {
                    string userName = user.Identity.Name;
                    return await context.Orders
                        .Include(o => o.OrderItems)
                        .Where(o => o.UserName == userName)
                        .ToListAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving orders: {ex.Message}");
                return null;
            }
        }
        public async Task<Checkout> GetCheckoutDetails(int orderId)
        {
            try
            {
                // Находим заказ с указанным orderId
                var order = await context.Orders
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order != null)
                {
                    // Ищем объект Checkout по CheckoutId из заказа
                    var checkout = await context.Checkouts
                        .FirstOrDefaultAsync(c => c.Id == order.CheckoutId);

                    return checkout;
                }
                else
                {
                    // Если заказ не найден, возвращаем null
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving checkout details: {ex.Message}");
                throw;
            }
        }

    }
}