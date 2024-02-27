using System.Security.Claims;
using System.Threading.Tasks;
using RestaurauntApp.DTOS;
using RestaurauntApp.Models;
using RestaurauntApp.Models.Other;

namespace RestaurauntApp.Services.Base
{
    public interface IOrderService
    {
        Task<Order> GetUncompleteOrder(string userName);
        Task<Order> GetOrder(string userName);
        Task<List<Order>> GetOrders(ClaimsPrincipal user);
        Task<bool> AddToOrder(OrderItemDTO orderItemDTO, string userName);
        Task<bool> CreateCheckout(CheckoutDTO checkoutModel, string userName);
        Task<Checkout> GetCheckoutDetails(int orderId);
        Task<bool> UpdateOrderStatus(int orderId,string action);
    }
}