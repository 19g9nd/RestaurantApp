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

        public async Task<IActionResult> GetOrders(){
            var user = HttpContext.User;
            var orders =  await orderRepository.GetOrdersWithItems(user);
            return View(orders);
        }

        public async Task<IActionResult> GetCheckoutDetails(int orderId){
            var checkout = await orderRepository.GetCheckoutDetails(orderId);
            return View(checkout);
        }

    }
}