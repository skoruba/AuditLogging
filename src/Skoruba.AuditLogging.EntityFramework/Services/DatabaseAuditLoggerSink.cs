using System;
using System.Threading.Tasks;
using Skoruba.AuditLogging.EntityFramework.Mapping;
using Skoruba.AuditLogging.EntityFramework.Repositories;
using Skoruba.AuditLogging.Events;
using Skoruba.AuditLogging.Services;

namespace Skoruba.AuditLogging.EntityFramework.Services
{
    public class DatabaseAuditLoggerSink : IAuditLoggerSink
    {
        private readonly IAuditLoggingRepository _auditLoggingRepository;

        public DatabaseAuditLoggerSink(IAuditLoggingRepository auditLoggingRepository)
        {
            _auditLoggingRepository = auditLoggingRepository;
        }

        public async Task PersistAsync(AuditEvent auditEvent)
        {
            if (auditEvent == null) throw new ArgumentNullException(nameof(auditEvent));

            var auditLog = auditEvent.MapToEntity();

            await _auditLoggingRepository.SaveAsync(auditLog);
        }
    }
}