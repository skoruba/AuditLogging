using Microsoft.EntityFrameworkCore;
using TechAdvisor.AuditLogging.EntityFramework.Entities;

namespace TechAdvisor.AuditLogging.EntityFramework.DbContexts.Default
{
    public class DefaultAuditLoggingDbContext : AuditLoggingDbContext<AuditLog>
    {
        public DefaultAuditLoggingDbContext(DbContextOptions<DefaultAuditLoggingDbContext> dbContextOptions) 
            : base(dbContextOptions)
        {

        }
    }
}