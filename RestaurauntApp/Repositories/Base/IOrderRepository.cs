using RestaurauntApp.DTOS;
using RestaurauntApp.Models;
using RestaurauntApp.Models.Other;

namespace RestaurauntApp.Repositories.Base
{
    public interface IOrderRepository
    {
        Task<Order> GetUncompleteOrderWithItems(string userName);
        Task<Order> GetOrderWithItems(string userName);
        Task<bool> AddToOrder(OrderItemDTO orderItemDTO, string userName);
        Task<bool> Checkout(CheckoutDTO checkoutModel, string userName);
    }

}