using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurauntApp.Services.Base;

namespace RestaurauntApp.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var user = HttpContext.User;
            var orders = await orderService.GetOrders(user);
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetCheckoutDetails(int orderId)
        {
            var checkout = await orderService.GetCheckoutDetails(orderId);
            return View(checkout);
        }

        [HttpPost]
        // [HttpGet("UpdateOrderStatus")]
        public async Task<IActionResult> UpdateOrderStatus([FromQuery] int orderId, [FromQuery] string action)
        {
         try
            {
                var result = await orderService.UpdateOrderStatus(orderId, action);
                if (result)
                {
                    return Ok(); // Возвращаем успешный результат
                }
                else
                {
                    return BadRequest(); // Возвращаем ошибку, если действие не выполнено
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

    }
}