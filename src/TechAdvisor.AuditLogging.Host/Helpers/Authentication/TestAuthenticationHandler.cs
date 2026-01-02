using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechAdvisor.AuditLogging.Host.Consts;

namespace TechAdvisor.AuditLogging.Host.Helpers.Authentication
{
    public class TestAuthenticationHandler(
        IOptionsMonitor<TestAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : AuthenticationHandler<TestAuthenticationOptions>(options, logger, encoder)
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authenticationTicket = new AuthenticationTicket(new ClaimsPrincipal(Options.Identity),
                new AuthenticationProperties(), AuthenticationConsts.AuthenticationType);

            return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
        }
    }
}