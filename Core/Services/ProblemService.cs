
global using Shared.ProblemModels;
using Domain.Entities;

namespace Services
{
    public class ProblemService (
        IUnitOfWork _unitOfWork,
        IFileService _fileService,
        IHttpContextAccessor _httpContextAccessor
        )
        : IProblemService
    {
        public async Task AddProblemAsync(string userId, CreateProblemDto dto)
        {
            var client = await GetClientByAppUserIdAsync(userId);

            var problemRepo = _unitOfWork.GetRepository<Problem, string>();
            string imagePath = await _fileService.SaveFileAsync(dto.Image, "uploads/problems");

            var problem = new Problem
            {
                Id = Guid.NewGuid().ToString(),
                ImagePath = imagePath,
                Description = dto.Description,
                Type = dto.Type,
                ClientId = client!.Id,
                Status = ProblemStatus.Pending
            };

            await problemRepo.AddAsync(problem);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ProblemDto> GetProblemAsync(string problemId)
        {
            var problemRepo = _unitOfWork.GetRepository<Problem, string>();
            var problem = await problemRepo.GetAsync(problemId);

            if (problem == null)
                throw new Exception("Problem not found");

            var user = await _unitOfWork.GetRepository<Client, string>()
                .GetWithIncludesAsync(c => c.Id == problem.ClientId, c => c.ApplicationUser!);
            var clientName = user?.ApplicationUser != null ?
                        $"{user.ApplicationUser.FirstName} {user.ApplicationUser.LastName}" : "Unknown";


            var tech = await _unitOfWork.GetRepository<Technician, string>()
                .GetWithIncludesAsync(t => t.Id == problem.TechnicianId, t => t.ApplicationUser!);
            var techName = tech?.ApplicationUser != null ?
                        $"{tech.ApplicationUser.FirstName} {tech.ApplicationUser.LastName}" : "Unassigned";

            return new ProblemDto
            {
                Id = problem.Id,
                ImageUrl = GetFullImageUrl(problem.ImagePath),
                Description = problem.Description,
                Status = problem.Status,
                ClientId = problem.ClientId,
                ClientName = clientName,
                TechnicianId = problem.TechnicianId,
                TechnicianName = techName,
                Type = problem.Type
            };
        }
        public async Task<IEnumerable<ProblemDto>> GetProblemsAsync()
        {
            var problemRepo = _unitOfWork.GetRepository<Problem, string>();
            var problems = await problemRepo.GetAllWithIncludesAsync
                (p => true, p => p.Client!.ApplicationUser!, p => p.Technician!.ApplicationUser!);

            return problems
                .Select(p => new ProblemDto
                {
                    Id = p.Id,
                    ImageUrl = GetFullImageUrl(p.ImagePath),
                    Description = p.Description,
                    Status = p.Status,
                    ClientId = p.ClientId,
                    ClientName = p.Client?.ApplicationUser != null ?
                        $"{p.Client.ApplicationUser.FirstName} {p.Client.ApplicationUser.LastName}" : "Unknown",
                    TechnicianId = p.TechnicianId,
                    TechnicianName = p.Technician?.ApplicationUser != null ?
                        $"{p.Technician.ApplicationUser.FirstName} {p.Technician.ApplicationUser.LastName}" : "Unassigned",
                    Type = p.Type
                });
        }
        public async Task<IEnumerable<ProblemDto>> GetProblemsByStatusAsync(ProblemStatus status)
        {
            var problemRepo = _unitOfWork.GetRepository<Problem, string>();
            var problems = await problemRepo.GetAllWithIncludesAsync
                (p => true, p => p.Client!.ApplicationUser!, p => p.Technician!.ApplicationUser!);

            return problems
                .Where(p => p.Status == status)
                .Select(p => new ProblemDto
                {
                    Id = p.Id,
                    ImageUrl = GetFullImageUrl(p.ImagePath),
                    Description = p.Description,
                    Status = p.Status,
                    ClientId = p.ClientId,
                    ClientName = p.Client?.ApplicationUser != null ?
                        $"{p.Client.ApplicationUser.FirstName} {p.Client.ApplicationUser.LastName}" : "Unknown",
                    TechnicianId = p.TechnicianId,
                    TechnicianName = p.Technician?.ApplicationUser != null ?
                        $"{p.Technician.ApplicationUser.FirstName} {p.Technician.ApplicationUser.LastName}" : "Unassigned",
                    Type = p.Type
                });
        }

        public async Task<IEnumerable<ProblemDto>> GetProblemsByTechnicianAsync(string technicianId)
        {
            var technician = await GetTechnicianByAppUserId(technicianId);
            if (technician == null)
                throw new Exception("Techician not found in tech table");

            var problemRepo = _unitOfWork.GetRepository<Problem, string>();
            var problems = await problemRepo.GetAllWithIncludesAsync
                (p => true, p => p.Client!.ApplicationUser!, p => p.Technician!.ApplicationUser!);

            return problems
                .Where(p => p.TechnicianId == technician.Id)
                .Select(p => new ProblemDto
                {
                    Id = p.Id,
                    ImageUrl = GetFullImageUrl(p.ImagePath),
                    Description = p.Description,
                    Status = p.Status,
                    ClientId = p.ClientId,
                    ClientName = p.Client?.ApplicationUser != null ?
                        $"{p.Client.ApplicationUser.FirstName} {p.Client.ApplicationUser.LastName}" : "Unknown",
                    TechnicianId = p.TechnicianId,
                    TechnicianName = p.Technician?.ApplicationUser != null ?
                        $"{p.Technician.ApplicationUser.FirstName} {p.Technician.ApplicationUser.LastName}" : "Unassigned",
                    Type = p.Type
                });
        }
        public async Task<IEnumerable<ProblemDto>> GetProblemsByClientAsync(string clientId)
        {
            var client = await GetClientByAppUserIdAsync(clientId);
            if (client == null)
                throw new Exception("Client not found in Client table");

            var problemRepo = _unitOfWork.GetRepository<Problem, string>();
            var problems = await problemRepo.GetAllWithIncludesAsync
                (p => true, p => p.Client!.ApplicationUser!, p => p.Technician!.ApplicationUser!);

            return problems
                .Where(p => p.ClientId == client.Id)
                .Select(p => new ProblemDto
                {
                    Id = p.Id,
                    ImageUrl = GetFullImageUrl(p.ImagePath),
                    Description = p.Description,
                    Status = p.Status,
                    ClientId = p.ClientId,
                    ClientName = p.Client?.ApplicationUser != null ?
                        $"{p.Client.ApplicationUser.FirstName} {p.Client.ApplicationUser.LastName}" : "Unknown",
                    TechnicianId = p.TechnicianId,
                    TechnicianName = p.Technician?.ApplicationUser != null ?
                        $"{p.Technician.ApplicationUser.FirstName} {p.Technician.ApplicationUser.LastName}" : "Unassigned",
                    Type = p.Type
                });
        }
        public async Task AssignProblemToTechnicianAsync(string problemId, string technicianId)
        {
            var problemRepo = _unitOfWork.GetRepository<Problem, string>();
            var problem = await problemRepo.GetAsync(problemId);

            if (problem == null)
                throw new Exception("Problem not found");

            var technician = await GetTechnicianByAppUserId(technicianId);
            if (technician == null)
                throw new Exception("Techician not found in tech table");
            
            problem.TechnicianId = technician.Id;
            problem.Status = ProblemStatus.InProgress;

            problemRepo.Update(problem);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task DeleteProblemAsync(string problemId)
        {
            var problemRepo = _unitOfWork.GetRepository<Problem, string>();
            var problem = await problemRepo.GetAsync(problemId);

            if (problem == null)
                throw new Exception("Problem not found");

            // Delete associated image if exists
            if (!string.IsNullOrEmpty(problem.ImagePath))
            {
                _fileService.DeleteFile(problem.ImagePath);
            }

            problemRepo.Delete(problem);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateProblemAsync(string problemId, UpdateProblemDto dto)
        {
            var problemRepo = _unitOfWork.GetRepository<Problem, string>();
            var problem = await problemRepo.GetAsync(problemId);

            if (problem == null)
                throw new Exception("Problem not found");

            problem.Description = dto.Description;
            problem.Type = dto.Type;

            problemRepo.Update(problem);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateProblemStatusAsync(string problemId, ProblemStatus status)
        {
            var problemRepo = _unitOfWork.GetRepository<Problem, string>();
            var problem = await problemRepo.GetAsync(problemId);

            if (problem == null)
                throw new Exception("Problem not found");

            problem.Status = status;

            problemRepo.Update(problem);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateProblemImageAsync(string problemId, IFormFile image)
        {
            var problemRepo = _unitOfWork.GetRepository<Problem, string>();
            var problem = await problemRepo.GetAsync(problemId);

            if (problem == null)
                throw new Exception("Problem not found");

            // Delete old image if exists
            if (!string.IsNullOrEmpty(problem.ImagePath))
            {
                _fileService.DeleteFile(problem.ImagePath);
            }

            string imagePath = await _fileService.SaveFileAsync(image, "uploads/problems");
            problem.ImagePath = imagePath;

            problemRepo.Update(problem);
            await _unitOfWork.SaveChangesAsync();
        }

        
        private string GetFullImageUrl(string imagePath)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null) return imagePath;

            var baseUrl = $"{request.Scheme}://{request.Host}";
            return $"{baseUrl}/{imagePath}";
        }
        public async Task<Client?> GetClientByAppUserIdAsync(string applicationUserId)
            => await _unitOfWork.GetRepository<Client, string>().GetByConditionAsync(user => user.ApplicationUserId == applicationUserId);
        public async Task<Technician?> GetTechnicianByAppUserId(string applicationUserId)
            => await _unitOfWork.GetRepository<Technician, string>().GetByConditionAsync(user => user.ApplicationUserId == applicationUserId);

    }
}
