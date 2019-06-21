using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.AuditLogging.Events;
using Skoruba.AuditLogging.Helpers.JsonHelpers;

namespace Skoruba.AuditLogging.EntityFramework.Mapping
{
    public static class AuditMapping
    {
        public static TAuditLog MapToEntity<TAuditLog>(this AuditEvent auditEvent)
        where TAuditLog : AuditLog, new()
        {
            var auditLog = new TAuditLog
            {
                SubjectIdentifier = auditEvent.SubjectIdentifier,
                SubjectName = auditEvent.SubjectName,
                Category = auditEvent.Category,
                Data = AuditLogSerializer.Serialize(auditEvent)
            };
            
            return auditLog;
        }
    }
}