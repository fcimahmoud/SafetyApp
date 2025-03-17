
global using Domain.Entities;
global using Shared.ProblemModels;

namespace Services.Abstractions
{
    public interface IProblemService
    {
        Task AddProblemAsync(string userId, CreateProblemDto dto);

        Task<ProblemDto> GetProblemAsync(string problemId);
        Task<IEnumerable<ProblemDto>> GetProblemsAsync();
        Task<IEnumerable<ProblemDto>> GetProblemsByTechnicianAsync(string technicianId);
        Task<IEnumerable<ProblemDto>> GetProblemsByClientAsync(string clientId);
        Task<IEnumerable<ProblemDto>> GetProblemsByStatusAsync(ProblemStatus status);
        Task AssignProblemToTechnicianAsync(string problemId, string technicianId);
        
        Task DeleteProblemAsync(string problemId);
        
        Task UpdateProblemAsync(string problemId, UpdateProblemDto dto);
        Task UpdateProblemStatusAsync(string problemId, ProblemStatus status);
        Task UpdateProblemImageAsync(string problemId, IFormFile image);
    }
}
