namespace RestaurauntApp.DTOS
{
    public class MenuItemDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public bool IsVegetarian { get; set; }
        public int? Calories { get; set; }
        public string? ImageURL { get; set; }
        public decimal? Price { get; set; }
    }
}