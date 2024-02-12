namespace RestaurauntApp.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal FinalTotal { get; set; }
        public EnumOrderState OrderState { get; set; }
        public virtual User Customer { get; set; } // Навигационное свойство к пользователю, который разместил заказ
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }

    public enum EnumOrderState
    {
        waiting = 0,
        unpaid = 1,
        completed = 2
    }
}