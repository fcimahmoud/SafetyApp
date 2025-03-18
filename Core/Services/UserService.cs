
global using Shared.UsersModels;

namespace Services
{
    internal class UserService (
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork
        )
        : IUserService
    {
        public async Task AddEngineerAsync(EngineerDto engineerModel)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserType = "Engineer",
                Email = engineerModel.Email,
                FirstName = engineerModel.FirstName,
                LastName = engineerModel.LastName,
                UserName = engineerModel.Email,
                PhoneNumber = engineerModel.PhoneNumber,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, engineerModel.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description).ToList();
                throw new ValidationException(errors);
            }

            // Add the user to the appropriate role
            var roleAssignmentResult = await userManager.AddToRoleAsync(user, "EngineerRole");
            if (!roleAssignmentResult.Succeeded)
            {
                var errors = roleAssignmentResult.Errors.Select(error => error.Description).ToList();
                throw new ValidationException(errors);
            }

            var individualRepo = unitOfWork.GetRepository<Engineer, string>();
            await individualRepo.AddAsync(new Engineer { Id = Guid.NewGuid().ToString(), ApplicationUserId = user.Id });
            await unitOfWork.SaveChangesAsync();
        }
        public async Task AddTechnicianAsync(TechnicianDto technicianModel)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserType = "Technician",
                Email = technicianModel.Email,
                FirstName = technicianModel.FirstName,
                LastName = technicianModel.LastName,
                UserName = technicianModel.Email,
                PhoneNumber = technicianModel.PhoneNumber,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, technicianModel.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description).ToList();
                throw new ValidationException(errors);
            }

            // Add the user to the appropriate role
            var roleAssignmentResult = await userManager.AddToRoleAsync(user, "TechnicianRole");
            if (!roleAssignmentResult.Succeeded)
            {
                var errors = roleAssignmentResult.Errors.Select(error => error.Description).ToList();
                throw new ValidationException(errors);
            }

            var individualRepo = unitOfWork.GetRepository<Technician, string>();
            await individualRepo.AddAsync(new Technician { Id = Guid.NewGuid().ToString(), ApplicationUserId = user.Id });
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(string appUserId)
        {
            var user = await userManager.FindByIdAsync(appUserId);
            if (user == null) throw new Exception("User not found");

            // Remove from ApplicationUser
            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded) throw new Exception("Failed to delete user");

            // Remove associated entity
            if (user.UserType == "Technician")
            {
                var technicianRepo = unitOfWork.GetRepository<Technician, string>();
                var technician = await technicianRepo.GetByConditionAsync(t => t.ApplicationUserId == user.Id);
                if (technician != null) technicianRepo.Delete(technician);
            }
            else if (user.UserType == "Engineer")
            {
                var engineerRepo = unitOfWork.GetRepository<Engineer, string>();
                var engineer = await engineerRepo.GetByConditionAsync(e => e.ApplicationUserId == user.Id);
                if (engineer != null) engineerRepo.Delete(engineer);
            }

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<UserDto> GetUserProfileAsync(string appUserId)
        {
            var appUser = await userManager.FindByIdAsync(appUserId);
            if (appUser == null) throw new Exception("User not found");
            return new UserDto { 
                Id = appUser.Id,
                Email = appUser.Email!, 
                FirstName = appUser.FirstName, 
                LastName = appUser.LastName, 
                PhoneNumber = appUser.PhoneNumber 
            };
        }
        public async Task<IEnumerable<UserDto>> GetAllUsersByTypeAsync(string userType)
        {
            return await userManager.Users.Where(u => u.UserType == userType).Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
            }).ToListAsync();
        }
        public async Task<IEnumerable<UserDto>> SearchUsersByNameAsync(string name, string userType)
        {
            return await userManager.Users
                .Where(u => u.UserType == userType && (u.FirstName.Contains(name) || u.LastName.Contains(name)))
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PhoneNumber = u.PhoneNumber,
                })
                .ToListAsync();
        }

        public async Task UpdateUserAsync(string appUserId, UpdatedUserDto updatedUser)
        {
            var user = await userManager.FindByIdAsync(appUserId);
            if (user == null) throw new Exception("User not found");

            // Update ApplicationUser Table
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.PhoneNumber = updatedUser.PhoneNumber;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded) throw new Exception("Failed to update profile");

        }
    }
}
