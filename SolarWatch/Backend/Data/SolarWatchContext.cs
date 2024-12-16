using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SolarWatch.Models;

namespace SolarWatch.Data
{
    public class SolarWatchContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public SolarWatchContext(DbContextOptions<SolarWatchContext> options) : base(options) { }
        public DbSet<CityModel> Cities { get; set; }
        public DbSet<SunsetSunriseModel> SunriseSunsets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<CityModel>()
                .HasKey(c => c.CityId);

            modelBuilder.Entity<CityModel>()
                .HasMany(c => c.SunriseSunsets)
                .WithOne(s => s.CityModel)
                .HasForeignKey(s => s.CityId);
        }
    }
}