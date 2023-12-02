using System.Data.SqlClient;
using System.Net;
using System.Text.Json;
using Dapper;

public class MenuController : ControllerBase
{
    private const string ConnectionString = "Server=localhost;Database=RestaurantAppDb;Integrated Security=SSPI";

    [HttpGet("GetAll")]
    // http://localhost:8080/menu/getall
    public async Task GetMenuItemsAsync(HttpListenerContext context)
    {
        using var writer = new StreamWriter(context.Response.OutputStream);

        using var connection = new SqlConnection(ConnectionString);
        var menuItems = await connection.QueryAsync<MenuItem>("select * from Menu");

        var menuItemsHtml = menuItems.GetHtml();
        await writer.WriteLineAsync(menuItemsHtml);
        context.Response.ContentType = "text/html";

        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpGet("GetById")]
    //http://localhost:8080/menu/GetById?id=2
    public async Task GetMenuItemByIdAsync(HttpListenerContext context)
    {
        var menuItemIdToGetObj = context.Request.QueryString["id"];

        if (menuItemIdToGetObj == null || int.TryParse(menuItemIdToGetObj, out int menuItemIdToGet) == false)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var menuItem = await connection.QueryFirstOrDefaultAsync<MenuItem>(
            sql: "select top 1 * from Menu where Id = @Id",
            param: new { Id = menuItemIdToGet });

        if (menuItem is null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        using var writer = new StreamWriter(context.Response.OutputStream);
        await writer.WriteLineAsync(JsonSerializer.Serialize(menuItem));

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpPost("Create")]
    //http://localhost:8080/menu/create
    public async Task CreateMenuItemAsync(HttpListenerContext context)
    {
        using var reader = new StreamReader(context.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        var newMenuItem = JsonSerializer.Deserialize<MenuItem>(json);

        if (newMenuItem == null || newMenuItem.Price == null || string.IsNullOrWhiteSpace(newMenuItem.Name))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var rowsAffected = await connection.ExecuteAsync(
            @"INSERT INTO Menu (Name, Description, Category, IsVegetarian, Calories, ImageURL, Price) 
              VALUES (@Name, @Description, @Category, @IsVegetarian, @Calories, @ImageURL, @Price)",
            param: newMenuItem);

        context.Response.StatusCode = (int)HttpStatusCode.Created;
    }

    [HttpDelete]
    //http://localhost:8080/menu/delete?id=1
    public async Task DeleteMenuItemAsync(HttpListenerContext context)
    {
        var menuItemIdToDeleteObj = context.Request.QueryString["id"];

        if (menuItemIdToDeleteObj == null || int.TryParse(menuItemIdToDeleteObj, out int menuItemIdToDelete) == false)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
        var rowsDeleted = await connection.ExecuteAsync(
            @"DELETE FROM Menu
              WHERE Id = @Id",
            param: new { Id = menuItemIdToDelete });

        if (rowsDeleted == 0)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }

    [HttpPut]
    //http://localhost:8080/menu/update?id=1
    public async Task UpdateMenuItemAsync(HttpListenerContext context)
    {
        var menuItemIdToUpdateObj = context.Request.QueryString["id"];

        if (menuItemIdToUpdateObj == null || int.TryParse(menuItemIdToUpdateObj, out int menuItemIdToUpdate) == false)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var reader = new StreamReader(context.Request.InputStream);
        var json = await reader.ReadToEndAsync();

        var menuItemToUpdate = JsonSerializer.Deserialize<MenuItem>(json);

        if (menuItemToUpdate == null || menuItemToUpdate.Price == null || string.IsNullOrEmpty(menuItemToUpdate.Name))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        using var connection = new SqlConnection(ConnectionString);
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
                Id = menuItemIdToUpdate
            });

        if (rowsAffected == 0)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return;
        }

        context.Response.StatusCode = (int)HttpStatusCode.OK;
    }
}
