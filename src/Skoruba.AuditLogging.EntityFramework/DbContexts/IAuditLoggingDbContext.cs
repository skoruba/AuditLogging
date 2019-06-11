using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skoruba.AuditLogging.EntityFramework.Entities;

namespace Skoruba.AuditLogging.EntityFramework.DbContexts
{
    public interface IAuditLoggingDbContext
    {
        DbSet<AuditLog> AuditLog { get; set; }

        Task<int> SaveChangesAsync();
    }
}