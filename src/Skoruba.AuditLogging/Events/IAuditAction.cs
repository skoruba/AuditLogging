namespace Skoruba.AuditLogging.Events
{
    public interface IAuditAction
    {
        object Action { get; set; }
    }
}