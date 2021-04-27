using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

using evolib.Databases;
using evolib.Databases.personal;
using evolib.Databases.common2;





namespace evosequencing
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

		}

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			// Databases~
			var dbLoggerFactory = new LoggerFactory(new[] { new SqlLoggerProvider() });
			services.AddDbContext<Common2DBContext>(opt =>
			{
				opt.UseMySql(Configuration.GetSection("RdbConnections").GetSection("Common2").Value);

				opt.UseLoggerFactory(dbLoggerFactory);
			});
			PersonalDBShardManager.AddService(services, Configuration, dbLoggerFactory);
			// ~Databases


			services.AddScoped<evolib.Log.ILogObj, evolib.Log.LogObj>();
			services.AddScoped<Services.IServicePack, Services.ServicePack>();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

			app.UseMiddleware<Middlewares.FirstSetting>();
			app.UseMiddleware<evolib.Middlewares.RequestHead>();
			app.UseMiddleware<evolib.Middlewares.RequestTail>();

			//app.UseHttpsRedirection();
			app.UseMvc();
        }
    }
}
