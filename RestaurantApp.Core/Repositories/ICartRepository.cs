using RestaurauntApp.Core.DTOS;
using RestaurauntApp.Core.Models;

namespace RestaurauntApp.Core.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetCartWithItems(string userName);
        Task<bool> AddToCart(CartItemDTO cartItem, string userName);
    }

}