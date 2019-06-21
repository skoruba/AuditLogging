using Skoruba.AuditLogging.Events;
using Skoruba.AuditLogging.Host.Dtos;

namespace Skoruba.AuditLogging.Host.Events
{
    public class ProductGetEvent : AuditEvent
    {
        public ProductDto Product { get; set; }
    }
}
