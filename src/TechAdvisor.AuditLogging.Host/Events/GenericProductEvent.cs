using TechAdvisor.AuditLogging.Events;
using TechAdvisor.AuditLogging.Host.Dtos;

namespace TechAdvisor.AuditLogging.Host.Events
{
    public class GenericProductEvent<T1, T2, T3> : AuditEvent
    {
        public ProductDto Product { get; set; }
    }
}
