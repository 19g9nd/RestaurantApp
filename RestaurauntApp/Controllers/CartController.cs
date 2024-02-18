using System.Net;
using Microsoft.AspNetCore.Mvc;
using RestaurauntApp.DTOS;
using RestaurauntApp.Repositories.Base;

namespace RestaurauntApp.Controllers
{
    [Route("[controller]/[action]")]
    public class CartController : Controller
    {
        private readonly IOrderRepository cartRepository;

        public CartController(IOrderRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] OrderItemDTO cartItem)
        {
            try
            {
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
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutDTO cart) 
        {
          return base.Ok(cart);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            try
            {
                var userName = User.Identity.Name;
                var result = await cartRepository.RemoveFromOrder(itemId, userName);

                if (result)
                {
                     return RedirectToAction("GetAll","Menu");
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, "Item not found in the order.");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing the request.");
            }
        }

    }
}