using Microsoft.AspNetCore.Mvc;
using System.Net;
using RestaurauntApp.DTOS;
using RestaurauntApp.Services.Base;

namespace RestaurantApp.Controllers
{
    [Route("menu")]
    public class MenuController : Controller
    {
        private readonly IMenuService menuService;
        public MenuController(IMenuService menuService)
        {
            this.menuService = menuService;
        }

        public async Task<IActionResult> GetAll()
        {
            var menuItems = await menuService.GetAllMenuItems();
            return View(menuItems);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm][Bind("Id,Name,Description,Category,IsVegetarian,Calories,Price")] MenuItemDTO newMenuItem, IFormFile Image)
        {
            try
            {
                var rowsAffected = await menuService.CreateMenuItem(newMenuItem, Image);

                if (rowsAffected > 0)
                {
                    return RedirectToAction("GetAll");
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to insert the product.");
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing the request.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rowsDeleted = await menuService.DeleteMenuItem(id);
            if (rowsDeleted == 0)
            {
                return NotFound("The product you are trying to delete does not exist.");
            }
            return RedirectToAction("GetAll");

        }
        [HttpGet]
        [Route("GetDetails")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var menuItem = await menuService.GetMenuItem(id);
            return View(menuItem);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateMenuItemAsync(int id, [FromBody] MenuItemDTO menuItemToUpdate)
        {
            var rowsAffected = await menuService.UpdateMenuItem(id, menuItemToUpdate);
            if (rowsAffected == 0)
            {
                return NotFound("The product you are trying to update does not exist.");
            }
            return Ok();
        }
        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var menuItem = await menuService.GetMenuItem(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            return View(menuItem);
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(int id,MenuItemDTO menuItem)
        {
            if (!ModelState.IsValid)
            {
                return View(menuItem);
            }
            var rowsAffected = await menuService.UpdateMenuItem(id, menuItem);
            if (rowsAffected == 0)
            {
                return NotFound();
            }

            return RedirectToAction("GetAll");
        }

    }
}
