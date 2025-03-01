using Microsoft.EntityFrameworkCore;
using Skoruba.AuditLogging.EntityFramework.Entities;
using System.Threading.Tasks;

namespace Skoruba.AuditLogging.EntityFramework.DbContexts
{
    public interface IAuditLoggingDbContext<TAuditLog> where TAuditLog : AuditLog
    {
        DbSet<TAuditLog> AuditLog { get; set; }

        Task<int> SaveChangesAsync();
    }
}