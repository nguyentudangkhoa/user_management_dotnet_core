using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using test_dotnet_core_migration.Helpers;

namespace test_dotnet_core_migration.Helpers
{
    public class MySqlContext : DataContext
    {
        public MySqlContext(IConfiguration configuration) : base(configuration) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }
    }
}