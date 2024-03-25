using RestaurauntApp.Models.Other;

namespace RestaurauntApp.Models
{
    public class DiscountCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal Value { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public virtual ICollection<DiscountUsage> Usages { get; set; } // Связь с использованиями кода скидки

        public DiscountCode()
        {
            Usages = new List<DiscountUsage>();
            ValidFrom = DateTime.Now;
            ValidTo = DateTime.Today.AddDays(1);
        }
    }

}