using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestaurauntApp.DTOS;
using RestaurauntApp.Repositories.Base;

namespace RestaurauntApp.Controllers
{
    [Route("[controller]/[action]")]
    public class DiscountController : Controller
    {
        private readonly IDiscountRepository discountRepository;
        public DiscountController(IDiscountRepository discountRepository)
        {
            this.discountRepository = discountRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(DiscountCodeDTO discountCode)
        {
            if (ModelState.IsValid) // Проверяем, прошла ли модель валидацию
            {
                var result = await discountRepository.AddDiscountCode(discountCode);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError("", "Failed to add discount code."); // Добавляем ошибку модели
                }
            }
            return base.RedirectToAction("Index","Home"); // Возвращаем пользователя на ту же страницу с информацией об ошибках
        }

    }
}