using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LogProcess.Databases.EvoGameLog
{
    public class EvoGameLogDbContextFactory : IDesignTimeDbContextFactory<EvoGameLogDbContext>
    {
        public EvoGameLogDbContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("EVOGAMELOGDB_CONNECTIONSTRING");

            var optionsBuilder = new DbContextOptionsBuilder<EvoGameLogDbContext>();
            optionsBuilder.UseMySql(connectionString);

            return new EvoGameLogDbContext(optionsBuilder.Options);
        }
    }
}
