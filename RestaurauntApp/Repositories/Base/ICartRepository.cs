using RestaurauntApp.DTOS;
using RestaurauntApp.Models;

namespace RestaurauntApp.Repositories.Base
{
    public interface ICartRepository
    {
        Task<Cart> GetCartWithItems(string userName);
        Task<bool> AddToCart(CartItemDTO cartItem, string userName);
    }

}