namespace Skoruba.AuditLogging.Events
{
    public abstract class AuditEvent
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public string SubjectIdentifier { get; set; }

        public string SubjectName { get; set; }

        public string Data { get; set; }
    }
}