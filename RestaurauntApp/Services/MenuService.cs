// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using RestaurauntApp.DTOS;
// using RestaurauntApp.Repositories;
// using RestaurauntApp.Services.Base;

// namespace RestaurauntApp.Services
// {
//     public class MenuService : IMenuService
//     {
//         private readonly IMenuRepository menuRepository;

//         public MenuService(IMenuRepository menuRepository)
//         {
//               this.menuRepository = menuRepository;
//         }
//         public Task CreateMenuItem(MenuItemDTO newMenuItem)
//         {
//             throw new NotImplementedException();
//         }

//         public async Task<int> DeleteMenuItem(int id)
//         {
//                 try
//             {
//                 var rowsDeleted = await menuRepository.DeleteMenuItemAsync(id);

//                 if (rowsDeleted == 0)
//                 {
//                     return NotFound("The product you are trying to delete does not exist.");
//                 }

//                 return RedirectToAction("GetAll");
//             }
//             catch (Exception)
//             {
//                 return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing the request.");
//             }
//         }

//         public async Task<IEnumerable<MenuItem>> GetAllMenuItems()
//         {
//                try
//             {
//                 var menuItems = await menuRepository.GetAllMenuItemsAsync();
//                 return menuItems;
//             }
//             catch (Exception)
//             {
//                 throw new InvalidOperationException("Can`t find menu items");
//             }
//         }

//         public Task<MenuItem> GetMenuItem(int id)
//         {
//             throw new NotImplementedException();
//         }

//         public Task<int> UpdateMenuItem(int id, MenuItemDTO menuItemToUpdate)
//         {
//             throw new NotImplementedException();
//         }
//     }
// }