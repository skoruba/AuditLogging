namespace Skoruba.AuditLogging.Events
{
    public interface IAuditCaller
    {
        string SubjectIdentifier { get; set; }

        string SubjectName { get; set; }
    }
}