using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StudentQuizSystem.Data
{
    // This Factory is ONLY used by the 'dotnet ef' tooling (Add-Migration, Update-Database)
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // This is the SQL Server configuration for the MIGRATION TOOL
            builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=StudentQuizSystem_MigrationDb;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new ApplicationDbContext(builder.Options);
        }
    }
}