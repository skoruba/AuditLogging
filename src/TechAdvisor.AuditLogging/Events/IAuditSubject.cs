namespace TechAdvisor.AuditLogging.Events
{
    public interface IAuditSubject
    {
        string SubjectIdentifier { get; set; }

        string SubjectName { get; set; }

        string SubjectType { get; set; }

        object SubjectAdditionalData { get; set; }
    }
}