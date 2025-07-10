using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshop.Infrastructure.Persistence
{
    public class CarWorkshopDbContext : IdentityDbContext
    {
        public CarWorkshopDbContext(DbContextOptions<CarWorkshopDbContext> options) : base(options)
        {
            
        }
        public DbSet<Domain.Entities.CarWorkshopEntity> CarWorkshops { get; set; }
        public DbSet<Domain.Entities.CarWorkshopService> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Domain.Entities.CarWorkshopEntity>()
                .OwnsOne(c => c.ContactDetails);

            modelBuilder.Entity<Domain.Entities.CarWorkshopEntity>()
                .HasMany(c => c.Services)
                .WithOne(s => s.CarWorkshop)
                .HasForeignKey(s => s.CarWorkshopId);
        }
    }
}