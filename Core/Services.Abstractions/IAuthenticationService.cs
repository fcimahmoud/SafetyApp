
global using Shared.AuthenticationModels;

namespace Services.Abstractions
{
    public interface IAuthenticationService
    {
        public Task<UserResultDTO> LoginAsync(LoginDTO loginModel);
        public Task<UserResultDTO> RegisterAsync(RegisterDTO registerModel);
        public Task<bool> ConfirmEmailAsync(string email, string token);
        public Task<UserResultDTO> RefreshTokenAsync(string refreshToken);

        public Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto model);
        public Task<bool> ResetPasswordAsync(ResetPasswordRequestDto model);
    }
}
