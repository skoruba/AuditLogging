using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Skoruba.AuditLogging.Events;
using Skoruba.AuditLogging.Helpers.HttpContextHelpers;

namespace Skoruba.AuditLogging.Host.Logging
{
    public class AuditAction : IAuditAction
    {
        public AuditAction(IHttpContextAccessor accessor)
        {
            Action = new
            {
                TraceIdentifier = accessor.HttpContext.TraceIdentifier,
                RequestUrl = accessor.HttpContext.Request.GetDisplayUrl(),
                HttpMethod = accessor.HttpContext.Request.Method,
                FormVariables = HttpContextHelpers.GetFormVariables(accessor.HttpContext)
            };
        }

        public object Action { get; set; }
    }
}