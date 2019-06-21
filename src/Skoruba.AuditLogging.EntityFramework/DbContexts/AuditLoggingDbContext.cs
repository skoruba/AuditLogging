using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skoruba.AuditLogging.EntityFramework.Entities;

namespace Skoruba.AuditLogging.EntityFramework.DbContexts
{
    public class AuditLoggingDbContext<TAuditLog> : DbContext, IAuditLoggingDbContext<TAuditLog> 
        where TAuditLog : AuditLog

    {
        public AuditLoggingDbContext(DbContextOptions<AuditLoggingDbContext<TAuditLog>> dbContextOptions)
            : base(dbContextOptions)
        {

        }

        public DbSet<TAuditLog> AuditLog { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
