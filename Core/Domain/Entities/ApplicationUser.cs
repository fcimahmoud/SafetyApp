
global using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserType { get; set; }

        // Add these fields for OTP verification
        public string? EmailConfirmationOTP { get; set; }
        public DateTime? OTPExpiryTime { get; set; }

        // Add OTP fields for password reset
        public string? PasswordResetOTP { get; set; }
        public DateTime? PasswordResetOTPExpiry { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Firebase Token
        public string? FcmToken { get; set; }

        // Navigational Properties
        public Client? Client { get; set; }
        public Technician? Technician { get; set; }
        public Engineer? Engineer { get; set; }
    }
}
