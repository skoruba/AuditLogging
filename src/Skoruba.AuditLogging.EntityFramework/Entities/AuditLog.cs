﻿using System;

namespace Skoruba.AuditLogging.EntityFramework.Entities
{
    public class AuditLog
    {
        /// <summary>
        /// Unique identifier for event
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Event category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Subject Identifier - who is responsible for current action
        /// </summary>
        public string SubjectIdentifier { get; set; }

        /// <summary>
        /// Subject Name - who is responsible for current action
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// Data which are serialized into JSON format
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Date and time for creating of the event
        /// </summary>
        public DateTime Created { get; set; } = DateTime.Now;
    }
}