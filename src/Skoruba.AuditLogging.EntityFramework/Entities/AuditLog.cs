using System;

namespace Skoruba.AuditLogging.EntityFramework.Entities
{
    public class AuditLog
    {
        /// <summary>
        /// Unique identifier for event
        /// </summary>
        public long Id { get; set; }

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
        /// Subject Identifier - who is responsible for current action
        /// </summary>
        public string SubjectIdentifier { get; set; } = default!;

        /// <summary>
        /// Subject Name - who is responsible for current action
        /// </summary>
        public string SubjectName { get; set; } = default!;

        /// <summary>
        /// Subject Type - User/Machine
        /// </summary>
        public string SubjectType { get; set; } = default!;

        /// <summary>
        /// Subject - some additional data
        /// </summary>
        public string? SubjectAdditionalData { get; set; }

        /// <summary>
        /// Information about request/action
        /// </summary>
        public string? Action { get; set; }

        /// <summary>
        /// Data which are serialized into JSON format
        /// </summary>
        public string Data { get; set; } = default!;

        /// <summary>
        /// Date and time for creating of the event
        /// </summary>
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
