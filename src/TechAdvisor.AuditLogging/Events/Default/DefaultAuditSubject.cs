namespace TechAdvisor.AuditLogging.Events.Default
{
    public class DefaultAuditSubject : IAuditSubject
    {
        public string SubjectIdentifier { get; set; }
        public string SubjectName { get; set; }
        public string SubjectType { get; set; }
        public object SubjectAdditionalData { get; set; }
    }
}