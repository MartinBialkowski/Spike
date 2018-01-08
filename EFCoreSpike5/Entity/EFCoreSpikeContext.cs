using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Spike.Core.Entity;

namespace EFCoreSpike5.Models
{
    public class EFCoreSpikeContext : IdentityDbContext
    {
        public EFCoreSpikeContext(DbContextOptions<EFCoreSpikeContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Student>().ToTable("Student");
        }
    }
}
