namespace RestaurauntApp.DTOS
{
    public class OrderItemDTO
    {
        public int MenuItemId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}