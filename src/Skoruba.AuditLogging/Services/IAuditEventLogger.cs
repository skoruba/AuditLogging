using System;
using System.Threading.Tasks;
using Skoruba.AuditLogging.Events;

namespace Skoruba.AuditLogging.Services
{
    public interface IAuditEventLogger
    {
        /// <summary>
        /// Log an event
        /// </summary>
        /// <param name="auditEvent">The specific audit event</param>
        /// <param name="loggerOptions"></param>
        /// <returns></returns>
        Task LogEventAsync(AuditEvent auditEvent, Action<AuditLoggerOptions> loggerOptions = default);
    }
}