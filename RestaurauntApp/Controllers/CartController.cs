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
        private readonly IOrderRepository cartRepository;

        public CartController(IOrderService cartService, IOrderRepository cartRepository)
        {
            this.cartService = cartService;
            this.cartRepository = cartRepository;
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
        [HttpDelete]
        public async Task<IActionResult> RemoveFromOrder(int itemId)
        {
            try
            {
                var orderItem = await cartRepository.RemoveFromOrder(itemId, User.Identity.Name);
                if (orderItem != null)
                {
                    return Ok(); // Item successfully removed from the order
                }
                else
                {
                    return NotFound(); // Item not found in the order
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing item from order: {ex.Message}");
                return StatusCode(500); // Internal server error
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddToOrder([FromBody] List<OrderItemDTO> cartItems)
        {
            try
            {
                var userName = User.Identity.Name;
                foreach (var cartItem in cartItems)
                {
                    Console.WriteLine(cartItem.Name);
                    await cartService.AddToOrder(cartItem, userName);
                }
                return Ok();
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

        public async Task<IActionResult> ApplyDiscount(string discountCode)
        {
            try
            {
                var result = await cartRepository.ApplyDiscount(discountCode, this.User.Identity.Name);
                if (result)
                {
                    return base.Ok();
                }
                else
                {
                    ModelState.AddModelError("", "Failed to apply the discount.");
                    return BadRequest("Invalid discount code");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalPrice()
        {
            try
            {
                var totalPrice = await cartRepository.GetTotalPrice(User.Identity.Name);
                return Json(new { totalPrice });
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                // Handle exception
                return StatusCode(500, "Error fetching total price");
            }
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