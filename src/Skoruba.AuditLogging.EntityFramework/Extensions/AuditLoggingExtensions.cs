using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Skoruba.AuditLogging.Configuration;
using Skoruba.AuditLogging.EntityFramework.DbContexts;
using Skoruba.AuditLogging.EntityFramework.DbContexts.Default;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.AuditLogging.EntityFramework.Repositories;
using Skoruba.AuditLogging.EntityFramework.Services;
using Skoruba.AuditLogging.Events;
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
        /// <param name="loggerOptions"></param>
        /// <returns></returns>
        public static IAuditLoggingBuilder AddAuditLogging<TAuditLoggerOptions>(this IServiceCollection service, Action<TAuditLoggerOptions> loggerOptions = default)
        where TAuditLoggerOptions : AuditLoggerOptions, new()
        {
            var builder = service.AddAuditLoggingBuilder();

            var auditLoggerOptions = new TAuditLoggerOptions();
            loggerOptions?.Invoke(auditLoggerOptions);

            builder.Services.AddSingleton(auditLoggerOptions);
            builder.Services.AddTransient<IAuditEventLogger, AuditEventLogger>();

            return builder;
        }

        /// <summary>
        /// Add audit logging middleware
        /// </summary>
        /// <param name="service"></param>
        /// <param name="loggerOptions"></param>
        /// <returns></returns>
        public static IAuditLoggingBuilder AddAuditLogging(this IServiceCollection service,
            Action<AuditLoggerOptions> loggerOptions = default)
        {
            return service.AddAuditLogging<AuditLoggerOptions>(loggerOptions);
        }

        /// <summary>
        /// Add audit default data - subject and action
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IAuditLoggingBuilder AddDefaultHttpEventData(this IAuditLoggingBuilder builder, Action<AuditHttpSubjectOptions> options = default)
        {
            var auditHttpSubjectOptions = new AuditHttpSubjectOptions();
            options?.Invoke(auditHttpSubjectOptions);

            builder.Services.AddSingleton(auditHttpSubjectOptions);

            builder.Services.AddTransient<IAuditSubject, AuditHttpSubject>();
            builder.Services.AddTransient<IAuditAction, AuditHttpAction>();

            return builder;
        }

        /// <summary>
        /// Add default DbContext and Repository
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="dbContextOptions"></param>
        /// <returns></returns>
        public static IAuditLoggingBuilder AddDefaultStore(this IAuditLoggingBuilder builder, Action<DbContextOptionsBuilder> dbContextOptions)
        {
            builder.AddStore<DefaultAuditLoggingDbContext, AuditLog, AuditLoggingRepository<DefaultAuditLoggingDbContext, AuditLog>>(dbContextOptions);

            return builder;
        }

        /// <summary>
        /// Add store with DbContext and Repository
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <typeparam name="TAuditLoggingRepository"></typeparam>
        /// <typeparam name="TAuditLog"></typeparam>
        /// <param name="builder"></param>
        /// <param name="dbContextOptions"></param>
        /// <returns></returns>
        public static IAuditLoggingBuilder AddStore<TDbContext, TAuditLog, TAuditLoggingRepository>(this IAuditLoggingBuilder builder, Action<DbContextOptionsBuilder> dbContextOptions)
            where TDbContext : DbContext, IAuditLoggingDbContext<TAuditLog> where TAuditLoggingRepository : class, IAuditLoggingRepository<TAuditLog> where TAuditLog : AuditLog
        {
            builder.Services.AddDbContext<TDbContext>(dbContextOptions);
            builder.Services.AddTransient<IAuditLoggingRepository<TAuditLog>, TAuditLoggingRepository>();

            return builder;
        }

        /// <summary>
        /// Add default database audit sink
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IAuditLoggingBuilder AddDefaultAuditSink(this IAuditLoggingBuilder builder)
        {
            builder.AddAuditSinks<DatabaseAuditEventLoggerSink<AuditLog>>();

            return builder;
        }

        public static IAuditLoggingBuilder AddAuditSinks<T1>(this IAuditLoggingBuilder builder)
            where T1 : class, IAuditEventLoggerSink
        {
            builder.Services.TryAddEnumerable(new ServiceDescriptor[]
            {
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T1>(),
            });

            return builder;
        }

        public static IAuditLoggingBuilder AddAuditSinks<T1, T2>(this IAuditLoggingBuilder builder)
            where T1 : class, IAuditEventLoggerSink
            where T2 : class, IAuditEventLoggerSink
        {
            builder.Services.TryAddEnumerable(new ServiceDescriptor[]
            {
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T1>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T2>()
            });

            return builder;
        }

        public static IAuditLoggingBuilder AddAuditSinks<T1, T2, T3>(this IAuditLoggingBuilder builder)
            where T1 : class, IAuditEventLoggerSink
            where T2 : class, IAuditEventLoggerSink
            where T3 : class, IAuditEventLoggerSink
        {
            builder.Services.TryAddEnumerable(new ServiceDescriptor[]
            {
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T1>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T2>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T3>()
            });

            return builder;
        }

        public static IAuditLoggingBuilder AddAuditSinks<T1, T2, T3, T4>(this IAuditLoggingBuilder builder)
            where T1 : class, IAuditEventLoggerSink
            where T2 : class, IAuditEventLoggerSink
            where T3 : class, IAuditEventLoggerSink
            where T4 : class, IAuditEventLoggerSink
        {
            builder.Services.TryAddEnumerable(new ServiceDescriptor[]
            {
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T1>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T2>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T3>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T4>()
            });

            return builder;
        }

        public static IAuditLoggingBuilder AddAuditSinks<T1, T2, T3, T4, T5>(this IAuditLoggingBuilder builder)
            where T1 : class, IAuditEventLoggerSink
            where T2 : class, IAuditEventLoggerSink
            where T3 : class, IAuditEventLoggerSink
            where T4 : class, IAuditEventLoggerSink
            where T5 : class, IAuditEventLoggerSink
        {
            builder.Services.TryAddEnumerable(new ServiceDescriptor[]
            {
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T1>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T2>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T3>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T4>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T5>()
            });

            return builder;
        }

        public static IAuditLoggingBuilder AddAuditSinks<T1, T2, T3, T4, T5, T6>(this IAuditLoggingBuilder builder)
            where T1 : class, IAuditEventLoggerSink
            where T2 : class, IAuditEventLoggerSink
            where T3 : class, IAuditEventLoggerSink
            where T4 : class, IAuditEventLoggerSink
            where T5 : class, IAuditEventLoggerSink
            where T6 : class, IAuditEventLoggerSink
        {
            builder.Services.TryAddEnumerable(new ServiceDescriptor[]
            {
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T1>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T2>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T3>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T4>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T5>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T6>()
            });

            return builder;
        }

        public static IAuditLoggingBuilder AddAuditSinks<T1, T2, T3, T4, T5, T6, T7>(this IAuditLoggingBuilder builder)
            where T1 : class, IAuditEventLoggerSink
            where T2 : class, IAuditEventLoggerSink
            where T3 : class, IAuditEventLoggerSink
            where T4 : class, IAuditEventLoggerSink
            where T5 : class, IAuditEventLoggerSink
            where T6 : class, IAuditEventLoggerSink
            where T7 : class, IAuditEventLoggerSink
        {
            builder.Services.TryAddEnumerable(new ServiceDescriptor[]
            {
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T1>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T2>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T3>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T4>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T5>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T6>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T7>()
            });

            return builder;
        }

        public static IAuditLoggingBuilder AddAuditSinks<T1, T2, T3, T4, T5, T6, T7, T8>(this IAuditLoggingBuilder builder)
            where T1 : class, IAuditEventLoggerSink
            where T2 : class, IAuditEventLoggerSink
            where T3 : class, IAuditEventLoggerSink
            where T4 : class, IAuditEventLoggerSink
            where T5 : class, IAuditEventLoggerSink
            where T6 : class, IAuditEventLoggerSink
            where T7 : class, IAuditEventLoggerSink
            where T8 : class, IAuditEventLoggerSink
        {
            builder.Services.TryAddEnumerable(new ServiceDescriptor[]
            {
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T1>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T2>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T3>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T4>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T5>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T6>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T7>(),
                ServiceDescriptor.Transient<IAuditEventLoggerSink, T8>()
            });

            return builder;
        }
    }
}