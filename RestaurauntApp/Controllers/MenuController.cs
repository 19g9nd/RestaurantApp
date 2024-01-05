using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using RestaurauntApp.DTOS;
using RestaurauntApp.Repositories;

namespace RestaurantApp.Controllers
{
    [Route("menu")]
    public class MenuController : Controller
    {

        private readonly IMenuRepository menuRepository;
        public MenuController(IMenuRepository menuRepository)
        {
            this.menuRepository = menuRepository;
        }



        public async Task<IActionResult> GetAll()
        {
            try
            {
             
                    var menuItems = await menuRepository.GetAllMenuItemsAsync();

                    return View(menuItems);
                
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing the request.");
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetMenuItemByIdAsync(int id)
        {
            try
            {
              
                    var menuItem = await menuRepository.GetMenuItemByIdAsync(id);
                    if (menuItem == null)
                    {
                        return NotFound("The product you are trying to get does not exist.");
                    }

                    return Json(menuItem);
                
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing the request.");
            }
        }



        [HttpGet]
        [Route("[action]")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create([FromForm] MenuItemDTO newMenuItem)
        {
            try
            {
                var rowsAffected = await menuRepository.CreateMenuItemAsync(newMenuItem);

                if (rowsAffected > 0)
                {
                    // Вставка прошла успешно
                    return RedirectToAction("GetAll");
                }
                else
                {
                    // Вставка не удалась
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to insert the product.");
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing the request.");
            }
        }

       [HttpDelete("delete/{id}")]
public async Task<IActionResult> DeleteMenuItemAsync(int id)
{
    try
    {
        var rowsDeleted = await menuRepository.DeleteMenuItemAsync(id);

        if (rowsDeleted == 0)
        {
            return NotFound("The product you are trying to delete does not exist.");
        }

        return RedirectToAction("GetAll");
    }
    catch (Exception)
    {
        return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing the request.");
    }
}


        [HttpPut("update/{id}")]

        public async Task<IActionResult> UpdateMenuItemAsync(int id, [FromBody] MenuItemDTO menuItemToUpdate)
        {
            try
            {
                if (menuItemToUpdate == null || menuItemToUpdate.Price == null || string.IsNullOrEmpty(menuItemToUpdate.Name))
                {
                    return BadRequest("The product you are trying to update does not exist.");
                }

                
                    var rowsAffected = await menuRepository.UpdateMenuItemAsync(id,menuItemToUpdate);

                    if (rowsAffected == 0)
                    {
                        return NotFound("The product you are trying to update does not exist.");
                    }

                    return Ok();
                }
            
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
