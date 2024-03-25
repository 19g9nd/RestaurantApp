namespace RestaurauntApp.Models.Other
{
    public class DiscountUsage
{
    public int Id { get; set; }
    public int DiscountCodeId { get; set; }
    public string UserName { get; set; }
    public DateTime UsageDate { get; set; } // Дата использования кода скидки
    public DiscountCode DiscountCode { get; set; }
}

}