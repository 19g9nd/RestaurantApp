using Microsoft.AspNetCore.Mvc;
using System.Net;
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
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing the request(menu).");
            }
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
                if (Image != null && Image.Length > 0)
                {
                    // Define the directory where images will be saved
                    var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    // Generate a unique filename for the uploaded image
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                    // Combine the directory and filename to get the full path
                    var imagePath = Path.Combine(uploadDirectory, uniqueFileName);
                    // Create the directory if it doesn't exist
                    if (!Directory.Exists(uploadDirectory))
                    {
                        Directory.CreateDirectory(uploadDirectory);
                    }
                    // Save the image to the server
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }
                    // Store the image path in DTO
                    newMenuItem.ImageURL = "uploads/" + uniqueFileName;
                }
                var rowsAffected = await menuRepository.CreateMenuItemAsync(newMenuItem);

                if (rowsAffected > 0)
                {
                    // Insertion was successful
                    return RedirectToAction("GetAll");
                }
                else
                {
                    // Insertion failed
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
        [HttpGet]
        [Route("GetDetails")]
        public async Task<IActionResult> GetDetails(int id)
        {
            try
            {
                var menuItem = await menuRepository.GetMenuItemAsync(id);
                return View(menuItem);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"An error occurred while processing the request, {ex}.");
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

                var rowsAffected = await menuRepository.UpdateMenuItemAsync(id, menuItemToUpdate);
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
