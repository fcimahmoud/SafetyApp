
namespace Services
{
    public class ServiceManager(
        UserManager<ApplicationUser> userManager,
        IOptions<JwtOptions> options,
        IUnitOfWork unitOfWork,
        IOptions<EmailSettings> emailSettings,
        IEmailService emailService,
        IFileService fileService,
        IHttpContextAccessor httpContextAccessor
        ) : IServiceManager
    {
        private readonly Lazy<IAuthenticationService> _lazyAuthenticationService =
            new(() => new AuthenticationService(userManager, unitOfWork, options, emailService, httpContextAccessor));
        private readonly Lazy<IEmailService> _lazyEmailService =
            new(() => new EmailService(emailSettings));
        private readonly Lazy<IProblemService> _lazyProblemService =
            new(() => new ProblemService(unitOfWork, fileService, httpContextAccessor));
        private readonly Lazy<IUserService> _lazyUserService =
            new(() => new UserService(userManager, unitOfWork));

        public IAuthenticationService AuthenticationService => _lazyAuthenticationService.Value;
        public IEmailService EmailService => _lazyEmailService.Value;

        public IProblemService ProblemService => _lazyProblemService.Value;

        public IUserService UserService => _lazyUserService.Value;
    }
}
