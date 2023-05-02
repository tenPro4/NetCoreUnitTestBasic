using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using StationMonitorAPI.Configurations.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTest.Api2.Common;

namespace XTest.Api2.Fixtures
{
    public class BaseClassFixture : IClassFixture<TestFixture>
    {
        protected readonly HttpClient Client;
        protected readonly TestServer TestServer;
        public BaseClassFixture(TestFixture fixture)
        {
            Client = fixture.Client;
            TestServer = fixture.TestServer;
        }

        protected virtual void SetupAdminClaimsViaHeaders()
        {
            using (var scope = TestServer.Services.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<AppSettings>();
                Client.SetAdminClaimsViaHeaders(configuration);
            }
        }
    }
}
