using TechAdvisor.AuditLogging.Events;
using TechAdvisor.AuditLogging.Host.Dtos;

namespace TechAdvisor.AuditLogging.Host.Events
{
    public class ProductGetEvent : AuditEvent
    {
        public ProductDto Product { get; set; }
    }
}
