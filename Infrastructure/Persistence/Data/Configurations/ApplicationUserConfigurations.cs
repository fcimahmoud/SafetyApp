
namespace Persistence.Data.Configurations
{
    public class ApplicationUserConfigurations
        : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(true);

            builder.Property(u => u.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(true);

            builder.HasOne(A => A.Client)
                   .WithOne(I => I.ApplicationUser)
                   .HasForeignKey<Client>(I => I.ApplicationUserId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(A => A.Technician)
                   .WithOne(F => F.ApplicationUser)
                   .HasForeignKey<Technician>(F => F.ApplicationUserId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(A => A.Engineer)
                   .WithOne(E => E.ApplicationUser)
                   .HasForeignKey<Engineer>(E => E.ApplicationUserId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
