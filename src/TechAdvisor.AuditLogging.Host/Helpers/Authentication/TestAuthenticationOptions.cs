using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace TechAdvisor.AuditLogging.Host.Helpers.Authentication
{
    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public virtual ClaimsIdentity Identity { get; set; }
    }
}