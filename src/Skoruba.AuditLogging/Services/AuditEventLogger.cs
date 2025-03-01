// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Original file: https://github.com/IdentityServer/IdentityServer4/src/Services/Default/DefaultEventService.cs
// Modified by Jan Škoruba

using Skoruba.AuditLogging.Configuration;
using Skoruba.AuditLogging.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skoruba.AuditLogging.Services
{
    public class AuditEventLogger(IEnumerable<IAuditEventLoggerSink> sinks, IAuditSubject auditSubject, IAuditAction auditAction, AuditLoggerOptions auditLoggerOptions) : IAuditEventLogger
    {
        protected readonly IEnumerable<IAuditEventLoggerSink> Sinks = sinks;
        protected readonly IAuditSubject AuditSubject = auditSubject;
        protected readonly IAuditAction AuditAction = auditAction;
        private readonly AuditLoggerOptions _auditLoggerOptions = auditLoggerOptions;

        /// <summary>
        /// Prepare default values for an event
        /// </summary>
        /// <param name="auditEvent"></param>
        /// <param name="loggerOptions"></param>
        /// <returns></returns>
        protected virtual Task PrepareEventAsync(AuditEvent auditEvent, Action<AuditLoggerOptions>? loggerOptions)
        {
            if (loggerOptions == default)
            {
                PrepareDefaultValues(auditEvent, _auditLoggerOptions);
            }
            else
            {
                var auditLoggerOptions = new AuditLoggerOptions();
                loggerOptions.Invoke(auditLoggerOptions);
                PrepareDefaultValues(auditEvent, auditLoggerOptions);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Prepare default values according to logger options
        /// </summary>
        /// <param name="auditEvent"></param>
        /// <param name="loggerOptions"></param>
        private void PrepareDefaultValues(AuditEvent auditEvent, AuditLoggerOptions loggerOptions)
        {
            if (loggerOptions.UseDefaultSubject)
            {
                PrepareDefaultSubject(auditEvent);
            }

            if (loggerOptions.UseDefaultAction)
            {
                PrepareDefaultAction(auditEvent);
            }

            PrepareDefaultConfiguration(auditEvent, loggerOptions);
        }

        /// <summary>
        /// Prepare default configuration
        /// </summary>
        /// <param name="auditEvent"></param>
        /// <param name="loggerOptions"></param>
        private void PrepareDefaultConfiguration(AuditEvent auditEvent, AuditLoggerOptions loggerOptions)
        {
            auditEvent.Source = loggerOptions.Source;
        }

        /// <summary>
        /// Prepare default action from IAuditAction
        /// </summary>
        /// <param name="auditEvent"></param>
        private void PrepareDefaultAction(AuditEvent auditEvent)
        {
            auditEvent.Action = AuditAction.Action;
        }

        /// <summary>
        /// Prepare default subject from IAuditSubject
        /// </summary>
        /// <param name="auditEvent"></param>
        private void PrepareDefaultSubject(AuditEvent auditEvent)
        {
            auditEvent.SubjectName = AuditSubject.SubjectName;
            auditEvent.SubjectIdentifier = AuditSubject.SubjectIdentifier;
            auditEvent.SubjectType = AuditSubject.SubjectType;
            auditEvent.SubjectAdditionalData = AuditSubject.SubjectAdditionalData;
        }

        /// <summary>
        /// Log an event
        /// </summary>
        /// <param name="auditEvent"></param>
        /// <param name="loggerOptions"></param>
        /// <returns></returns>
        public virtual async Task LogEventAsync(AuditEvent auditEvent, Action<AuditLoggerOptions>? loggerOptions = default)
        {
            if (!_auditLoggerOptions.Enabled)
            {
                return;
            }

            await PrepareEventAsync(auditEvent, loggerOptions);

            foreach (var sink in Sinks)
            {
                await sink.PersistAsync(auditEvent);
            }
        }
    }
}