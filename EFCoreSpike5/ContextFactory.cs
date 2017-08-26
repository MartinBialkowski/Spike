using EFCoreSpike5.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EFCoreSpike5
{
    public class ContextFactory : IDesignTimeDbContextFactory<EFCoreSpikeContext>
    {
        public EFCoreSpikeContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EFCoreSpikeContext>();
            optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=EFCoreSpikeCodeFirst;Trusted_Connection=True;");

            return new EFCoreSpikeContext(optionsBuilder.Options);
        }
    }
}
