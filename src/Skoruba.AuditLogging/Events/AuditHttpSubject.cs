using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Skoruba.AuditLogging.Constants;

namespace Skoruba.AuditLogging.Events
{
    public class AuditHttpSubject : IAuditSubject
    {
        public AuditHttpSubject(IHttpContextAccessor accessor)
        {
            SubjectIdentifier = accessor.HttpContext.User.FindFirst(ClaimsConsts.Sub)?.Value;
            SubjectName = accessor.HttpContext.User.FindFirst(ClaimsConsts.Name)?.Value;
            SubjectAdditionalData = new
            {
                RemoteIpAddress = accessor.HttpContext.Connection?.RemoteIpAddress?.ToString(),
                LocalIpAddress = accessor.HttpContext.Connection?.LocalIpAddress?.ToString(),
                Claims = accessor.HttpContext.User.Claims?.Select(x=> new { x.Type, x.Value })
            };
        }

        public string SubjectName { get; set; }

        public string SubjectType { get; set; } = AuditSubjectTypes.User;

        public object SubjectAdditionalData { get; set; }

        public string SubjectIdentifier { get; set; }
    }
}