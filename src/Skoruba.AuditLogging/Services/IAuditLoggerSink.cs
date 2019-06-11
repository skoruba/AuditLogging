using System.Threading.Tasks;
using Skoruba.AuditLogging.Events;

namespace Skoruba.AuditLogging.Services
{
    public interface IAuditLoggerSink
    {
        Task PersistAsync(AuditEvent auditEvent);
    }
}