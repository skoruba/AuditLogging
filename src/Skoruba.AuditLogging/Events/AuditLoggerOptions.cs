namespace Skoruba.AuditLogging.Events
{
    public class AuditLoggerOptions
    {
        public bool UseDefaultSubject { get; set; } = true;

        public bool UseDefaultAction { get; set; } = true;
    }
}