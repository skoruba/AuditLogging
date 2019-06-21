using System.Threading.Tasks;
using Skoruba.AuditLogging.Events;

namespace Skoruba.AuditLogging.Services
{
    public interface IAuditLogger
    {
        /// <summary>
        /// Log an event
        /// </summary>
        /// <param name="auditEvent"></param>
        /// <param name="useDefaultAuditCaller"></param>
        /// <returns></returns>
        Task LogAsync(AuditEvent auditEvent, bool useDefaultAuditCaller = true);
    }
}