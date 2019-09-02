using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Skoruba.AuditLogging.Configuration;
using Skoruba.AuditLogging.Events;
using Skoruba.AuditLogging.Events.Default;
using Skoruba.AuditLogging.Events.Http;
using Skoruba.AuditLogging.Extensions;
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
        /// <param name="subjectOptions"></param>
        /// <param name="actionOptions"></param>
        /// <returns></returns>
        public static IAuditLoggingBuilder AddDefaultHttpEventData(this IAuditLoggingBuilder builder, Action<AuditHttpSubjectOptions> subjectOptions = default, Action<AuditHttpActionOptions> actionOptions = default)
        {
            var auditHttpSubjectOptions = new AuditHttpSubjectOptions();
            subjectOptions?.Invoke(auditHttpSubjectOptions);
            builder.Services.AddSingleton(auditHttpSubjectOptions);

            var auditHttpActionOptions = new AuditHttpActionOptions();
            actionOptions?.Invoke(auditHttpActionOptions);
            builder.Services.AddSingleton(auditHttpActionOptions);

            builder.Services.AddTransient<IAuditSubject, HttpAuditSubject>();
            builder.Services.AddTransient<IAuditAction, HttpAuditAction>();

            return builder;
        }

        /// <summary>
        /// Use IAuditSubject with pre-defined static data. This might be useful for service which is running as machine app.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="defaultAuditSubject"></param>
        /// <returns></returns>
        public static IAuditLoggingBuilder AddStaticEventSubject(this IAuditLoggingBuilder builder, Action<DefaultAuditSubject> defaultAuditSubject)
        {
            var auditSubject = new DefaultAuditSubject();
            defaultAuditSubject?.Invoke(auditSubject);
            builder.Services.AddSingleton(auditSubject);

            builder.Services.AddSingleton<IAuditSubject, DefaultAuditSubject>();

            return builder;
        }

        /// <summary>
        /// Use default implementation of IAuditSubject.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IAuditLoggingBuilder AddDefaultEventSubject(this IAuditLoggingBuilder builder)
        {
            builder.Services.AddTransient<IAuditSubject, DefaultAuditSubject>();

            return builder;
        }

        /// <summary>
        /// Use default implementation of IAuditAction.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IAuditLoggingBuilder AddDefaultEventAction(this IAuditLoggingBuilder builder)
        {
            builder.Services.AddTransient<IAuditAction, DefaultAuditAction>();

            return builder;
        }

        /// <summary>
        /// Use default implementation of IAuditSubject and IAuditAction.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IAuditLoggingBuilder AddDefaultEventData(this IAuditLoggingBuilder builder)
        {
            builder.Services.AddTransient<IAuditSubject, DefaultAuditSubject>();
            builder.Services.AddTransient<IAuditAction, DefaultAuditAction>();

            return builder;
        }

        /// <summary>
        /// Add own implementation for event data
        /// </summary>
        /// <typeparam name="TEventSubject"></typeparam>
        /// <typeparam name="TEventAction"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IAuditLoggingBuilder AddEventData<TEventSubject, TEventAction>(this IAuditLoggingBuilder builder)
        where TEventSubject : class, IAuditSubject
        where TEventAction : class, IAuditAction
        {
            builder.Services.AddTransient<IAuditSubject, TEventSubject>();
            builder.Services.AddTransient<IAuditAction, TEventAction>();

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