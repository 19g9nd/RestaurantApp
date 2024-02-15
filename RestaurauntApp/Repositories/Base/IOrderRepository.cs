using RestaurauntApp.DTOS;
using RestaurauntApp.Models;

namespace RestaurauntApp.Repositories.Base
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderWithItems(string userName);
        Task<bool> AddToOrder(OrderItemDTO cartItem, string userName);
    }

}