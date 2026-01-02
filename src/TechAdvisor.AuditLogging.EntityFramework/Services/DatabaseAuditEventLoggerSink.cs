using System;
using System.Threading.Tasks;
using TechAdvisor.AuditLogging.EntityFramework.Entities;
using TechAdvisor.AuditLogging.EntityFramework.Mapping;
using TechAdvisor.AuditLogging.EntityFramework.Repositories;
using TechAdvisor.AuditLogging.Events;
using TechAdvisor.AuditLogging.Services;

namespace TechAdvisor.AuditLogging.EntityFramework.Services
{
    public class DatabaseAuditEventLoggerSink<TAuditLog> : IAuditEventLoggerSink 
        where TAuditLog : AuditLog, new()
    {
        private readonly IAuditLoggingRepository<TAuditLog> _auditLoggingRepository;

        public DatabaseAuditEventLoggerSink(IAuditLoggingRepository<TAuditLog> auditLoggingRepository)
        {
            _auditLoggingRepository = auditLoggingRepository;
        }

        public virtual async Task PersistAsync(AuditEvent auditEvent)
        {
            if (auditEvent == null) throw new ArgumentNullException(nameof(auditEvent));

            var auditLog = auditEvent.MapToEntity<TAuditLog>();

            await _auditLoggingRepository.SaveAsync(auditLog);
        }
    }
}