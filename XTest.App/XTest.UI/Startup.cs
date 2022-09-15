using Microsoft.EntityFrameworkCore;
using XTest.Database;
using XTesting.Services;

namespace XTest.UI
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("Cookie")
                .AddCookie("Cookie");

            services.AddDbContext<AppDbContext>(options => {
                options.UseInMemoryDatabase("Database");
            });

            services.AddSingleton<IVotingPollFactory, VotingPollFactory>();
            services.AddSingleton<ICounterManager, CounterManager>();
            services.AddScoped<IVotingSystemPersistance, VotingSystemPersistance>();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
