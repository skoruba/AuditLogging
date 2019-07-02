namespace Skoruba.AuditLogging.Configuration
{
    public class AuditLoggerOptions
    {
        /// <summary>
        /// Use default subject from IAuditSubject
        /// </summary>
        public bool UseDefaultSubject { get; set; } = true;

        /// <summary>
        /// Use default action from IAuditAction
        /// </summary>
        public bool UseDefaultAction { get; set; } = true;
    }
}