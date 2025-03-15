
global using Domain.Entities;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Persistence.Data;
global using Domain.Contracts;
global using Persistence.Repositories;
global using Persistence;

namespace Safety.API.Extensions
{
    public static class InfraStructureServicesExtensions
    {
        public static IServiceCollection AddInfraStructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<SafetyDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            })
                .AddEntityFrameworkStores<SafetyDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

    }
}
