using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using evolib.Log;

using evoapi.ProtocolModels;
using evoapi.ProtocolModels.ReportPlayer;



namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class PlayerReportsController : BaseController
	{
		public PlayerReportsController(
            Services.IServicePack servicePack
        ) : base(servicePack) { }

        [HttpPost]
		public IActionResult ReportPlayer([FromBody]ReportPlayer.Request req)
		{
            // 通報内容をログとして保存
            Logger.Logging(new LogObj().AddChild(new LogModels.ReportPlayer
            {
                ReportingPlayerId = SelfHost.playerInfo.playerId,
                Date = DateTime.UtcNow,
                PlayerId = req.playerId,
                MatchId = req.matchId,
                Uncooperative = req.selectedReasons.uncooperative,
                Sabotage = req.selectedReasons.sabotage,
                Cheat = req.selectedReasons.cheat,
                Harassment = req.selectedReasons.harassment,
                Abuse = req.selectedReasons.abuse,
                HateSpeech = req.selectedReasons.hateSpeech,
                InappropriateName = req.selectedReasons.inappropriateName,
                Comment = req.comment,
            }));

            var res = new ReportPlayer.Response();
			return Ok(res);
		}
	}
}
