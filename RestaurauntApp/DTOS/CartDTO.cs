namespace RestaurauntApp.DTOS
{
    public class CartDTO
    {
        public int UserId { get; set; }
        public List<CartItemDTO> Items { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }
    }
}