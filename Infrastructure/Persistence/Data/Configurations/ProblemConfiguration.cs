
global using Domain.Entities;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class ProblemConfiguration : IEntityTypeConfiguration<Problem>
    {
        public void Configure(EntityTypeBuilder<Problem> builder)
        {

            builder.Property(t => t.Description)
                   .IsUnicode(true);

            builder.HasOne(t => t.Client)
                   .WithMany()
                   .HasForeignKey(t => t.ClientId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Technician)
                   .WithMany()
                   .HasForeignKey(t => t.TechnicianId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
