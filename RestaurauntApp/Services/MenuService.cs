using RestaurauntApp.DTOS;
using RestaurauntApp.Repositories;
using RestaurauntApp.Services.Base;

namespace RestaurauntApp.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository menuRepository;

        public MenuService(IMenuRepository menuRepository)
        {
            this.menuRepository = menuRepository;
        }
        public async Task<int> CreateMenuItem(MenuItemDTO newMenuItem, IFormFile image)
        {
             if (newMenuItem.Price < 0 || newMenuItem.Price == null)
            {
                throw new ArgumentException("Price cannot be negative or empty.");
            }
            try
            {
                if (image != null && image.Length > 0)
                {
                    newMenuItem.ImageURL = await SaveImageAsync(image);
                }

                var rowsAffected = await menuRepository.CreateMenuItemAsync(newMenuItem);

                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create menu item", ex);
            }
        }

        private async Task<string> SaveImageAsync(IFormFile image)
        {
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            var imagePath = Path.Combine(uploadDirectory, uniqueFileName);

            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return "uploads/" + uniqueFileName;
        }


        public async Task<int> DeleteMenuItem(int id)
        {
            try
            {
                var rowsDeleted = await menuRepository.DeleteMenuItemAsync(id);
                return rowsDeleted;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to delete menu item", ex);
            }
        }

        public async Task<IEnumerable<MenuItem>> GetAllMenuItems()
        {
            try
            {
                var menuItems = await menuRepository.GetAllMenuItemsAsync();
                return menuItems;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to retrieve menu items", ex);
            }
        }

        public async Task<MenuItem> GetMenuItem(int id)
        {
            try
            {
                var menuItem = await menuRepository.GetMenuItemAsync(id);
                return menuItem;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to retrieve menu item", ex);
            }
        }

        public async Task<int> UpdateMenuItem(int id, MenuItemDTO menuItemToUpdate)
        {
            if (menuItemToUpdate == null || menuItemToUpdate.Price == null || string.IsNullOrEmpty(menuItemToUpdate.Name))
            {
                throw new ArgumentException("Invalid menu item data provided.");
            }

            try
            {
                var rowsAffected = await menuRepository.UpdateMenuItemAsync(id, menuItemToUpdate);
                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to update menu item", ex);
            }
        }

    }
}
