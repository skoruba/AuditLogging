using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Skoruba.AuditLogging.Events;

namespace Skoruba.AuditLogging.Services
{
    public class AuditEventLogger : IAuditEventLogger
    {
        protected readonly IEnumerable<IAuditEventLoggerSink> Sinks;
        protected readonly IAuditSubject AuditSubject;
        protected readonly IAuditAction AuditAction;

        public AuditEventLogger(IEnumerable<IAuditEventLoggerSink> sinks, IAuditSubject auditSubject, IAuditAction auditAction)
        {
            Sinks = sinks;
            AuditSubject = auditSubject;
            AuditAction = auditAction;
        }

        protected virtual Task PrepareEventAsync(AuditEvent auditEvent, Action<AuditLoggerOptions> loggerOptions)
        {
            var auditLoggerOptions = new AuditLoggerOptions();
            loggerOptions?.Invoke(auditLoggerOptions);

            if (auditLoggerOptions.UseDefaultSubject)
            {
                PrepareDefaultSubject(auditEvent);
            }

            if (auditLoggerOptions.UseDefaultAction)
            {
                PrepareDefaultAction(auditEvent);
            }

            return Task.CompletedTask;
        }

        private void PrepareDefaultAction(AuditEvent auditEvent)
        {
            auditEvent.Action = AuditAction.Action;
        }

        private void PrepareDefaultSubject(AuditEvent auditEvent)
        {
            auditEvent.SubjectName = AuditSubject.SubjectName;
            auditEvent.SubjectIdentifier = AuditSubject.SubjectIdentifier;
            auditEvent.SubjectType = AuditSubject.SubjectType;
            auditEvent.SubjectAdditionalData = AuditSubject.SubjectAdditionalData;
        }

        public virtual async Task LogEventAsync(AuditEvent auditEvent, Action<AuditLoggerOptions> loggerOptions = default)
        {
            await PrepareEventAsync(auditEvent, loggerOptions);
            
            foreach (var sink in Sinks)
            {
                await sink.PersistAsync(auditEvent);
            }
        }
    }
}