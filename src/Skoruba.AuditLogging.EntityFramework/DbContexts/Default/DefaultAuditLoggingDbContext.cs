using Microsoft.EntityFrameworkCore;
using Skoruba.AuditLogging.EntityFramework.Entities;

namespace Skoruba.AuditLogging.EntityFramework.DbContexts.Default
{
    public class DefaultAuditLoggingDbContext(DbContextOptions<DefaultAuditLoggingDbContext> dbContextOptions) : AuditLoggingDbContext<AuditLog>(dbContextOptions)
    {
    }
}