using BeFit.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>().HasData(
                new Microsoft.AspNetCore.Identity.IdentityRole
                {
                    Id = "1",
                    Name = "Adult",
                    NormalizedName = "ADULT"
                }
            );
        }

        public DbSet<Exercise> Exercise { get; set; }
        public DbSet<ExerciseType> ExerciseType { get; set; }
        public DbSet<Session> Session { get; set; }
    }
}