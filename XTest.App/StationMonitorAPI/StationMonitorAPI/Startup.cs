using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StationMonitorAPI.Configurations.Constants;
using StationMonitorAPI.Configurations.Settings;
using StationMonitorAPI.Models;
using System.Text;

namespace StationMonitorAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterDbContexts(services);

            var appConfig = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
            services.AddSingleton(appConfig);

            services.AddControllers()
                 .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            );

            var fileSettingsSection = Configuration.GetSection(nameof(FileSettings));
            var fileSettings = fileSettingsSection.Get<FileSettings>();
            services.Configure<FileSettings>(fileSettingsSection);

            RegisterAuthentication(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseRouting();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        public virtual void RegisterDbContexts(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultCS")));
        }

        public virtual void RegisterAuthentication(IServiceCollection services)
        {
            var settingsSection = Configuration.GetSection(nameof(AppSettings));
            var settings = settingsSection.Get<AppSettings>();
            services.Configure<AppSettings>(settingsSection);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "StationMonitorAPI",
                    ValidAudience = "NuxtApp",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),
                });
        }
    }
}
