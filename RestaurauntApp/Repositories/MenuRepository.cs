using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using RestaurauntApp.DTOS;

namespace RestaurauntApp.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly SqlConnection connection;
        public MenuRepository(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }
        public async Task<int> CreateMenuItemAsync(MenuItemDTO newMenuItem)
        {
          var rowsAffected = await connection.ExecuteAsync(
                @"INSERT INTO Menu (Name, Description, Category, IsVegetarian, Calories, ImageURL, Price) 
                  VALUES (@Name, @Description, @Category, @IsVegetarian, @Calories, @ImageURL, @Price)",
                param: newMenuItem);

            return rowsAffected;
        }

        public async Task<int> DeleteMenuItemAsync(int id)
        {
             var rowsDeleted = await connection.ExecuteAsync(
                @"DELETE FROM Menu WHERE Id = @Id",
                param: new { Id = id });

            return rowsDeleted;
        }

        public async Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync()
        {
           var menuItems = await connection.QueryAsync<MenuItem>("SELECT * FROM Menu");
            return menuItems;
        }

        public async Task<MenuItem> GetMenuItemByIdAsync(int id)
        {
             var menuItem = await connection.QueryFirstOrDefaultAsync<MenuItem>(
                sql: "SELECT TOP 1 * FROM Menu WHERE Id = @Id",
                param: new { Id = id });

            return menuItem;
        }

        public async Task<int> UpdateMenuItemAsync(int id, MenuItemDTO menuItemToUpdate)
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

            return rowsAffected;
        }
    }
        
    
}