using Microsoft.EntityFrameworkCore;

namespace portfolio_api.Data
{
    public class MarekPuuDbContext : DbContext
    {

        public DbSet<Household> Households { get; set; }
        public DbSet<AuthServerUser> AuthServerUsers { get; set; }
        public DbSet<HouseholdUser> HouseholdUsers { get; set; }
        public DbSet<Role> Roles { get; set; }


        public MarekPuuDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Household>(entity =>
            {
                entity.HasKey(h => h.HouseholdId);
            });
            modelBuilder.Entity<AuthServerUser>(entity =>
            {
                entity.HasKey(a => a.AuthServerUserId);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.roleId);
            });

            modelBuilder.Entity<HouseholdUser>(entity =>
            {
                entity.HasKey(h => new { h.HouseholdId, h.AuthServerUserId });

                entity.HasOne(h => h.Household)
                    .WithMany(h => h.HouseholdUsers)
                    .HasForeignKey(h => h.HouseholdId);

                entity.HasOne(a => a.AuthServerUser)
                   .WithMany(h => h.HouseholdUsers)
                   .HasForeignKey(a => a.AuthServerUserId);

                entity.HasOne(h => h.Role)
                    .WithMany(r => r.HouseholdUsers)
                    .HasForeignKey(h => h.RoleId);
            });


        }
    }
}
