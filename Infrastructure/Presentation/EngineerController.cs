
using Shared.UsersModels;

namespace Presentation
{
    [Authorize(Roles = "EngineerRole")]
    public class EngineerController (IServiceManager serviceManager)
        : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> AddEngineer(EngineerDto engineerModel)
        {
            await serviceManager.UserService.AddEngineerAsync(engineerModel);
            return Ok("Engineer added successfully.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEngineer(UpdatedUserDto engineerModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new
            {
                StatusCode = 401,
                ErrorMessage = "User not authenticated."
            });

            await serviceManager.UserService.UpdateUserAsync(userId, engineerModel);
            return Ok("Engineer updated successfully.");
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteEngineer(string userId)
        {
            await serviceManager.UserService.DeleteUserAsync(userId);
            return Ok("Engineer deleted successfully.");
        }

        [HttpGet]
        public async Task<IActionResult> GetEngineer()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new
            {
                StatusCode = 401,
                ErrorMessage = "User not authenticated."
            });

            var engineer = await serviceManager.UserService.GetUserProfileAsync(userId);
            return Ok(engineer);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllEngineers()
        {
            var engineers = await serviceManager.UserService.GetAllUsersByTypeAsync("Engineer");
            return Ok(engineers);
        }

        [HttpGet("search")]
        [Authorize(Roles = "EngineerRole")]
        public async Task<IActionResult> SearchEngineer([FromQuery]string name)
        {
            var engineers = await serviceManager.UserService.SearchUsersByNameAsync(name, "Engineer");
            return Ok(engineers);
        }
    }
}
