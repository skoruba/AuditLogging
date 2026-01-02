namespace TechAdvisor.AuditLogging.Configuration
{
    public class AuditLoggerOptions
    {

        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Name of source
        /// </summary>
        public string Source { get; set; }

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