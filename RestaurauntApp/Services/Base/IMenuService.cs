using RestaurauntApp.DTOS;

namespace RestaurauntApp.Services.Base
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuItem>> GetAllMenuItems();
        Task<int> CreateMenuItem(MenuItemDTO newMenuItem, IFormFile image);
        Task<int> DeleteMenuItem(int id);
        Task<int> UpdateMenuItem(int id, MenuItemDTO menuItemToUpdate);
        Task<MenuItem> GetMenuItem(int id);
    }
}