using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers
{
    public class TesteAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TesteAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[] { new Claim(ClaimTypes.Name, "usuarioTeste") };

            var identity = new ClaimsIdentity(claims, "Teste");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Teste");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
