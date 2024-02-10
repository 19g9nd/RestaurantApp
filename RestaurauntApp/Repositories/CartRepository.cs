using Microsoft.EntityFrameworkCore;
using RestaurauntApp.Data;
using RestaurauntApp.DTOS;
using RestaurauntApp.Models;
using RestaurauntApp.Repositories.Base;

namespace RestaurauntApp.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly RestaurantAppDbContext context;

        public CartRepository(RestaurantAppDbContext context)
        {
            this.context = context;
        }

        public async Task<Cart> GetCartWithItems(string userName)
        {
            var cart = await context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserName == userName);
            return cart;
        }

        public async Task<bool> AddToCart(CartItemDTO cartItem, string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                {
                    throw new Exception("User name is null or empty.");
                }

                // Проверка существует ли уже корзина у пользователя
                var cart = await context.Carts
                   .Include(c => c.CartItems) 
                   .FirstOrDefaultAsync(c => c.UserName == userName);


                if (cart == null)
                {
                    // Если корзины нет создаем новую
                    cart = new Cart { UserName = userName };
                    context.Carts.Add(cart);
                }

                // Создаем новый элемент корзины
                var cartItemModel = new CartItem
                {
                    MenuItemId = cartItem.MenuItemId,
                    Quantity = cartItem.Quantity,
                    Name = cartItem.Name,
                    Price = cartItem.Price,
                    UserName = userName
                };

                // Добавляем элемент в корзину
                cart.CartItems.Add(cartItemModel);
                // Update total price of the cart
                cart.TotalPrice += (cartItem.Price * cartItem.Quantity);

                // Сохраняем изменения в базе данных
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Обработка исключения
                Console.WriteLine($"Error adding item to cart: {ex.Message}");
                return false;
            }
        }
    }
}