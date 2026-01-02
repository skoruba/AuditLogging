using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechAdvisor.AuditLogging.EntityFramework.Entities;

namespace TechAdvisor.AuditLogging.EntityFramework.DbContexts
{
    public interface IAuditLoggingDbContext<TAuditLog> where TAuditLog : AuditLog
    {
        DbSet<TAuditLog> AuditLog { get; set; }

        Task<int> SaveChangesAsync();
    }
}