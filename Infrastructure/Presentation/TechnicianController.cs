
using Shared.UsersModels;

namespace Presentation
{
    public class TechnicianController(IServiceManager serviceManager)
        : ApiController
    {
        [HttpPost]
        [Authorize(Roles = "EngineerRole")]
        public async Task<IActionResult> AddTechnician(TechnicianDto techModel)
        {
            await serviceManager.UserService.AddTechnicianAsync(techModel);
            return Ok("Technician added successfully.");
        }

        [HttpPut]
        [Authorize(Roles = "TechnicianRole")]
        public async Task<IActionResult> UpdateTechnician(UpdatedUserDto techModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new
            {
                StatusCode = 401,
                ErrorMessage = "User not authenticated."
            });

            await serviceManager.UserService.UpdateUserAsync(userId, techModel);
            return Ok("Technician updated successfully.");
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "EngineerRole")]
        public async Task<IActionResult> DeleteTechnician(string userId)
        {
            await serviceManager.UserService.DeleteUserAsync(userId);
            return Ok("Technician deleted successfully.");
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "EngineerRole,TechnicianRole")]
        public async Task<IActionResult> GetTechnician(string userId)
        {
            var technician = await serviceManager.UserService.GetUserProfileAsync(userId);
            return Ok(technician);
        }

        [HttpGet("all")]
        [Authorize(Roles = "EngineerRole")]
        public async Task<IActionResult> GetAllTechnicians()
        {
            var technicians = await serviceManager.UserService.GetAllUsersByTypeAsync("Technician");
            return Ok(technicians);
        }

        [HttpGet("search")]
        [Authorize(Roles = "EngineerRole")]
        public async Task<IActionResult> SearchTechnician([FromQuery]string name)
        {
            var technicians = await serviceManager.UserService.SearchUsersByNameAsync(name, "Technician");
            return Ok(technicians);
        }
    }
}
