namespace Skoruba.AuditLogging.Events.Default
{
    public class DefaultAuditSubject : IAuditSubject
    {
        public string SubjectIdentifier { get; set; } = default!;
        public string SubjectName { get; set; } = default!;
        public string SubjectType { get; set; } = default!;
        public object SubjectAdditionalData { get; set; } = default!;
    }
}