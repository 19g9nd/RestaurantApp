namespace RestaurauntApp.DTOS
{
    public class CheckoutDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public DateTime PickupTime { get; set; }
        public required string CardNumber { get; set; }
        public required string CVV { get; set; }
        public required string Expiry { get; set; }
    }
}