
global using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserType { get; set; }

        public string? EmailConfirmationOTP { get; set; }
        public DateTime? OTPExpiryTime { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Navigational Properties
        public Client? Client { get; set; }
        public Technician? Technician { get; set; }
        public Engineer? Engineer { get; set; }
    }
}
