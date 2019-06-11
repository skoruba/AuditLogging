using System.Threading.Tasks;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.AuditLogging.EntityFramework.Helpers.Common;

namespace Skoruba.AuditLogging.EntityFramework.Repositories
{
    public interface IAuditLoggingRepository
    {
        Task SaveAsync(AuditLog auditLog);

        Task<PagedList<AuditLog>> GetAsync(int page = 1, int pageSize = 10);

        Task<PagedList<AuditLog>> GetAsync(string subjectIdentifier, string subjectName, string category, int page = 1,
            int pageSize = 10);
    }
}