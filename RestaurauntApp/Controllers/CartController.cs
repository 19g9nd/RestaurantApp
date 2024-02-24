using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurauntApp.DTOS;
using RestaurauntApp.Repositories.Base;

namespace RestaurauntApp.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IOrderRepository cartRepository;

        public CartController(IOrderRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            try
            {
                var userName = User.Identity.Name;
                var cart = await cartRepository.GetUncompleteOrderWithItems(userName);
                return View(cart);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddToOrder([FromBody] OrderItemDTO cartItem)
        {
            try
            {
                System.Console.WriteLine(cartItem.Name);
                var userName = User.Identity.Name; // получаем имя пользователя для связи с таблицей

                var result = await cartRepository.AddToOrder(cartItem, userName);
                if (result)
                {
                    return RedirectToAction("GetAll", "Menu");
                }
                else
                {
                    return BadRequest(new { message = "Failed to add item to cart." });
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutDTO cart)
        {
            try
            {
                // Передача данных о корзине и текущем пользователе в метод Checkout репозитория
                var result = await cartRepository.Checkout(cart, userName: User.Identity.Name);
                System.Console.WriteLine(result);
                if (result)
                {
                    // Если заказ успешно оформлен, перенаправляем пользователя на страницу со всем меню
                    return RedirectToAction("Success", "Cart");
                }
                else
                {
                    // Если произошла ошибка при оформлении заказа, возвращаем BadRequest
                    return BadRequest("Failed to checkout");
                }
            }
            catch (Exception ex)
            {
                // В случае исключения выводим сообщение об ошибке в консоль и возвращаем код 500
                Console.WriteLine(ex);
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        public IActionResult Success(){
            return View();
        }

    }
}