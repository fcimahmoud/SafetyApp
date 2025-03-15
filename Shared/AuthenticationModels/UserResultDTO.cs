namespace Shared.AuthenticationModels
{
    public record UserResultDTO(string FirstName, string LastName, string Email, string AccessToken,
    string RefreshToken);
}
