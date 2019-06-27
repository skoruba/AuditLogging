using System.Collections.Generic;
using System.Threading.Tasks;
using Skoruba.AuditLogging.Events;

namespace Skoruba.AuditLogging.Services
{
    public class AuditLogger : IAuditLogger
    {
        protected readonly IEnumerable<IAuditLoggerSink> Sinks;
        protected readonly IAuditSubject AuditSubject;
        protected readonly IAuditAction AuditAction;
        protected bool UseDefaultSubject = true;
        protected bool UseDefaultAction = true;

        public AuditLogger(IEnumerable<IAuditLoggerSink> sinks, IAuditSubject auditSubject, IAuditAction auditAction)
        {
            Sinks = sinks;
            AuditSubject = auditSubject;
            AuditAction = auditAction;
        }

        protected virtual Task PrepareEventAsync(AuditEvent auditEvent)
        {
            PrepareDefaultSubject(auditEvent);
            PrepareDefaultAction(auditEvent);

            return Task.CompletedTask;
        }

        private void PrepareDefaultAction(AuditEvent auditEvent)
        {
            if (!UseDefaultAction) return;

            auditEvent.Action = AuditAction.Action;
        }

        private void PrepareDefaultSubject(AuditEvent auditEvent)
        {
            if (!UseDefaultSubject) return;

            auditEvent.SubjectName = AuditSubject.SubjectName;
            auditEvent.SubjectIdentifier = AuditSubject.SubjectIdentifier;
            auditEvent.SubjectType = AuditSubject.SubjectType;
            auditEvent.SubjectAdditionalData = AuditSubject.SubjectAdditionalData;
        }

        public virtual async Task LogAsync(AuditEvent auditEvent, bool useDefaultSubject = true, bool useDefaultAction = true)
        {
            UseDefaultSubject = useDefaultSubject;
            UseDefaultAction = useDefaultAction;

            await PrepareEventAsync(auditEvent);

            foreach (var sink in Sinks)
            {
                await sink.PersistAsync(auditEvent);
            }
        }
    }
}