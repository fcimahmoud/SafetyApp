
global using Shared.UsersModels;

namespace Services.Abstractions
{
    public interface IUserService
    {
        Task AddTechnicianAsync(TechnicianDto technician);
        Task AddEngineerAsync(EngineerDto engineer);


        Task DeleteUserAsync(string appUserId);
        Task UpdateUserAsync(string appUserId, UpdatedUserDto updatedUser);


        Task<UserDto> GetUserProfileAsync(string appUserId);
        Task<IEnumerable<UserDto>> GetAllUsersByTypeAsync(string userType); // (Technician, Engineer, Client)
        Task<IEnumerable<UserDto>> SearchUsersByNameAsync(string name, string userType);
    }
}
