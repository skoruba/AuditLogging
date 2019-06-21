namespace Skoruba.AuditLogging.Events
{
    /// <summary>
    /// Default audit event for logging
    /// </summary>
    public abstract class AuditEvent
    {
        /// <summary>
        /// Unique identifier for the event
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Event category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Identifier of caller which is responsible for the event
        /// </summary>
        public string SubjectIdentifier { get; set; }

        /// <summary>
        /// Name of caller which is responsible for the event
        /// </summary>
        public string SubjectName { get; set; }
    }
}