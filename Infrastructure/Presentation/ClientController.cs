
using Shared.UsersModels;

namespace Presentation
{
    public class ClientController (IServiceManager serviceManager)
        : ApiController
    {
        [HttpPut]
        [Authorize(Roles = "ClientRole")]
        public async Task<IActionResult> UpdateClient(UpdatedUserDto clientModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new
            {
                StatusCode = 401,
                ErrorMessage = "User not authenticated."
            });

            await serviceManager.UserService.UpdateUserAsync(userId, clientModel);
            return Ok("Client updated successfully.");
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "ClientRole,EngineerRole")]
        public async Task<IActionResult> DeleteClient(string userId)
        {
            await serviceManager.UserService.DeleteUserAsync(userId);
            return Ok("Client deleted successfully.");
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetClient(string userId)
        {
            var client = await serviceManager.UserService.GetUserProfileAsync(userId);
            return Ok(client);
        }

        [HttpGet("all")]
        [Authorize(Roles = "ClientRole,EngineerRole")]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await serviceManager.UserService.GetAllUsersByTypeAsync("Client");
            return Ok(clients);
        }
    }
}
