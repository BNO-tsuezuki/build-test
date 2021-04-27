using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using evolib.Databases.personal;
using evolib.Databases.common1;
using evolib.Databases.common2;
using evolib.Databases.common3;
using evolib.Log;

using evosequencing.Services;


namespace evosequencing.Middlewares
{
	public class FirstSetting
	{
		private readonly RequestDelegate _next;

		public FirstSetting(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context,
			IServicePack servicePack,
			Common2DBContext common2DBContext,
			PersonalDBShardManager.PersonalShard001 personalShard001,
			PersonalDBShardManager.PersonalShard002 personalShard002,
			PersonalDBShardManager.PersonalShard003 personalShard003,
			PersonalDBShardManager.PersonalShard004 personalShard004,
			PersonalDBShardManager.PersonalShard005 personalShard005,
			PersonalDBShardManager.PersonalShard006 personalShard006,
			PersonalDBShardManager.PersonalShard007 personalShard007,
			PersonalDBShardManager.PersonalShard008 personalShard008,
			ILogObj logObj )
		{
			servicePack.Common2DBContext = common2DBContext;
			servicePack.PersonalDBShardManager = new PersonalDBShardManager(
				personalShard001,
				personalShard002,
				personalShard003,
				personalShard004,
				personalShard005,
				personalShard006,
				personalShard007,
				personalShard008
			);

			//log
			servicePack.Log = logObj;

			await _next(context);

			Logger.Logging(logObj);
		}
	}
}
