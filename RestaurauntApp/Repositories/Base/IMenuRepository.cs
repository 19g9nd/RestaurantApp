using RestaurauntApp.DTOS;

namespace RestaurauntApp.Repositories
{
    public interface IMenuRepository
    {
        Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync();
        Task<MenuItem> GetMenuItemByIdAsync(int id);
        Task<int> CreateMenuItemAsync(MenuItemDTO newMenuItem);
        Task<int> DeleteMenuItemAsync(int id);
        Task<int> UpdateMenuItemAsync(int id, MenuItemDTO menuItemToUpdate);
    }
}