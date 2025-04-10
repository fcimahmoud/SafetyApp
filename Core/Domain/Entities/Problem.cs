
namespace Domain.Entities
{
    public class Problem : BaseEntity<string>
    {
        public required string ImagePath { get; set; }
        public required string Description { get; set; }
        public ProblemStatus Status { get; set; } = ProblemStatus.Pending;

        public string? ProblemTypeId { get; set; }
        public ProblemType? ProblemType { get; set; }

        public string? ClientId { get; set; }
        public Client? Client { get; set; }

        public string? TechnicianId { get; set; }
        public Technician? Technician { get; set; }
    }
    public enum ProblemStatus
    {
        Pending, // معلقة
        InProgress, // قيد التنفيذ
        Completed // مكتملة
    }
}
