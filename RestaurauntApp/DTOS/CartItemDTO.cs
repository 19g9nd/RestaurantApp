namespace RestaurauntApp.DTOS
{
    public class CartItemDTO
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; internal set; }
        public int CartId { get; internal set; }
    }
}