using Assignment_2_3_CarlRizk.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment_2_3_CarlRizk.Contexts
{
    public class PoliciesDbContext : DbContext
    {
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }

        public PoliciesDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Policy>()
                .Property(p => p.PolicyNumber)
                .HasComputedColumnSql("LTRIM(STR(YEAR([EffectiveDate]))) + '-Medical-' + LTRIM(STR([Id]))");
        }
    }
}
