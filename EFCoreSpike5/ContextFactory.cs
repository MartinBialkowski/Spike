using EFCoreSpike5.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Spike.Core
{
    public class ContextFactory: IDesignTimeDbContextFactory<EFCoreSpikeContext>
    {
        public EFCoreSpikeContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EFCoreSpikeContext>();
            optionsBuilder.UseSqlServer(@"Server=spikemssql-database;Database=EFCoreSpikeCodeFirst;User Id=sa;Password=Password123!");

            return new EFCoreSpikeContext(optionsBuilder.Options);
        }
    }
}
