using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Skoruba.AuditLogging.Host.Helpers.Authentication
{
    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public virtual ClaimsIdentity Identity { get; set; } = default!;
    }
}