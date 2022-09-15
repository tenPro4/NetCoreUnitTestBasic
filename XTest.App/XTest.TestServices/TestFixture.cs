using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace XTest.TestServices
{
    public class TestFixture<TStartup> : IDisposable where TStartup : class
    {
        public readonly TestServer Server;
        private readonly HttpClient Client;


        public TestFixture()
        {
            var builder = new WebHostBuilder()
                .UseStartup<TStartup>();

            Server = new TestServer(builder);
            Client = new HttpClient();
        }


        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }
    }
}