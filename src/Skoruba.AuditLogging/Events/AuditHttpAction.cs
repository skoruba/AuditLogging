using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Skoruba.AuditLogging.Helpers.HttpContextHelpers;

namespace Skoruba.AuditLogging.Events
{
    public class AuditHttpAction : IAuditAction
    {
        public AuditHttpAction(IHttpContextAccessor accessor)
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