using RestaurauntApp.DTOS;
using RestaurauntApp.Models;

namespace RestaurauntApp.Repositories.Base
{
    public interface ICartRepository
    {
        Task<bool> AddToCart(CartItemDTO cartItem, string userName);
        // Task<bool> RemoveFromCart(int cartDetailsId);
        // Task<int> Clear(int id);
        // Task RemoveFromCart(int cartId, int menuItemId);
        
    }

}