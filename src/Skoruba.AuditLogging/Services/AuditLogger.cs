using System.Collections.Generic;
using System.Threading.Tasks;
using Skoruba.AuditLogging.Events;

namespace Skoruba.AuditLogging.Services
{
    public class AuditLogger : IAuditLogger
    {
        private readonly IEnumerable<IAuditLoggerSink> _sinks;
        private readonly IAuditCaller _auditCaller;

        public AuditLogger(IEnumerable<IAuditLoggerSink> sinks, IAuditCaller auditCaller)
        {
            _sinks = sinks;
            _auditCaller = auditCaller;
        }

        protected virtual Task PrepareEventAsync(AuditEvent auditEvent)
        {
            auditEvent.SubjectIdentifier = _auditCaller.SubjectIdentifier;
            auditEvent.SubjectName = _auditCaller.SubjectName;

            return Task.CompletedTask;
        }

        public virtual async Task LogAsync(AuditEvent auditEvent)
        {
            await PrepareEventAsync(auditEvent);

            foreach (var sink in _sinks)
            {
                await sink.PersistAsync(auditEvent);
            }
        }
    }
}