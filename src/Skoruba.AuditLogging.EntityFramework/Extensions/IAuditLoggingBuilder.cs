using Microsoft.Extensions.DependencyInjection;

namespace Skoruba.AuditLogging.EntityFramework.Extensions
{
    public interface IAuditLoggingBuilder
    {
        IServiceCollection Services { get; }
    }
}