
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence.Data
{
    public class SafetyDbContext 
        : IdentityDbContext<ApplicationUser>
    {
        public SafetyDbContext(DbContextOptions<SafetyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyRefrence).Assembly);
        }

        public DbSet<Problem> Problems { get; set; }
        public DbSet<Engineer> Engineers { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<Client> Clients { get; set; }


    }
}
