using Skoruba.AuditLogging.Helpers.Common;

namespace Skoruba.AuditLogging.Events
{
    /// <summary>
    /// Default audit event for logging
    /// </summary>
    public abstract class AuditEvent
    {
        protected AuditEvent()
        {
            Event = GetType().GetNameWithoutGenericParams();
        }

        /// <summary>
        /// Event name
        /// </summary>
        public string Event { get; set; } = default!;

        /// <summary>
        /// Source of logging events
        /// </summary>
        public string? Source { get; set; }

        /// <summary>
        /// Event category
        /// </summary>
        public string Category { get; set; } = default!;

        /// <summary>
        /// Identifier of caller which is responsible for the event
        /// </summary>
        public string SubjectIdentifier { get; set; } = default!;

        /// <summary>
        /// Name of caller which is responsible for the event
        /// </summary>
        public string SubjectName { get; set; } = default!;

        /// <summary>
        /// Subject Type - User/Machine
        /// </summary>
        public string SubjectType { get; set; } = default!;

        /// <summary>
        /// Subject - some additional data
        /// </summary>
        public object SubjectAdditionalData { get; set; } = default!;

        /// <summary>
        /// Information about request/action
        /// </summary>
        public object Action { get; set; } = default!;
    }
}