using System.Threading.Tasks;
using Skoruba.AuditLogging.Events;

namespace Skoruba.AuditLogging.Services
{
    public interface IAuditLogger
    {
        /// <summary>
        /// Log an event
        /// </summary>
        /// <param name="auditEvent">The specific audit event</param>
        /// <param name="useDefaultSubject">Get default subject from IAuditSubject</param>
        /// <param name="useDefaultAction">Get default subject from IAuditAction</param>
        /// <returns></returns>
        Task LogAsync(AuditEvent auditEvent, bool useDefaultSubject = true, bool useDefaultAction = true);
    }
}