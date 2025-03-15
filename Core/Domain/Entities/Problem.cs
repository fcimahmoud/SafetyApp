
namespace Domain.Entities
{
    public class Problem : BaseEntity<string>
    {
        public required string PictureUrl { get; set; }
        public required string Description { get; set; }
        public ProblemStatus Status { get; set; } = ProblemStatus.Pending;

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
