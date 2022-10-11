using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTest.Database;

namespace XTest.Api.Infrastructure
{
    public class DbContextUtils
    {
        public static void ActionDatabase(IServiceProvider provider, Action<AppDbContext> action)
        {
            using (var scope = provider.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                action(ctx);
            }
        }
    }
}
