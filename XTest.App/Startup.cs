using AspNet.Security.OAuth.Validation;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XTest.App.Infrastructure;
using XTest.App.Infrastructure.Middlewares;
using XTest.Database;
using XTest.Database.Models;
using XTesting.Services;
using XTesting.Services.Infrastructure.ErrorHandler;
using XTesting.Services.Repositories;
using XTesting.Services.Services;

namespace XTest.App
{
    public partial class Startup
    {
        private readonly IConfiguration _config;

        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment env,IConfiguration config)
        {
            //need set this if not test module cant target appsettings location
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", true, true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            builder.AddEnvironmentVariables();
            _config = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => {
                options.UseSqlServer(_config.GetConnectionString("Main"));
                options.UseOpenIddict();
            });

            services.AddIdentity<AppUser, IdentityRole>(opts => {
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            })
           .AddEntityFrameworkStores<AppDbContext>()
           .AddDefaultTokenProviders();

            services.AddOpenIddict()

    // Register the OpenIddict core components.
    .AddCore(options =>
    {
            // Configure OpenIddict to use the Entity Framework Core stores and models.
            // Note: call ReplaceDefaultEntities() to replace the default entities.
        options.UseEntityFrameworkCore()
               .UseDbContext<AppDbContext>();
    })

    // Register the OpenIddict server components.
    .AddServer(options =>
    {
            // Enable the token endpoint.
        options.SetTokenEndpointUris("/connect/token");

            // Enable the client credentials flow.
        options.AllowClientCredentialsFlow();

            // Register the signing and encryption credentials.
        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();
    })

    // Register the OpenIddict validation components.
    .AddValidation(options =>
    {
            // Import the configuration from the local OpenIddict server instance.
        options.UseLocalServer();
    });

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            //services.AddAuthentication("Cookie")
            //    .AddCookie("Cookie");
            services.AddAuthentication(IdentityConstants.ApplicationScheme).AddOAuthValidation();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Privileged", policy => policy
                    .AddAuthenticationSchemes(IdentityConstants.ApplicationScheme)
                    .RequireClaim("CustomRoleType", "God")
                    .RequireAuthenticatedUser());

                options.AddPolicy("Admin", policy => policy
                    .AddAuthenticationSchemes(IdentityConstants.ApplicationScheme)
                    .RequireClaim("CustomRoleType", "admin")
                    .RequireAuthenticatedUser());
            });

            services.AddSwaggerGen();
            services.AddSingleton<IVotingPollFactory, VotingPollFactory>();
            services.AddSingleton<ICounterManager, CounterManager>();
            services.AddScoped<IVotingSystemPersistance, VotingSystemPersistance>();

            services.AddTransient<IBaseRepository<Course>, BaseRepository<Course>>();
            services.AddTransient<IBaseService<Course>, BaseService<Course>>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IErrorHandler, ErrorHandler>();

            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));

            services.Configure<FileSettings>(_config.GetSection(nameof(FileSettings)));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
