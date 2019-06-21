using Microsoft.AspNetCore.Http;
using Skoruba.AuditLogging.Events;
using Skoruba.AuditLogging.Host.Consts;

namespace Skoruba.AuditLogging.Host.Logging
{
    public class AuditCaller : IAuditCaller
    {
        public AuditCaller(IHttpContextAccessor accessor)
        {
            SubjectIdentifier = accessor.HttpContext.User.FindFirst(AuthenticationConsts.ClaimSub)?.Value;
            SubjectName = accessor.HttpContext.User.FindFirst(AuthenticationConsts.ClaimName)?.Value;
        }

        public string SubjectName { get; set; }

        public string SubjectIdentifier { get; set; }
    }
}