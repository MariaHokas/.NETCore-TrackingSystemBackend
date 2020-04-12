using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using timeTrackingSystemBackend.Entities;

namespace timeTrackingSystemBackend.Helpers
{
    public class DataContext : DbContext
    {
        protected readonly Microsoft.Extensions.Configuration.IConfiguration Configuration;
        private AutoMapper.Configuration.IConfiguration configuration;

        public DataContext(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public DataContext(AutoMapper.Configuration.IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server database
            options.UseSqlServer(Configuration.GetConnectionString("Server=DESKTOP-35CADGH\\SQLEMA;Database=WebApiDatabase;Trusted_Connection=True;"));
        }

        public DbSet<User> Users { get; set; }
    }
}
