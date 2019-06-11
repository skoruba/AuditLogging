using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skoruba.AuditLogging.EntityFramework.DbContexts;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.AuditLogging.EntityFramework.Helpers;
using Skoruba.AuditLogging.EntityFramework.Helpers.Common;

namespace Skoruba.AuditLogging.EntityFramework.Repositories
{
    public class AuditLoggingRepository : IAuditLoggingRepository
    {
        protected IAuditLoggingDbContext DbContext;

        public AuditLoggingRepository(IAuditLoggingDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<PagedList<AuditLog>> GetAsync(int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<AuditLog>();

            var auditLogs = await DbContext.AuditLog
                .PageBy(x => x.Id, page, pageSize)
                .ToListAsync();

            pagedList.Data.AddRange(auditLogs);
            pagedList.PageSize = pageSize;
            pagedList.TotalCount = await DbContext.AuditLog.CountAsync();


            return pagedList;
        }

        public async Task<PagedList<AuditLog>> GetAsync(string subjectIdentifier, string subjectName, string category, int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<AuditLog>();

            var auditLogs = await DbContext.AuditLog
                .WhereIf(!string.IsNullOrWhiteSpace(subjectIdentifier), x=> x.SubjectIdentifier == subjectIdentifier)
                .WhereIf(!string.IsNullOrWhiteSpace(subjectName), x => x.SubjectName == subjectName)
                .WhereIf(!string.IsNullOrWhiteSpace(category), x => x.Category == category)
                .PageBy(x => x.Id, page, pageSize)
                .ToListAsync();

            pagedList.Data.AddRange(auditLogs);
            pagedList.PageSize = pageSize;
            pagedList.TotalCount = await DbContext.AuditLog.CountAsync();


            return pagedList;
        }

        public async Task SaveAsync(AuditLog auditLog)
        {
            await DbContext.AuditLog.AddAsync(auditLog);
            await DbContext.SaveChangesAsync();
        }
    }
}