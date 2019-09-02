using Microsoft.Extensions.DependencyInjection;

namespace Skoruba.AuditLogging.Extensions
{
    public interface IAuditLoggingBuilder
    {
        IServiceCollection Services { get; }
    }
}