using Microsoft.EntityFrameworkCore;
using AuthenticationService.Entities;

namespace AuthenticationService.DbContexts
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<UserEntity> Users { get; set; }
    }
}

