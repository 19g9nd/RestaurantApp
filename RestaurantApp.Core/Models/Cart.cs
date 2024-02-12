namespace RestaurauntApp.Core.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserName { get; set; }
         public virtual ICollection<CartItem> CartItems { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }

        public Cart()
        {
            CartItems = new List<CartItem>();
            CreatedAt = DateTime.Now;
            TotalPrice = 0;
        }
    }
}