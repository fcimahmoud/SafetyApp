
namespace Shared.ProblemModels
{
    public class UpdateProblemDto
    {
        public string Description { get; set; } = string.Empty;
        public ProblemType Type { get; set; }
    }
}
