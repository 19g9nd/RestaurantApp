using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurauntApp.DTOS;
using RestaurauntApp.Repositories.Base;
using RestaurauntApp.Services.Base;
#pragma warning disable CS8602,CS8604

namespace RestaurauntApp.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IOrderService cartService; 

        public CartController(IOrderService cartService)
        {
            this.cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            try
            {
                var userName = User.Identity.Name;
                var cart = await cartService.GetUncompleteOrder(userName);
                return View(cart);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToOrder([FromBody] OrderItemDTO cartItem)
        {
            try
            {
                Console.WriteLine(cartItem.Name);
                var userName = User.Identity.Name; // получаем имя пользователя для связи с таблицей

                var result = await cartService.AddToOrder(cartItem, userName);
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
                Console.WriteLine(ex);
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutDTO cart)
        {
            if (!ModelState.IsValid)
            {
                // Если модель не проходит валидацию, вернуть текущее представление с ошибками валидации
                return View(cart);
            }

            try
            {
                // Передача данных о корзине и текущем пользователе в метод Checkout репозитория
                var result = await cartService.CreateCheckout(cart, userName: User.Identity.Name);
                Console.WriteLine(result);
                if (result)
                {
                    // Если заказ успешно оформлен, перенаправляем пользователя на страницу со всем меню
                    return RedirectToAction("Success", "Cart");
                }
                else
                {
                    // view с ошибкой
                    return View("Checkout");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        public IActionResult Success()
        {
            return View();
        }

    }
}