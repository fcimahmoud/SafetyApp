
namespace Domain.Entities
{
    public class Client : BaseEntity<string>
    {
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
