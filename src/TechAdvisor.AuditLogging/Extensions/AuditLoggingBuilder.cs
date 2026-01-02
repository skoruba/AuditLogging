using Microsoft.Extensions.DependencyInjection;
using TechAdvisor.AuditLogging.EntityFramework.Extensions;

namespace TechAdvisor.AuditLogging.Extensions
{
    public class AuditLoggingBuilder : IAuditLoggingBuilder
    {
        public AuditLoggingBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}