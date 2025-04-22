using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Skoruba.AuditLogging.Configuration;
using Skoruba.AuditLogging.Helpers.HttpContextHelpers;

namespace Skoruba.AuditLogging.Events.Http
{
    public class HttpAuditAction : IAuditAction
    {
        public HttpAuditAction(IHttpContextAccessor accessor, AuditHttpActionOptions options)
        {
            Action = new
            {
                accessor.HttpContext?.TraceIdentifier,
                RequestUrl = accessor.HttpContext?.Request.GetDisplayUrl(),
                HttpMethod = accessor.HttpContext?.Request.Method,
                FormVariables = options.IncludeFormVariables && accessor.HttpContext != null ? HttpContextHelpers.GetFormVariables(accessor.HttpContext) : null
            };
        }

        public object Action { get; set; }
    }
}