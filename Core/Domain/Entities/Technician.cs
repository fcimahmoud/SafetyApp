
namespace Domain.Entities
{
    public class Technician : BaseEntity<string>
    {
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
