using Microsoft.EntityFrameworkCore;
using AuthenticationService.Entities;

namespace AuthenticationService.DbContexts
{
    public class AccountStatusDbContext : DbContext
    {
        public AccountStatusDbContext(DbContextOptions<AccountStatusDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        DbSet<AccountStatusEntity> AccountStatuses { get; set; }
    }
}