using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


using evolib.Databases.personal;
using evolib.Log;

using evoapi.ProtocolModels;
using evoapi.ProtocolModels.Option;



namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class OptionController : BaseController
	{
		string CommonMobileSuitId = "Common";

		public OptionController(
            Services.IServicePack servicePack
        ) : base(servicePack) { }


        [HttpPost]
		public async Task<IActionResult> GetOptions([FromBody]GetOptions.Request req)
		{
			// TODO 190611 DedicatedServerが本プロトコルを投げてくるので暫定処理
			if (SelfHost.hostType != evolib.HostType.Player)
			{
				return Ok(new GetOptions.Response()
				{
					appOptions = new List<GetOptions.Response.AppOption>(),
					mobileSuitOptions = new List<GetOptions.Response.MobileSuitOption>(),
				});
			}

			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);


			var res = new GetOptions.Response();

			res.appOptions = new List<GetOptions.Response.AppOption>();
			for ( int i=0; i<=(int)AppOption.Const.MaxCategory; i++)
			{
				var reco = await db.AppOptions.FindAsync(SelfHost.playerInfo.playerId, i);
				if(reco == null)
				{
					reco = new AppOption()
					{
						playerId = SelfHost.playerInfo.playerId,
						category = i,
						//TODO set from master data.

					};
					await db.AppOptions.AddAsync(reco);
				}

				res.appOptions.Add(new GetOptions.Response.AppOption()
				{
					category = reco.category,
					values = reco.Values(),
				});
			}

			res.mobileSuitOptions = new List<GetOptions.Response.MobileSuitOption>();
			var allMsIds = MasterData.AllMobileSuitIds;
			allMsIds.Add(CommonMobileSuitId);
			foreach ( var msId in allMsIds )
			{
				var reco = await db.MobileSuitOptions.FindAsync(SelfHost.playerInfo.playerId, msId);
				if( reco == null)
				{
					reco = new MobileSuitOption()
					{
						playerId = SelfHost.playerInfo.playerId,
						mobileSuitId = msId,
						//TODO set from master data.
					};
					await db.MobileSuitOptions.AddAsync(reco);
				}

				res.mobileSuitOptions.Add(new GetOptions.Response.MobileSuitOption()
				{
					mobileSuitId = reco.mobileSuitId,
					values = reco.Values(),
				});
			}

			await db.SaveChangesAsync();
			
			return Ok(res);
		}


		[HttpPost]
		public async Task<IActionResult> SetAppOption([FromBody]SetAppOption.Request req)
		{
			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			var reco = await db.AppOptions.FindAsync(SelfHost.playerInfo.playerId, req.category);
			if( reco == null)
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}

            Logger.Logging(
                new LogObj().AddChild(new LogModels.SetAppOption
                {
                    PlayerId = SelfHost.playerInfo.playerId,
                    Date = DateTime.UtcNow,
                    Category = req.category,
                    Values = req.values,
                })
            );

            for ( var i=0; i<req.values.Length; i++ )
			{
				reco[i] = req.values[i];
			}
			await db.SaveChangesAsync();

			return Ok(new SetAppOption.Response());
		}

		[HttpPost]
		public async Task<IActionResult> SetMobileSuitOptions([FromBody]SetMobileSuitOptions.Request req)
		{
			foreach( var opt in req.options)
			{
				if( opt.mobileSuitId != CommonMobileSuitId
					&& null == MasterData.GetMobileSuit(opt.mobileSuitId))
				{
					return BuildErrorResponse(Error.LowCode.BadParameter);
				}
			}

			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);


			var query = db.MobileSuitOptions.Where(r => r.playerId == SelfHost.playerInfo.playerId);
			var recList = await query.ToListAsync();

			foreach (var opt in req.options)
			{
				var rec = recList.Find(r => r.mobileSuitId == opt.mobileSuitId);
				if (rec == null) continue;

				for( var i=0; i<opt.values.Length; i++)
				{
					rec[i] = opt.values[i];
				}
			}

			await db.SaveChangesAsync();

			return Ok(new SetMobileSuitOptions.Response());
		}
	}
}
