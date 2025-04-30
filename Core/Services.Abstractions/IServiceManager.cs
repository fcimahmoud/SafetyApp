
namespace Services.Abstractions
{
    public interface IServiceManager
    {
        public IAuthenticationService AuthenticationService { get; }
        public IEmailService EmailService { get; }
        public IProblemService ProblemService { get; }
        public IUserService UserService { get; }
        public IProblemTypeService ProblemTypeService { get; }
        public INotificationService NotificationService { get; }
    }
}
