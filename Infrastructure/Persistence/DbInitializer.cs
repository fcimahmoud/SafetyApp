
global using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class DbInitializer(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IUnitOfWork unitOfWork
        )
        : IDbInitializer
    {

        public async Task InitializeIdentityAsync()
        {
            var roles = new[]
            { "EngineerRole", "ClientRole", "TechnicianRole"};

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
                var admin1 = new ApplicationUser
                {
                    FirstName = "ابراهيم",
                    LastName = "والي",
                    UserType = "Engineer",
                    UserName = "e1",
                    Email = "ibrahimwaly@sfcegypt.com",
                    EmailConfirmed = true
                };
                var admin2 = new ApplicationUser
                {
                    FirstName = "رنا",
                    LastName = "حسين",
                    UserType = "Engineer",
                    UserName = "e2",
                    Email = "Rana.hussein@sfcegypt.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin1, "P@ssw0rdIbrahim");
                await userManager.AddToRoleAsync(admin1, "EngineerRole");

                await userManager.CreateAsync(admin2, "P@ssw0rdRana");
                await userManager.AddToRoleAsync(admin2, "EngineerRole");

                var eng1 = new Engineer
                {
                    Id = Guid.NewGuid().ToString(),
                    ApplicationUserId = admin1.Id
                };
                var eng2 = new Engineer
                {
                    Id = Guid.NewGuid().ToString(),
                    ApplicationUserId = admin2.Id
                };
                await unitOfWork.GetRepository<Engineer, string>().AddAsync(eng1);
                await unitOfWork.GetRepository<Engineer, string>().AddAsync(eng2);

                await unitOfWork.SaveChangesAsync();
            }
        }
    }
}
