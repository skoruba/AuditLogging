using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skoruba.AuditLogging.EntityFramework.Entities;

namespace Skoruba.AuditLogging.EntityFramework.DbContexts
{
    public class AuditLoggingDbContext : DbContext, IAuditLoggingDbContext
    {
        public AuditLoggingDbContext(DbContextOptions<AuditLoggingDbContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }

        public DbSet<AuditLog> AuditLog { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
