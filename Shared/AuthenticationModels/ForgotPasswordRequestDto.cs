namespace Shared.AuthenticationModels
{
    public class ForgotPasswordRequestDto
    {
        [Required, EmailAddress]
        public string Email { get; init; }
    }
}
