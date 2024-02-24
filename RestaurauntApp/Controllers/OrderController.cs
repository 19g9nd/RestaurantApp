using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurauntApp.Repositories.Base;

namespace RestaurauntApp.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var user = HttpContext.User;
            var orders = await orderRepository.GetOrdersWithItems(user);
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetCheckoutDetails(int orderId)
        {
            var checkout = await orderRepository.GetCheckoutDetails(orderId);
            return View(checkout);
        }

        [HttpPost]
        [HttpGet("UpdateOrderStatus")]
        public async Task<IActionResult> UpdateOrderStatus([FromQuery] int orderId, [FromQuery] string action)
        {
            bool result = false;

            if (action == "cancel")
            {
                result = await orderRepository.CancelOrder(orderId);
            }
            else if (action == "next")
            {
                result = await orderRepository.UpdateOrderStatus(orderId);
            }

            if (result)
            {
                return Ok(); // Возвращаем успешный результат
            }
            else
            {
                return BadRequest(); // Возвращаем ошибку, если действие не выполнено
            }
        }

    }
}