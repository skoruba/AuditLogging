using Microsoft.Extensions.DependencyInjection;
using Skoruba.AuditLogging.EntityFramework.Extensions;

namespace Skoruba.AuditLogging.Extensions
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