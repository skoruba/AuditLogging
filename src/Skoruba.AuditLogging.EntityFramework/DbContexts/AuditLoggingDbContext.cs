using Microsoft.EntityFrameworkCore;
using Skoruba.AuditLogging.EntityFramework.Entities;
using System.Threading.Tasks;

namespace Skoruba.AuditLogging.EntityFramework.DbContexts
{
    public abstract class AuditLoggingDbContext<TAuditLog>(DbContextOptions options) : DbContext(options), IAuditLoggingDbContext<TAuditLog>
        where TAuditLog : AuditLog

    {
        public DbSet<TAuditLog> AuditLog { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
