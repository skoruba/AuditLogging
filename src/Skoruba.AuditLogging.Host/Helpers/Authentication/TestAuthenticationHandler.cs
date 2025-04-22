using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Skoruba.AuditLogging.Host.Consts;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Skoruba.AuditLogging.Host.Helpers.Authentication
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