using Microsoft.Extensions.DependencyInjection;

namespace Skoruba.AuditLogging.Extensions
{
    public class AuditLoggingBuilder(IServiceCollection services) : IAuditLoggingBuilder
    {
        public IServiceCollection Services { get; } = services;
    }
}