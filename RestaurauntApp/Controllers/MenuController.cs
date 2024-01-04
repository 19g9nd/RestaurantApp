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
        private const string ConnectionString = "Server=localhost;Database=RestaurantAppDb;Integrated Security=SSPI";
        private readonly SqlConnection connection = new SqlConnection(ConnectionString);

       public MenuController(IMenuRepository menuRepository)
    {
        this.menuRepository = menuRepository;
    }


       
        public async Task<IActionResult> GetAll()
        {
            try
            {
                using (connection)
                {
                       var menuItems = await connection.QueryAsync<MenuItem>("select * from Menu");
                      
                    return View(menuItems);
                }
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
                using (connection)
                {
                    var menuItem = await connection.QueryFirstOrDefaultAsync<MenuItem>(
                        sql: "select top 1 * from Menu where Id = @Id",
                        param: new { Id = id });

                    if (menuItem == null)
                    {
                        return NotFound("The product you are trying to get does not exist.");
                    }

                    return Json(menuItem);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing the request.");
            }
        }



        [HttpGet]
    [Route("[action]")]
    public IActionResult Create() {
        return View();
    }

        [HttpPost]

        public async Task<IActionResult> Create([FromForm] MenuItemDTO newMenuItem)
        {
            try
            {
                using (connection)
                {
                       var rowsAffected = await connection.ExecuteAsync(
                @"INSERT INTO Menu (Name, Description, Category, IsVegetarian, Calories, ImageURL, Price) 
                  VALUES (@Name, @Description, @Category, @IsVegetarian, @Calories, @ImageURL, @Price)",
                param: newMenuItem);

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
                using (connection)
                {
                    var rowsDeleted = await connection.ExecuteAsync(
                        @"DELETE FROM Menu WHERE Id = @Id",
                        param: new { Id = id });

                    if (rowsDeleted == 0)
                    {
                        return NotFound("The product you are trying to delete does not exist.");
                    }

                    return Ok();
                }
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

                using (connection)
                {
                    var rowsAffected = await connection.ExecuteAsync(
                        @"UPDATE Menu
                          SET Name = @Name, Description = @Description, Category = @Category, 
                              IsVegetarian = @IsVegetarian, Calories = @Calories, 
                              ImageURL = @ImageURL, Price = @Price
                          WHERE Id = @Id",
                        param: new
                        {
                            menuItemToUpdate.Name,
                            menuItemToUpdate.Description,
                            menuItemToUpdate.Category,
                            menuItemToUpdate.IsVegetarian,
                            menuItemToUpdate.Calories,
                            menuItemToUpdate.ImageURL,
                            menuItemToUpdate.Price,
                            Id = id
                        });

                    if (rowsAffected == 0)
                    {
                        return NotFound("The product you are trying to update does not exist.");
                    }

                    return Ok();
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
