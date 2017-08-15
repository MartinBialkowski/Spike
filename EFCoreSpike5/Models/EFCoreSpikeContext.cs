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
    }
}
