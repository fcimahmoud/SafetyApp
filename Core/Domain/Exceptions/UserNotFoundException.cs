
namespace Domain.Exceptions
{
    public class UserNotFoundException(string email)
        : NotFoundException($"no user with Email {email} was  not found!.")
    {
    }
}
