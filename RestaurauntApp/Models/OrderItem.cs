using System.ComponentModel.DataAnnotations;

namespace RestaurauntApp.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        [Required]
        public int MenuItemId { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}