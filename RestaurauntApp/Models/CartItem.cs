namespace RestaurauntApp.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        // public virtual MenuItem MenuItem { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

       
    }
}