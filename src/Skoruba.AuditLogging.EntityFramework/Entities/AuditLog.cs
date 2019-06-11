using System;

namespace Skoruba.AuditLogging.EntityFramework.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public string SubjectIdentifier { get; set; }

        public string SubjectName { get; set; }

        public string Data { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
    }
}
