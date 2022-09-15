using Blog.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var host = new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
             .UseUrls("http://localhost:5000/")
            .UseStartup<Startup>()
            .Build();

        host.Run();
    }
}