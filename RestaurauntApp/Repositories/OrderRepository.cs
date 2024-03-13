using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using RestaurauntApp.Data;
using RestaurauntApp.DTOS;
using RestaurauntApp.Models;
using RestaurauntApp.Models.Other;
using RestaurauntApp.Repositories.Base;
using RestaurauntApp.Services.Classes;
#pragma warning disable CS8602

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
            try
            {
                var order = await context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.UserName == userName);
                if (order == null)
                {
                    throw new ArgumentNullException();
                }
                return order;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new InvalidOperationException();
            }
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
                    CVV = Convert.ToInt32(checkoutModel.CVV),
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
                order.OrderDate = DateTime.Now;
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
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    string userName = user.Identity.Name;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    if (userName == null)
                    {
                        throw new ArgumentNullException("userName is empty.");
                    }
                    return await context.Orders
                        .Include(o => o.OrderItems)
                        .Where(o => o.UserName == userName)
                        .ToListAsync();
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving orders: {ex.Message}");
                throw;
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
                    if (checkout == null)
                    {
                        throw new ArgumentNullException("checkout doesnt exist");
                    }
                    return checkout;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving checkout details: {ex.Message}");
                throw;
            }
        }
        public async Task<bool> UpdateOrderStatus(int orderId)
        {
            try
            {
                var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    return false; // Заказ не найден
                }

                // Получаем индекс текущего состояния заказа в EnumOrderState
                var currentIndex = (int)order.OrderState;

                // Проверяем, является ли текущее состояние последним в EnumOrderState
                if (currentIndex == (int)EnumOrderStateHelper.GetLastStatus())
                {
                    return false; // Достигнуто последнее состояние заказа
                }

                // Переходим к следующему состоянию
                order.OrderState = (EnumOrderState)(currentIndex + 1);

                await context.SaveChangesAsync();

                return true; // Состояние заказа успешно обновлено
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating order status: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> CancelOrder(int orderId)
        {
            try
            {
                var order = await context.Orders
                    .Include(o => o.OrderItems) // Включаем связанные элементы заказа
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order != null && order.CheckoutId != 0)
                {
                    var checkout = await context.Checkouts.FirstOrDefaultAsync(c => c.Id == order.CheckoutId);

                    if (checkout != null)
                    {
                        context.Checkouts.Remove(checkout);
                    }

                    // Удаляем все связанные элементы заказа
                    context.OrderItems.RemoveRange(order.OrderItems);

                    context.Orders.Remove(order);

                    await context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    // Заказ не найден или CheckoutId не указан
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cancelling order: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ApplyDiscount(string discountCode, string userName)
        {
            try
            {
                var order = await context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.UserName == userName && o.OrderState == EnumOrderState.waiting);
                if (order == null)
                {
                    return false;
                }
                Dictionary<string, decimal> discountCodes = new Dictionary<string, decimal>
        {
            { "OFF10", 0.1m }, // 10% discount
            { "SALE20", 0.2m } // 20% discount
        };
                if (discountCodes.ContainsKey(discountCode))
                {
                    decimal discountPercentage = discountCodes[discountCode];
                    decimal discountAmount = order.TotalPrice * discountPercentage;
                    order.TotalPrice -= discountAmount;

                    await context.SaveChangesAsync();
                    return true; 
                }
                else
                {
                    return false;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error applying discount: {ex.Message}");
                return false;
            }
        }

    }
}