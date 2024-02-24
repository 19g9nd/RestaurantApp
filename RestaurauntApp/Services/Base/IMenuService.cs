using RestaurauntApp.DTOS;

namespace RestaurauntApp.Services.Base
{
    public interface IMenuService
    {
        Task GetAllMenuItems();
        Task CreateMenuItem(MenuItemDTO newMenuItem);
        Task<int> DeleteMenuItem(int id);
        Task<int> UpdateMenuItem(int id, MenuItemDTO menuItemToUpdate);
        Task<MenuItem> GetMenuItem(int id);
    }
}