using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.AuditLogging.EntityFramework.Helpers.Common;
using System.Threading.Tasks;

namespace Skoruba.AuditLogging.EntityFramework.Repositories
{
    public interface IAuditLoggingRepository<TAuditLog>
    where TAuditLog : AuditLog
    {
        Task SaveAsync(TAuditLog auditLog);

        Task<PagedList<TAuditLog>> GetAsync(int page = 1, int pageSize = 10);

        Task<PagedList<TAuditLog>> GetAsync(string subjectIdentifier, string subjectName, string category, int page = 1,
            int pageSize = 10);
    }
}