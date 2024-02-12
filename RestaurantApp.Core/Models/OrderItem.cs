using System.ComponentModel.DataAnnotations;

namespace RestaurauntApp.Core.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int MenuItemId { get; set; }
        public virtual MenuItem MenuItem { get; set; } // Навигационное свойство 

        [Required]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}