using Microsoft.Extensions.DependencyInjection;

namespace TechAdvisor.AuditLogging.Extensions
{
    public interface IAuditLoggingBuilder
    {
        IServiceCollection Services { get; }
    }
}