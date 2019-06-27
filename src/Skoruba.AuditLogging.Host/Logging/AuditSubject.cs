using System.Linq;
using Microsoft.AspNetCore.Http;
using Skoruba.AuditLogging.Events;
using Skoruba.AuditLogging.Host.Consts;

namespace Skoruba.AuditLogging.Host.Logging
{
    public class AuditSubject : IAuditSubject
    {
        public AuditSubject(IHttpContextAccessor accessor)
        {
            SubjectIdentifier = accessor.HttpContext.User.FindFirst(AuthenticationConsts.ClaimSub)?.Value;
            SubjectName = accessor.HttpContext.User.FindFirst(AuthenticationConsts.ClaimName)?.Value;
            SubjectAdditionalData = new
            {
                RemoteIpAddress = accessor.HttpContext.Connection?.RemoteIpAddress?.ToString(),
                Claims = accessor.HttpContext.User.Claims?.ToDictionary(t=> t.Type, t => t.Value)
            };
        }

        public string SubjectName { get; set; }

        public string SubjectType { get; set; } = AuditSubjectTypes.User;

        public object SubjectAdditionalData { get; set; }

        public string SubjectIdentifier { get; set; }
    }
}