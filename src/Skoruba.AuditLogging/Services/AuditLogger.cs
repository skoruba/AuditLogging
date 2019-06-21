using System.Collections.Generic;
using System.Threading.Tasks;
using Skoruba.AuditLogging.Events;

namespace Skoruba.AuditLogging.Services
{
    public class AuditLogger : IAuditLogger
    {
        private readonly IEnumerable<IAuditLoggerSink> _sinks;
        private readonly IAuditCaller _auditCaller;
        private bool UseDefaultAuditCaller { get; set; } = true;

        public AuditLogger(IEnumerable<IAuditLoggerSink> sinks, IAuditCaller auditCaller)
        {
            _sinks = sinks;
            _auditCaller = auditCaller;
        }

        protected virtual Task PrepareEventAsync(AuditEvent auditEvent)
        {
            if (UseDefaultAuditCaller)
            {
                auditEvent.SubjectIdentifier = _auditCaller.SubjectIdentifier;
                auditEvent.SubjectName = _auditCaller.SubjectName;
            }

            return Task.CompletedTask;
        }

        public virtual async Task LogAsync(AuditEvent auditEvent, bool useDefaultAuditCaller = true)
        {
            UseDefaultAuditCaller = useDefaultAuditCaller;

            await PrepareEventAsync(auditEvent);

            foreach (var sink in _sinks)
            {
                await sink.PersistAsync(auditEvent);
            }
        }
    }
}