
namespace Domain.Exceptions
{
    public sealed class UnAuthorizedException(string message = "Invalid Email or Password")
        : Exception(message)
    {
    }
}
