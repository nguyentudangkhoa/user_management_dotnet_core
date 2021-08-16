using System;
using test_dotnet_core_migration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace test_dotnet_core_migration.Helpers
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
        // connect to sql server database
          var serverVersion = new MySqlServerVersion(new Version(8, 0, 23));
          options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), serverVersion);
        }
        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }
    }
}
