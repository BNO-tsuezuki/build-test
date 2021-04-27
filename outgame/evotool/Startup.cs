using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

using evolib.DeliveryData;
using evolib.FamilyServerInfo;
using evolib.Databases;
using evolib.Databases.personal;
using evolib.Databases.common1;
using evolib.Databases.common2;
using evolib.Databases.common3;
using evotool.Services.Gdpr;
using evotool.Services.GMTool;
using AutoMapper;

namespace evotool
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
			Configuration = configuration;

			{
				var map = new Dictionary<string, List<String>>();

				var kvsConnectionsSection = Configuration.GetSection("KvsConnections");
				foreach (var item in kvsConnectionsSection.GetChildren())
				{
					map[item.Key] = item.Get<List<string>>();
				}

				evolib.Kvs.Kvs.Initialize(map);
			}

			EvoToolJwt.issuer = Configuration["Jwt:issuer"];
			EvoToolJwt.audience = Configuration["Jwt:audience"];
			EvoToolJwt.expiryMinutes = int.Parse(Configuration["Jwt:expiryMinutes"]);

			MatchingServerInfo.Initialize(Configuration);
			AuthenticationServerInfo.Initialize(Configuration);

			DeliveryDataInfo.Initialize(Configuration);

			DbContextFactory.Initialize(Configuration);

			System.Threading.Tasks.Task.Run(async () =>
			{
				using (var commonDB2 = DbContextFactory.CreateCommon2())
				{
					await TranslationTable.LoadAsync(commonDB2);
				}
			});
		}

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			// Databases~
			var dbLoggerFactory = new LoggerFactory(new[] { new SqlLoggerProvider() });
			services.AddDbContext<Common1DBContext>(opt =>
			{
				opt.UseMySql(Configuration.GetSection("RdbConnections").GetSection("Common1").Value);

				opt.UseLoggerFactory(dbLoggerFactory);
			});
			services.AddDbContext<Common2DBContext>(opt =>
			{
				opt.UseMySql(Configuration.GetSection("RdbConnections").GetSection("Common2").Value);

				opt.UseLoggerFactory(dbLoggerFactory);
			});
			services.AddDbContext<Common3DBContext>(opt =>
			{
				opt.UseMySql(Configuration.GetSection("RdbConnections").GetSection("Common3").Value);

				opt.UseLoggerFactory(dbLoggerFactory);
			});
			PersonalDBShardManager.AddService(services, Configuration, dbLoggerFactory);
			// ~Databases


			services.AddSession(opt =>
			{
				opt.Cookie.Name = "evotool-session-id";
				opt.Cookie.MaxAge = TimeSpan.MaxValue;
			});

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<evolib.Log.ILogObj, evolib.Log.LogObj>();
			services.AddScoped<Services.IServicePack, Services.ServicePack>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IMiscService, MiscService>();
            services.AddScoped<IOpsNoticeService, OpsNoticeService>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<ITranslationService, TranslationService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IVersionService, VersionService>();

            services.AddScoped<IGdprService, GdprService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			if (env.EnvironmentName == "Development")
			{
				app.UseDeveloperExceptionPage();

				app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
				{
					HotModuleReplacement = true,
					//ReactHotModuleReplacement = true
				});
			}

			if (env.EnvironmentName == "DevEnv")
			{
				app.UseDeveloperExceptionPage();
			}

			// TODO: app.UseMiddleware<EvoToolAuthentication>();

			app.UseStaticFiles();

			//app.UseSession();

			app.UseMiddleware<Middlewares.FirstSetting>();
			app.UseMiddleware<evolib.Middlewares.RequestHead>();
			app.UseMiddleware<evolib.Middlewares.RequestTail>();

			app.UseMvc(
				routes =>
				{
					routes.MapRoute(
						name: "default",
						template: "{controller=Home}/{action=Index}/{id?}");

					
				}
			);
		}
    }
}
