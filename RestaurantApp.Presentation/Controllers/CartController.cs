using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestaurauntApp.Core.DTOS;
using RestaurauntApp.Core.Repositories;

namespace RestaurauntApp.Controllers
{
    [Route("[controller]/[action]")]
    public class CartController : Controller
    {
        private readonly ICartRepository cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDTO cartItem)
        {
            try
            {
                var userName = User.Identity.Name; // получаем имя пользователя для связи с таблицей

                var result = await cartRepository.AddToCart(cartItem, userName);
                if (result)
                {
                    return RedirectToAction("Cart", "Cart"); // Перейти на страницу корзины
                                                             //   return RedirectToAction("GetAll", "Menu");
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
                var cart = await cartRepository.GetCartWithItems(userName);
                return View(cart);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }
    }
}