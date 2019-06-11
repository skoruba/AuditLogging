using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Skoruba.AuditLogging.EntityFramework.DbContexts;
using Skoruba.AuditLogging.EntityFramework.Repositories;
using Skoruba.AuditLogging.EntityFramework.Services;
using Skoruba.AuditLogging.Services;

namespace Skoruba.AuditLogging.EntityFramework.Extensions
{
    public static class AuditLoggingExtensions
    {
        public static IAuditLoggingBuilder AddAuditLoggingBuilder(this IServiceCollection services)
        {
            return new AuditLoggingBuilder(services);
        }

        /// <summary>
        /// Add audit logging middleware
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static IAuditLoggingBuilder AddAuditLogging(this IServiceCollection service)
        {
            var builder = service.AddAuditLoggingBuilder();

            builder.Services.AddTransient<IAuditLogger, AuditLogger>();
          
            return builder;
        }

        /// <summary>
        /// Add default DbContext and Repository
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IAuditLoggingBuilder AddDefaultStore(this IAuditLoggingBuilder builder)
        {
            builder.AddStore<AuditLoggingDbContext, AuditLoggingRepository>();

            return builder;
        }

        /// <summary>
        /// Add store with DbContext and Repository
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <typeparam name="TAuditLoggingRepository"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IAuditLoggingBuilder AddStore<TDbContext, TAuditLoggingRepository>(this IAuditLoggingBuilder builder)
            where TDbContext : DbContext, IAuditLoggingDbContext where TAuditLoggingRepository : class, IAuditLoggingRepository
        {
            builder.Services.AddDbContext<IAuditLoggingDbContext, TDbContext>();
            builder.Services.AddTransient<IAuditLoggingRepository, TAuditLoggingRepository>();

            return builder;
        }

        /// <summary>
        /// Add default database audit sink
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IAuditLoggingBuilder AddDefaultAuditSink(this IAuditLoggingBuilder builder)
        {
            builder.AddAuditSinks<DatabaseAuditLoggerSink>();

            return builder;
        }

        public static IAuditLoggingBuilder AddAuditSinks<T1>(this IAuditLoggingBuilder builder)
            where T1 : class, IAuditLoggerSink
        {
            builder.Services.TryAddEnumerable(new ServiceDescriptor[]
            {
                ServiceDescriptor.Transient<IAuditLoggerSink, T1>(),
            });

            return builder;
        }

        public static IAuditLoggingBuilder AddAuditSinks<T1, T2>(this IAuditLoggingBuilder builder)
            where T1 : class, IAuditLoggerSink
            where T2 : class, IAuditLoggerSink
        {
            builder.Services.TryAddEnumerable(new ServiceDescriptor[]
            {
                ServiceDescriptor.Transient<IAuditLoggerSink, T1>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T2>()
            });

            return builder;
        }

        public static IAuditLoggingBuilder AddAuditSinks<T1, T2, T3>(this IAuditLoggingBuilder builder)
            where T1 : class, IAuditLoggerSink
            where T2 : class, IAuditLoggerSink
            where T3 : class, IAuditLoggerSink
        {
            builder.Services.TryAddEnumerable(new ServiceDescriptor[]
            {
                ServiceDescriptor.Transient<IAuditLoggerSink, T1>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T2>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T3>()
            });

            return builder;
        }

        public static IAuditLoggingBuilder AddAuditSinks<T1, T2, T3, T4>(this IAuditLoggingBuilder builder)
            where T1 : class, IAuditLoggerSink
            where T2 : class, IAuditLoggerSink
            where T3 : class, IAuditLoggerSink
            where T4 : class, IAuditLoggerSink
        {
            builder.Services.TryAddEnumerable(new ServiceDescriptor[]
            {
                ServiceDescriptor.Transient<IAuditLoggerSink, T1>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T2>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T3>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T4>()
            });

            return builder;
        }

        public static IAuditLoggingBuilder AddAuditSinks<T1, T2, T3, T4, T5>(this IAuditLoggingBuilder builder)
            where T1 : class, IAuditLoggerSink
            where T2 : class, IAuditLoggerSink
            where T3 : class, IAuditLoggerSink
            where T4 : class, IAuditLoggerSink
            where T5 : class, IAuditLoggerSink
        {
            builder.Services.TryAddEnumerable(new ServiceDescriptor[]
            {
                ServiceDescriptor.Transient<IAuditLoggerSink, T1>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T2>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T3>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T4>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T5>()
            });

            return builder;
        }

        public static IAuditLoggingBuilder AddAuditSinks<T1, T2, T3, T4, T5, T6>(this IAuditLoggingBuilder builder)
            where T1 : class, IAuditLoggerSink
            where T2 : class, IAuditLoggerSink
            where T3 : class, IAuditLoggerSink
            where T4 : class, IAuditLoggerSink
            where T5 : class, IAuditLoggerSink
            where T6 : class, IAuditLoggerSink
        {
            builder.Services.TryAddEnumerable(new ServiceDescriptor[]
            {
                ServiceDescriptor.Transient<IAuditLoggerSink, T1>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T2>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T3>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T4>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T5>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T6>()
            });

            return builder;
        }

        public static IAuditLoggingBuilder AddAuditSinks<T1, T2, T3, T4, T5, T6, T7>(this IAuditLoggingBuilder builder)
            where T1 : class, IAuditLoggerSink
            where T2 : class, IAuditLoggerSink
            where T3 : class, IAuditLoggerSink
            where T4 : class, IAuditLoggerSink
            where T5 : class, IAuditLoggerSink
            where T6 : class, IAuditLoggerSink
            where T7 : class, IAuditLoggerSink
        {
            builder.Services.TryAddEnumerable(new ServiceDescriptor[]
            {
                ServiceDescriptor.Transient<IAuditLoggerSink, T1>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T2>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T3>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T4>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T5>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T6>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T7>()
            });

            return builder;
        }

        public static IAuditLoggingBuilder AddAuditSinks<T1, T2, T3, T4, T5, T6, T7, T8>(this IAuditLoggingBuilder builder)
            where T1 : class, IAuditLoggerSink
            where T2 : class, IAuditLoggerSink
            where T3 : class, IAuditLoggerSink
            where T4 : class, IAuditLoggerSink
            where T5 : class, IAuditLoggerSink
            where T6 : class, IAuditLoggerSink
            where T7 : class, IAuditLoggerSink
            where T8 : class, IAuditLoggerSink
        {
            builder.Services.TryAddEnumerable(new ServiceDescriptor[]
            {
                ServiceDescriptor.Transient<IAuditLoggerSink, T1>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T2>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T3>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T4>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T5>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T6>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T7>(),
                ServiceDescriptor.Transient<IAuditLoggerSink, T8>()
            });

            return builder;
        }
    }
}