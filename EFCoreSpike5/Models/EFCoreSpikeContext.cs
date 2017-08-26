using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSpike5.Models
{
    public class EFCoreSpikeContext : DbContext
    {
        public EFCoreSpikeContext(DbContextOptions<EFCoreSpikeContext> options): base(options)
        {
        }

        public DbSet<Claim> Claims { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Claim>().ToTable("Claim");
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaim");

            modelBuilder.Entity<UserClaim>()
                .HasKey(c => new { c.ClaimId, c.UserId });
        }
    }
}
