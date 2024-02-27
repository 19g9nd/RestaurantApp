namespace XUNIT_RestaurantApp;

using Microsoft.AspNetCore.Http;
using Moq;
using RestaurauntApp.DTOS;
using RestaurauntApp.Repositories;
using RestaurauntApp.Services;

public class MenuServiceTests
{
    private MenuService menuService;
    private Mock<IMenuRepository> mockMenuRepository;
    public MenuServiceTests()
    {
        mockMenuRepository = new Mock<IMenuRepository>();
        menuService = new MenuService(mockMenuRepository.Object);
    }

    [Fact]
    public async Task CreateMenuItem_WithValidInput_ReturnsRowsAffected()
    {
        var menuItemDTO = new MenuItemDTO { Name = "Test Item", Price = 10.99m };
        var imageMock = new Mock<IFormFile>();
        imageMock.Setup(x => x.Length).Returns(10); 
        mockMenuRepository.Setup(repo => repo.CreateMenuItemAsync(menuItemDTO)).ReturnsAsync(1); 

        var result = await menuService.CreateMenuItem(menuItemDTO, imageMock.Object);

        Assert.Equal(1, result);
    }

    [Fact]
    public async Task CreateMenuItem_WithNullImage_ReturnsRowsAffected()
    {
        var menuItemDTO = new MenuItemDTO { Name = "Test Item", Price = 10.99m };
        mockMenuRepository.Setup(repo => repo.CreateMenuItemAsync(menuItemDTO)).ReturnsAsync(1);
        var result = await menuService.CreateMenuItem(menuItemDTO, null);

        Assert.Equal(1, result);
    }

    [Theory]
    [InlineData("Pizza", -100)]
    public async Task CreateMenuItem_WithInvalidPrice_ThrowsException(string name, decimal price)
    {
        var menuItemDTO = new MenuItemDTO { Name = name, Price = price };
        var imageMock = new Mock<IFormFile>();
        var menuRepositoryMock = new Mock<IMenuRepository>();
        var menuService = new MenuService(menuRepositoryMock.Object);

        await Assert.ThrowsAsync<ArgumentException>(() => menuService.CreateMenuItem(menuItemDTO, imageMock.Object));
    }

    [Fact]
    public async Task CreateMenuItem_RepositoryFails_ThrowsException()
    {
        var menuItemDTO = new MenuItemDTO { Name = "Test Item", Price = 10.99m };
        var imageMock = new Mock<IFormFile>();
        mockMenuRepository.Setup(repo => repo.CreateMenuItemAsync(menuItemDTO)).ThrowsAsync(new Exception("Repository failure")); 

        await Assert.ThrowsAsync<InvalidOperationException>(() => menuService.CreateMenuItem(menuItemDTO, imageMock.Object));
    }

}