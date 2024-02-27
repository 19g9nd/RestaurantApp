using RestaurauntApp.Models.Other;

namespace RestaurauntApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public EnumOrderState OrderState { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public int CheckoutId { get; set; }
        
        public Order()
        {
            OrderItems = new List<OrderItem>();
            CreatedAt = DateTime.Now;
            OrderDate = DateTime.Now;
            TotalPrice = 0;
            OrderState = EnumOrderState.waiting;
        }
    }
}