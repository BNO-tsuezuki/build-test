using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


using evolib.Databases.common1;
using evolib.Databases.common2;
using evolib.Databases.common3;
using evolib.Databases.personal;


namespace evolib.Databases
{
	public static class DbContextFactory
	{
		static DbContextOptions<Common1DBContext> Common1Opts { get; set; }
		static DbContextOptions<Common2DBContext> Common2Opts { get; set; }
		static DbContextOptions<Common3DBContext> Common3Opts { get; set; }

		static List<DbContextOptions<PersonalDBContext>> PersonalOptsList { get; set; }


		public static void Initialize(IConfiguration configuration)
		{
			var dbLoggerFactory = new LoggerFactory(new[] { new SqlLoggerProvider() });

			var db1OptBuilder = new DbContextOptionsBuilder<Common1DBContext>();
			db1OptBuilder.UseMySql(
				configuration.GetSection("RdbConnections").GetSection("Common1").Value);
			db1OptBuilder.UseLoggerFactory(dbLoggerFactory);
			Common1Opts = db1OptBuilder.Options;

			var db2OptBuilder = new DbContextOptionsBuilder<Common2DBContext>();
			db2OptBuilder.UseMySql(
				configuration.GetSection("RdbConnections").GetSection("Common2").Value);
			db2OptBuilder.UseLoggerFactory(dbLoggerFactory);
			Common2Opts = db2OptBuilder.Options;

			var db3OptBuilder = new DbContextOptionsBuilder<Common3DBContext>();
			db3OptBuilder.UseMySql(
				configuration.GetSection("RdbConnections").GetSection("Common3").Value);
			db3OptBuilder.UseLoggerFactory(dbLoggerFactory);
			Common3Opts = db3OptBuilder.Options;

			PersonalOptsList = new List<DbContextOptions<PersonalDBContext>>();
			foreach (var section in configuration.GetSection("RdbConnections").GetSection("Personal").GetChildren())
			{
				var dbOptBuilder = new DbContextOptionsBuilder<PersonalDBContext>();
				dbOptBuilder.UseMySql(section.Value);
				dbOptBuilder.UseLoggerFactory(dbLoggerFactory);
				PersonalOptsList.Add(dbOptBuilder.Options);
			}
		}

		public static Common1DBContext CreateCommon1()
		{
			return new Common1DBContext(Common1Opts);
		}
		public static Common2DBContext CreateCommon2()
		{
			return new Common2DBContext(Common2Opts);
		}
		public static Common3DBContext CreateCommon3()
		{
			return new Common3DBContext(Common3Opts);
		}


		public class PersonalDbList
		{
			Dictionary<int, PersonalDBContext> _DbMap = new Dictionary<int, PersonalDBContext>();

			public PersonalDBContext PersonalDBContext(long playerId)
			{
				var idx = (int)(playerId % PersonalOptsList.Count);
				if (!_DbMap.ContainsKey(idx))
				{
					_DbMap[idx] = new PersonalDBContext(PersonalOptsList[idx]);
				}

				return _DbMap[idx];
			}
		}
		public static PersonalDbList CreatePersonalDbList()
		{
			return new PersonalDbList();
		}
	}
}
