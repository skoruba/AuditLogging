using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.AuditLogging.Events;
using Skoruba.AuditLogging.Helpers;

namespace Skoruba.AuditLogging.EntityFramework.Mapping
{
    public static class AuditMapping
    {
        public static AuditLog MapToEntity(this AuditEvent auditEvent)
        {
            var auditLog = new AuditLog
            {
                SubjectIdentifier = auditEvent.SubjectIdentifier,
                SubjectName = auditEvent.SubjectName,
                Category = auditEvent.Category,
                Data = LogSerializer.Serialize(auditEvent)
            };
            
            return auditLog;
        }
    }
}