namespace RestaurauntApp.DTOS
{
    public class DiscountCodeDTO
    {
        public string Code { get; set; }
        public decimal Value { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

    }
}