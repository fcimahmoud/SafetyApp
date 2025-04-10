
global using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ProblemType : BaseEntity<string>
    {
        [Required]
        public string Name { get; set; } = string.Empty;

    }
}
