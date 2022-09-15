using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using XTest.Api.Infrastructure;
using XTest.Database.Models;
using XTest.UI;

namespace XTest.TestUI.Fixtures
{
    public class VotingFixture:WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureServices(services =>
            {
                services.AddAuthentication("Test")
                   .AddScheme<AuthenticationSchemeOptions, AuthMock>("Test", _ => { });

                services.AddAntiforgery(setup =>
                {
                    setup.Cookie.Name = "test_csrf_cookie";
                    setup.FormFieldName = "test_csrf_field";
                });

                DbContextUtils.ActionDatabase(services.BuildServiceProvider(), ctx =>
                {
                    ctx.VotingPolls.Add(new VotingPoll
                    {
                        Title = "title",
                        Description = "desc",
                        Counters = new List<Counter> {
                        new Counter { Name = "One" },
                        new Counter { Name = "Two" }
                    }
                    });
                    ctx.SaveChanges();
                });
            });
        }
    }

    public class AuthMock : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public AuthMock(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Email, "test@test.com") };
            var identity = new ClaimsIdentity(claims, "Test Voting System");
            var principal = new ClaimsPrincipal(new[] { identity });
            var ticket = new AuthenticationTicket(principal, "Test");
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
