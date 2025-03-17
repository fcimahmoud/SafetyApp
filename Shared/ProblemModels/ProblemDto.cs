
global using Domain.Entities;

namespace Shared.ProblemModels
{
    public class ProblemDto
    {
        public string Id { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty; // Will return full URL
        public string Description { get; set; } = string.Empty;
        public ProblemStatus Status { get; set; }
        public string? ClientId { get; set; }
        public string? TechnicianId { get; set; }
        public ProblemType Type { get; set; }
    }
}
