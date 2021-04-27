using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using evolib.FamilyServerInfo;
using evolib.Kvs;
using evolib.Databases;
using evolib.Databases.personal;
using evolib.Databases.common1;
using evolib.Databases.common2;
using evolib.Databases.common3;
using evolib.Services.MasterData;
using evolib.DeliveryData;

namespace evoapi
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

				Kvs.Initialize(map);
			}

			EvoApiJwt.issuer = Configuration["Jwt:issuer"];
			EvoApiJwt.audience = Configuration["Jwt:audience"];
			EvoApiJwt.expiryMinutes = int.Parse(Configuration["Jwt:expiryMinutes"]);

			MatchingServerInfo.Initialize(Configuration);
			SequencingServerInfo.Initialize(Configuration);
			AuthenticationServerInfo.Initialize(Configuration);

			DeliveryDataInfo.Initialize(Configuration);

			evolib.SystemInfo.Initialize(Configuration);

            evolib.VivoxInfo.Initialize(Configuration);

            Task.Run(async () =>
			{
				while(!await MatchingServerInfo.SetupMultiMatchingServersAsync())
				{
					await Task.Delay(5000);
				}

				while (true)
				{
					await MasterDataLoader.LoadAsync();
					await Task.Delay(5000);
				}
			});

			evolib.Kvs.Models.Session.ExpiredSpan = TimeSpan.FromSeconds(
				ProtocolModels.HandShake.HandShake.NextResponseSeconds + 60);
		}

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// ----------------------
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
			PersonalDBShardManager.AddService(services, Configuration, dbLoggerFactory );
			// ~Databases
			// ----------------------


			// services.AddDistributedRedisCache(opt =>
			// {
			// 	opt.Configuration = Configuration.GetConnectionString("GameCacheConnection");
			// 	opt.InstanceName = "EvoGame";
			// });


			//services.AddSession( opt=>
			//{
			//	opt.Cookie.Name = Configuration["Session:CookieName"];
			//});

			DbContextFactory.Initialize(Configuration);

			
			evolib.VersionChecker.Start();
			evolib.OpsNoticeManager.Start();
			evolib.DisabledMobileSuit.Start();


			//services.AddSingleton<XXXX>(xxxx);
			services.AddScoped<evolib.Log.ILogObj, evolib.Log.LogObj>();
			services.AddScoped<Services.IServicePack, Services.ServicePack>();

			services.Configure<ForwardedHeadersOptions>(opt =>
			{
				opt.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor;
				
				//default
				//opt.ForwardLimit = 1;
			});


			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			if ( env.EnvironmentName == "Development" )
			{
				app.UseDeveloperExceptionPage();
			}

			if (env.EnvironmentName == "DevEnv")
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseForwardedHeaders();

			//app.UseSession();
			app.UseMiddleware<Middlewares.FirstSetting>();
			app.UseMiddleware<evolib.Middlewares.RequestHead>();
			app.UseMiddleware<Middlewares.Authentication>();
			app.UseMiddleware<evolib.Middlewares.RequestTail>();

			app.UseMvc();
        }
    }
}
