
global using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class DbInitializer(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        : IDbInitializer
    {

        public async Task InitializeIdentityAsync()
        {
            var roles = new[]
            { "EngineerRole", "ClientRole", "TechnicianRole", "AdminRole"};

            // Seed Default Roles
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed Default Users
            if (!userManager.Users.Any())
            {
                var admin = new ApplicationUser
                {
                    FirstName = "Safety",
                    LastName = "Admin",
                    Email = "ma5740@fayoum.edu.eg",
                    UserType = "Admin"
                };

                await userManager.CreateAsync(admin, "P@ssw0rd");
                await userManager.AddToRoleAsync(admin, "AdminRole");
            }
        }
    }
}
