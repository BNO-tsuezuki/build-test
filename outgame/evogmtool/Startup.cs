using System;
using System.Threading.Tasks;
using AutoMapper;
using evogmtool.Handlers;
using evogmtool.Middlewares;
using evogmtool.Models;
using evogmtool.Repositories;
using evogmtool.Repositories.Game;
using evogmtool.Services;
using evogmtool.Services.Game;
using evogmtool.Utils;
using LogProcess.Databases.EvoGameLog;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using static evogmtool.Constants;

namespace evogmtool
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        private IHostingEnvironment CurrentEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<GmToolDbContext>(opt =>
                {
                    opt.UseMySql(Configuration.GetSection("RdbConnections").GetSection("GMTool").Value);
                })
                .AddDbContext<EvoGameLogDbContext>(opt =>
                {
                    opt.UseMySql(Configuration.GetSection("RdbConnections").GetSection("GameLog").Value);
                });

            services
               .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   // api/mvc error: 401 403
                   options.LoginPath = "/login.html";
                   options.AccessDeniedPath = "/error/403";
                   options.Events.OnRedirectToAccessDenied = ReplaceRedirector(StatusCodes.Status403Forbidden, options.Events.OnRedirectToAccessDenied);
                   options.Events.OnRedirectToLogin = ReplaceRedirector(StatusCodes.Status401Unauthorized, options.Events.OnRedirectToLogin);
               });

            // todo: ReturnUrl 制御できる？ LocalRedirect  IsLocalUri

            services
                .AddMvc(options =>
                {
                    options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            if (CurrentEnvironment.IsDevelopment())
            {
                services.AddOpenApiDocument(options =>
                {
                    options.PostProcess = document =>
                    {
                        document.Info.Title = "EVO GM TOOL API";
                    };
                });
            }

            services.AddAutoMapper(typeof(Startup));

            services.AddHttpContextAccessor();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policy.RoleLevel, policy => policy.Requirements.Add(new RoleLevelRequirement()));
                options.AddPolicy(Policy.DomainRegionTarget, policy => policy.Requirements.Add(new DomainRegionTargetRequirement()));
            });

            AddAuthorizationHandlers(services);

            AddServices(services);

            AddRepositories(services);

            services.AddHttpClient<EvoToolClient>(client =>
            {
                var toolServer = Configuration.GetSection("ToolServer");
                client.BaseAddress = new Uri($"http://{toolServer.GetValue<string>("addr")}:{toolServer.GetValue<string>("port")}");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // todo: exception log

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }

            // mvc error: except 401 403 exception
            app.UseStatusCodePages(context =>
            {
                var request = context.HttpContext.Request;
                var response = context.HttpContext.Response;

                if (!request.Path.StartsWithSegments("/api"))
                {
                    context.HttpContext.Response.Redirect($"/error/{response.StatusCode}");
                }

                return Task.CompletedTask;
            });

            // mvc error: exception
            app.UseExceptionHandler(options =>
            {
                options.Run(context =>
                {
                    if (!context.Request.Path.StartsWithSegments("/api"))
                    {
                        context.Response.Redirect("/error/500");
                    }

                    return Task.CompletedTask;
                });
            });

            app.UseStaticFiles();
            app.UseAuthentication();
            //app.UseSession();  // todo: 今のところ使わない

            app.UseMiddleware<OperationLoggerMiddleware>();

            app.UseMvc();
        }

        private static Func<RedirectContext<CookieAuthenticationOptions>, Task> ReplaceRedirector(
            int statusCode,
            Func<RedirectContext<CookieAuthenticationOptions>, Task> existingRedirector) =>
            context =>
            {
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.StatusCode = statusCode;
                    return Task.CompletedTask;
                }

                return existingRedirector(context);
            };

        private static void AddAuthorizationHandlers(IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, RoleLevelAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, DomainRegionTargetAuthorizationHandler>();
        }

        private static void AddServices(IServiceCollection services)
        {
            // GMTool
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IDomainRegionService, DomainRegionService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IPublisherService, PublisherService>();
            services.AddScoped<ITimezoneService, TimezoneService>();
            services.AddScoped<IUserService, UserService>();

            // EvoTool
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IGashaService, GashaService>();
            services.AddScoped<IMapService, MapService>();
            services.AddScoped<IMiscService, MiscService>();
            services.AddScoped<IOpsNoticeService, OpsNoticeService>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IVersionService, VersionService>();

            // GameLog
            services.AddScoped<IGameLogService, GameLogService>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // GMTool
            services.AddTransient<IOperationLoggerRepository, OperationLoggerRepository>();
            services.AddScoped<IAuthLoggerRepository, AuthLoggerRepository>();
            services.AddScoped<IDomainRegionRepository, DomainRegionRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<ILoginUserRepository, LoginUserRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IPublisherRepository, PublisherRepository>();
            services.AddScoped<ITimezoneRepository, TimezoneRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // EvoTool
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IGashaRepository, GashaRepository>();
            services.AddScoped<IMapRepository, MapRepository>();
            services.AddScoped<IMiscRepository, MiscRepository>();
            services.AddScoped<IOpsNoticeRepository, OpsNoticeRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IVersionRepository, VersionRepository>();

            // GameLog
            services.AddScoped<IGameLogRepository, GameLogRepository>();
        }
    }
}
