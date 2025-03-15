
using Services;
using Services.Abstractions;
using Shared.AuthenticationModels;

namespace Safety.API.Extensions
{
    public static class CoreServicesExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IServiceManager, ServiceManager>();

            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

            return services;
        }
    }
}
