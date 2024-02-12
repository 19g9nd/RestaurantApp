namespace RestaurauntApp.Core.DTOS
{
    public class CartItemDTO
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public int CartId { get; set; }
    }
}