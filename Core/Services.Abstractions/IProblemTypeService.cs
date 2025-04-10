using Shared.ProblemTypeModels;

namespace Services.Abstractions
{
    public interface IProblemTypeService
    {
        Task<IEnumerable<ProblemTypeDto>> GetAllAsync();
        Task<ProblemTypeDto?> GetByIdAsync(string id);
        Task<ProblemTypeDto> CreateAsync(CreateProblemTypeDto dto);
        Task<bool> UpdateAsync(string id, CreateProblemTypeDto dto);
        Task<bool> DeleteAsync(string id);
    }
}
