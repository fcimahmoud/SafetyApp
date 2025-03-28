
namespace Shared.AuthenticationModels
{
    public class ResendOTPRequestDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
