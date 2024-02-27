using RestaurauntApp.Models.Other;

namespace RestaurauntApp.DTOS
{
    public class OrderDTO
    {
        public string UserName { get; set; }
        public ICollection<OrderItemDTO> OrderItems { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime OrderDate { get; set; }
        public EnumOrderState OrderState { get; set; }
        public decimal TotalPrice { get; set; }
    }
}