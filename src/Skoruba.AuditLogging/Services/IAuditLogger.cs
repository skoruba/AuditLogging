using System.Threading.Tasks;
using Skoruba.AuditLogging.Events;

namespace Skoruba.AuditLogging.Services
{
    public interface IAuditLogger
    {
        Task LogAsync(AuditEvent auditEvent);
    }
}