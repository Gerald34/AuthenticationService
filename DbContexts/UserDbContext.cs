using Microsoft.EntityFrameworkCore;
using AuthenticationService.Entities;

namespace AuthenticationService.DbContexts
{
    public class UserDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public UserDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<UserEntity> Users { get; set; }
    }
}

