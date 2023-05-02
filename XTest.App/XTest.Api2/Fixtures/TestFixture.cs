using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using StationMonitorAPI;
using StationMonitorAPI.Configurations.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTest.Api2.Fixtures
{
    public class TestFixture
    {
        public TestServer TestServer;

        public HttpClient Client { get; }
        public TestFixture()
        {
            var builder = new WebHostBuilder()
                 .ConfigureAppConfiguration((hostContext, configApp) =>
                 {
                     configApp.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                     var env = hostContext.HostingEnvironment;

                     configApp.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                 })
                 .UseStartup<StartupTest>();

            TestServer = new TestServer(builder);
            Client = TestServer.CreateClient();
        }
    }
}
