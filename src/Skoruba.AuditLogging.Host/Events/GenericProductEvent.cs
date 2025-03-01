using Skoruba.AuditLogging.Events;
using Skoruba.AuditLogging.Host.Dtos;

namespace Skoruba.AuditLogging.Host.Events
{
    public class GenericProductEvent<T1, T2, T3> : AuditEvent
    {
        public ProductDto Product { get; set; } = default!;
    }
}
