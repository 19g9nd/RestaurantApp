using Moq;
using RestaurauntApp.DTOS;
using RestaurauntApp.Repositories.Base;
using RestaurauntApp.Services;

namespace XUNIT_RestaurantApp
{
    public class OrderServiceTests
    {
        [Theory]
        [InlineData("Test Item", 1, true)]
        [InlineData("Another Item1", 2, true)]
        [InlineData("Another Item2", 0, false)] 
        public async Task AddToOrder_WithDifferentQuantities_ReturnsCorrectResult(string itemName, int quantity, bool expectedResult)
        {
            var cartItem = new OrderItemDTO { Name = itemName, Quantity = quantity };
            var userName = "testuser";
            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock.Setup(repo => repo.AddToOrder(cartItem, userName)).ReturnsAsync(expectedResult);
            var orderService = new OrderService(orderRepositoryMock.Object);

            var result = await orderService.AddToOrder(cartItem, userName);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task AddToOrder_ThrowsException_ReturnsFalse()
        {
            var cartItem = new OrderItemDTO { Name = "Test Item", Quantity = 1 };
            var userName = "testuser";
            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock.Setup(repo => repo.AddToOrder(cartItem, userName)).ThrowsAsync(new Exception());
            var orderService = new OrderService(orderRepositoryMock.Object);

            await Assert.ThrowsAsync<ArgumentException>(() => orderService.AddToOrder(cartItem, userName));
        }
    }
}
