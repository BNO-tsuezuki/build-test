using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace evolib.Databases.personal
{
	public class PersonalDBShardManager
	{
		class ConnStrsManager
		{
			List<string> ConnStrs { get; set; }
			public ConnStrsManager(IConfiguration conf)
			{
				ConnStrs = new List<string>();
				foreach (var section in conf.GetSection("RdbConnections").GetSection("Personal").GetChildren())
				{
					ConnStrs.Add(section.Value);
				}
			}

			public string Get(int index)
			{
				var updateDBShardIndex = Environment.GetEnvironmentVariable("UPDATE_DATABASE_PERSONAL_SHARD_INDEX");
				if (!string.IsNullOrEmpty(updateDBShardIndex))
				{
					index = int.Parse(updateDBShardIndex);
				}

				if (index < 0 || ConnStrs.Count <= index) return null;
				return ConnStrs[index];
			}
		}


		public class PersonalShard001 : PersonalDBContext { public PersonalShard001(DbContextOptions<PersonalShard001> opt) : base(opt) { } }
		public class PersonalShard002 : PersonalDBContext { public PersonalShard002(DbContextOptions<PersonalShard002> opt) : base(opt) { } }
		public class PersonalShard003 : PersonalDBContext { public PersonalShard003(DbContextOptions<PersonalShard003> opt) : base(opt) { } }
		public class PersonalShard004 : PersonalDBContext { public PersonalShard004(DbContextOptions<PersonalShard004> opt) : base(opt) { } }
		public class PersonalShard005 : PersonalDBContext { public PersonalShard005(DbContextOptions<PersonalShard005> opt) : base(opt) { } }
		public class PersonalShard006 : PersonalDBContext { public PersonalShard006(DbContextOptions<PersonalShard006> opt) : base(opt) { } }
		public class PersonalShard007 : PersonalDBContext { public PersonalShard007(DbContextOptions<PersonalShard007> opt) : base(opt) { } }
		public class PersonalShard008 : PersonalDBContext { public PersonalShard008(DbContextOptions<PersonalShard008> opt) : base(opt) { } }


		static int DbContextCount { get; set; }

		static void AddDbContext<TContext>(IServiceCollection services, ILoggerFactory loggerFactory, string connStr )
			where TContext : DbContext
		{
			if (string.IsNullOrEmpty(connStr)) return;

			services.AddDbContext<TContext>(opt =>
			{
				opt.UseMySql(connStr);
				opt.UseLoggerFactory(loggerFactory);
			});

			DbContextCount++;
		}

		public static void AddService( IServiceCollection services, IConfiguration conf, ILoggerFactory loggerFactory)
		{
			DbContextCount = 0;

			var connStrsMan = new ConnStrsManager(conf);

			var i = 0;
			AddDbContext<PersonalShard001>(services, loggerFactory, connStrsMan.Get(i++));
			AddDbContext<PersonalShard002>(services, loggerFactory, connStrsMan.Get(i++));
			AddDbContext<PersonalShard003>(services, loggerFactory, connStrsMan.Get(i++));
			AddDbContext<PersonalShard004>(services, loggerFactory, connStrsMan.Get(i++));
			AddDbContext<PersonalShard005>(services, loggerFactory, connStrsMan.Get(i++));
			AddDbContext<PersonalShard006>(services, loggerFactory, connStrsMan.Get(i++));
			AddDbContext<PersonalShard007>(services, loggerFactory, connStrsMan.Get(i++));
			AddDbContext<PersonalShard008>(services, loggerFactory, connStrsMan.Get(i++));
		}


		List<PersonalDBContext> DBsList = new List<PersonalDBContext>();

		public PersonalDBShardManager(params PersonalDBContext[] dbContexts)
		{
			for (int i = 0; i < DbContextCount; i++)
			{
				DBsList.Add(dbContexts[i]);
			}
		}

		public PersonalDBContext PersonalDBContext(long key)
		{
			return DBsList[(int)(key % DBsList.Count)];
		}
	}
}
