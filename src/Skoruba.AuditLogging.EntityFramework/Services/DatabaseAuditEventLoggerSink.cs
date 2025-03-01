using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.AuditLogging.EntityFramework.Mapping;
using Skoruba.AuditLogging.EntityFramework.Repositories;
using Skoruba.AuditLogging.Events;
using Skoruba.AuditLogging.Services;
using System;
using System.Threading.Tasks;

namespace Skoruba.AuditLogging.EntityFramework.Services
{
    public class DatabaseAuditEventLoggerSink<TAuditLog>(IAuditLoggingRepository<TAuditLog> auditLoggingRepository) : IAuditEventLoggerSink
        where TAuditLog : AuditLog, new()
    {
        private readonly IAuditLoggingRepository<TAuditLog> _auditLoggingRepository = auditLoggingRepository;

        public virtual async Task PersistAsync(AuditEvent auditEvent)
        {
            ArgumentNullException.ThrowIfNull(auditEvent);

            var auditLog = auditEvent.MapToEntity<TAuditLog>();

            await _auditLoggingRepository.SaveAsync(auditLog);
        }
    }
}