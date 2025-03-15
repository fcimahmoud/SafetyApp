
namespace Domain.Entities
{
    public class Engineer : BaseEntity<string>
    {
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
