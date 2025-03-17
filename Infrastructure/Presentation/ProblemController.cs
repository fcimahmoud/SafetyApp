
global using Domain.Entities;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Http;
global using Shared.ProblemModels;
global using System.Security.Claims;

namespace Presentation
{
    public class ProblemController(IServiceManager serviceManager)
        : ApiController
    {
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "ClientRole")]
        public async Task<IActionResult> AddProblem([FromForm] CreateProblemDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new
            {
                StatusCode = 401,
                ErrorMessage = "User not authenticated."
            });

            await serviceManager.ProblemService.AddProblemAsync(userId, dto);
            return Ok("Problem added successfully");
        }

        [HttpGet]
        [Authorize(Roles = "EngineerRole")]
        public async Task<IActionResult> GetAllProblems()
        {
            var problems = await serviceManager.ProblemService.GetProblemsAsync();
            return Ok(problems);
        }

        [HttpGet("{problemId}")]
        [Authorize]
        public async Task<IActionResult> GetProblemById(string problemId)
        {
            var problems = await serviceManager.ProblemService.GetProblemAsync(problemId);
            return Ok(problems);
        }

        [HttpGet("status/{status}")]
        [Authorize(Roles = "EngineerRole")]
        public async Task<IActionResult> GetProblemsByStatus(ProblemStatus status)
        {
            var problems = await serviceManager.ProblemService.GetProblemsByStatusAsync(status);
            return Ok(problems);
        }

        [HttpGet("technician")]
        [Authorize(Roles = "TechnicianRole")]
        public async Task<IActionResult> GetProblemsByTechnician()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new
            {
                StatusCode = 401,
                ErrorMessage = "User not authenticated."
            });

            var problems = await serviceManager.ProblemService.GetProblemsByTechnicianAsync(userId);
            return Ok(problems);
        }

        [HttpGet("client")]
        [Authorize(Roles = "ClientRole")]
        public async Task<IActionResult> GetProblemsByClient()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new
            {
                StatusCode = 401,
                ErrorMessage = "User not authenticated."
            });

            var problems = await serviceManager.ProblemService.GetProblemsByClientAsync(userId);
            return Ok(problems);
        }

        [HttpPut("{problemId}")]
        [Authorize(Roles = "ClientRole")]
        public async Task<IActionResult> UpdateProblem(string problemId, [FromBody] UpdateProblemDto dto)
        {
            await serviceManager.ProblemService.UpdateProblemAsync(problemId, dto);
            return Ok("Problem updated successfully");
        }

        [HttpPut("status/{problemId}")]
        [Authorize(Roles = "TechnicianRole,EngineerRole")]
        public async Task<IActionResult> UpdateProblemStatus(string problemId, ProblemStatus status)
        {
            await serviceManager.ProblemService.UpdateProblemStatusAsync(problemId, status);
            return Ok("Problem updated successfully");
        }

        [HttpPut("image/{problemId}/image")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "ClientRole")]
        public async Task<IActionResult> UpdateProblemImage(string problemId, [FromForm] IFormFile image)
        {
            await serviceManager.ProblemService.UpdateProblemImageAsync(problemId, image);
            return Ok("Problem image updated successfully");
        }

        [HttpDelete("{problemId}")]
        [Authorize(Roles = "ClientRole")]
        public async Task<IActionResult> DeleteProblem(string problemId)
        {
            await serviceManager.ProblemService.DeleteProblemAsync(problemId);
            return Ok("Problem deleted successfully");
        }


        [HttpPut("{problemId}/assign/{technicianId}")]
        [Authorize(Roles = "EngineerRole")]
        public async Task<IActionResult> AssignProblemToTechnician(string problemId, string technicianId)
        {
            await serviceManager.ProblemService.AssignProblemToTechnicianAsync(problemId, technicianId);
            return Ok("Problem assigned successfully");
        }

    }
}
