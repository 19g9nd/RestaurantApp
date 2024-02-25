using RestaurauntApp.Services;

namespace RestaurauntApp.Models.Other
{
    public class Checkout
    {

        public int Id { get; set; }
        public required string Username { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        [FutureDate(ErrorMessage = "The pickup time must be in the future.")]
        public DateTime PickupTime { get; set; }
        public required string CardNumber { get; set; }
        public required int CVV { get; set; }
        public required string Expiry { get; set; }
    }
}