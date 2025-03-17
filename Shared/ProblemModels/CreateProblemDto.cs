
global using Microsoft.AspNetCore.Http;

namespace Shared.ProblemModels
{
    public class CreateProblemDto
    {
        public IFormFile Image { get; set; } = default!;
        public string Description { get; set; } = string.Empty;
        public ProblemType Type { get; set; }
    }
}
