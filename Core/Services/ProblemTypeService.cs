
using Shared.ProblemTypeModels;

namespace Services
{
    internal class ProblemTypeService (IUnitOfWork unitOfWork)
        : IProblemTypeService
    {

        public async Task<IEnumerable<ProblemTypeDto>> GetAllAsync()
        {
            var repo = unitOfWork.GetRepository<ProblemType, string>();
            var types = await repo.GetAllAsync();
            return types.Select(t => new ProblemTypeDto { Id = t.Id, Name = t.Name });
        }

        public async Task<ProblemTypeDto?> GetByIdAsync(string id)
        {
            var repo = unitOfWork.GetRepository<ProblemType, string>();
            var type = await repo.GetAsync(id);
            return type == null ? null : new ProblemTypeDto { Id = type.Id, Name = type.Name };
        }
        public async Task<ProblemTypeDto> CreateAsync(CreateProblemTypeDto dto)
        {
            var repo = unitOfWork.GetRepository<ProblemType, string>();
            var entity = new ProblemType { Id = Guid.NewGuid().ToString(), Name = dto.Name };
            await repo.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();
            return new ProblemTypeDto { Id = entity.Id, Name = entity.Name };
        }
        public async Task<bool> UpdateAsync(string id, CreateProblemTypeDto dto)
        {
            var repo = unitOfWork.GetRepository<ProblemType, string>();
            var entity = await repo.GetAsync(id);
            if (entity == null) return false;

            entity.Name = dto.Name;
            repo.Update(entity);
            await unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var repo = unitOfWork.GetRepository<ProblemType, string>();
            var entity = await repo.GetAsync(id);
            if (entity == null) return false;

            repo.Delete(entity);
            await unitOfWork.SaveChangesAsync();
            return true;
        }

    }
}
