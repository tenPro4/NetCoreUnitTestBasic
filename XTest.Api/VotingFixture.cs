using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using XTest.Api.Infrastructure;
using XTest.App;
using XTest.Database.Models;

namespace XTest.Api
{
    public class VotingFixture:WebApplicationFactory<Startup>,IAsyncLifetime
    {
        public Stream TestFile { get; private set; }
        private string _cleanupPath;

        public WebApplicationFactory<Startup> AuthenticatedInstance(params Claim[] claimSeed)
        {
            return WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IAuthenticationSchemeProvider, MockSchemeProvider>();
                    services.AddSingleton<MockClaimSeed>(_ => new(claimSeed));
                });
            });
        }

        public async Task InitializeAsync()
        {
            TestFile = await GetTestImage();
        }

        private async Task<Stream> GetTestImage()
        {
            var memoryStream = new MemoryStream();
            var fileStream = File.OpenRead("base.png");
            await fileStream.CopyToAsync(memoryStream);
            fileStream.Close();
            return memoryStream;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureServices(services =>
            {
                services.Configure<FileSettings>(fs =>
                {
                    fs.Path = "test_images";
                });

                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
                _cleanupPath = Path.Combine(env.WebRootPath, "test_images");

                DbContextUtils.ActionDatabase(serviceProvider, ctx =>
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

        public Task DisposeAsync()
        {
            var directoryInfo = new DirectoryInfo(_cleanupPath);
            if (directoryInfo.Exists)
            {
                foreach (var file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }

                Directory.Delete(_cleanupPath);
            }

            return Task.CompletedTask;
        }
    }

    public class MockSchemeProvider : AuthenticationSchemeProvider
    {
        public MockSchemeProvider(IOptions<AuthenticationOptions> options) : base(options)
        {
        }

        protected MockSchemeProvider(
            IOptions<AuthenticationOptions> options,
            IDictionary<string, AuthenticationScheme> schemes
        )
            : base(options, schemes)
        {
        }

        public override Task<AuthenticationScheme> GetSchemeAsync(string name)
        {
            AuthenticationScheme mockScheme = new(
                IdentityConstants.ApplicationScheme,
                IdentityConstants.ApplicationScheme,
                typeof(MockAuthenticationHandler)
            );
            return Task.FromResult(mockScheme);
        }
    }

    public class MockAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly MockClaimSeed _claimSeed;

        public MockAuthenticationHandler(
            MockClaimSeed claimSeed,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
        )
            : base(options, logger, encoder, clock)
        {
            _claimSeed = claimSeed;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claimsIdentity = new ClaimsIdentity(_claimSeed.getSeeds(), IdentityConstants.ApplicationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var ticket = new AuthenticationTicket(claimsPrincipal, IdentityConstants.ApplicationScheme);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

    public class MockClaimSeed
    {
        private readonly IEnumerable<Claim> _seed;

        public MockClaimSeed(IEnumerable<Claim> seed)
        {
            _seed = seed;
        }

        public IEnumerable<Claim> getSeeds() => _seed;
    }
}