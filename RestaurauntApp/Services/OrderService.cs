using System.Security.Claims;
using RestaurauntApp.DTOS;
using RestaurauntApp.Models;
using RestaurauntApp.Models.Other;
using RestaurauntApp.Repositories.Base;
using RestaurauntApp.Services.Base;

namespace RestaurauntApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<bool> AddToOrder(OrderItemDTO cartItem, string userName)
        {
            try
            {
                Console.WriteLine(cartItem.Name);
                // Получаем имя пользователя для связи с таблицей
                var result = await orderRepository.AddToOrder(cartItem, userName);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new ArgumentException();
            }
        }

        public async Task<bool> CreateCheckout(CheckoutDTO checkoutModel, string userName)
        {
            try
            {
                // Передача данных о корзине и текущем пользователе в метод Checkout репозитория
                var result = await orderRepository.Checkout(checkoutModel, userName);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<Checkout> GetCheckoutDetails(int orderId)
        {
            try
            {
                var checkout = await orderRepository.GetCheckoutDetails(orderId);
                return checkout;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<Order> GetOrder(string userName)
        {
            try
            {
               var order = await orderRepository.GetOrderWithItems(userName);
               return order;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<List<Order>> GetOrders(ClaimsPrincipal user)
        {
            try
            {
                var orders = await orderRepository.GetOrdersWithItems(user);
                return orders;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<Order> GetUncompleteOrder(string userName)
        {
            try
            {
                // Получаем незавершенный заказ для указанного пользователя
                var cart = await orderRepository.GetUncompleteOrderWithItems(userName);
                return cart;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw; // передача исключения выше для обработки на уровне контроллера
            }
        }

        public async Task<bool> UpdateOrderStatus(int orderId, string action)
        {
            try
            {
                bool result = false;
                if (action == "cancel")
                {
                    result = await orderRepository.CancelOrder(orderId);
                }
                else if (action == "next")
                {
                    result = await orderRepository.UpdateOrderStatus(orderId);
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
