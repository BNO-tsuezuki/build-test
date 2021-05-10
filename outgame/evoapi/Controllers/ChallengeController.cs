using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using evolib.Databases.personal;

using evoapi.ProtocolModels;
using evoapi.ProtocolModels.Challenge;

namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class ChallengeController : BaseController
	{
		public ChallengeController(
			Services.IServicePack servicePack
		) : base(servicePack) { }

		[HttpPost]
		public async Task<IActionResult> GetList([FromBody]GetList.Request req)
		{
			var playerId = SelfHost.playerInfo.playerId;
			var db = PDBSM.PersonalDBContext(playerId);

			var srcList = await evolib.ChallengeList.GetAsync(playerId, MasterData, db);

			var ret = new GetList.Response();

			ret.list = srcList.Select(r =>
					{
						var isPaidChallenge = false;
						var unlocked = true;
						if (r.type != evolib.Challenge.Type.Beginner)
						{
							// TODO: 抽選で有償枠に入ったチャレンジの時にtrueを設定
							isPaidChallenge = false;

							unlocked = r.unlocked;
						}

						return new ChallengeStatus()
						{
							challengeId = r.challengeId,
							num = r.value,
							completed = (r.status == evolib.Challenge.Status.Clear),
							type = r.type,
							isPaidChallenge = isPaidChallenge,
							unlocked = unlocked,
							expirationDate = r.expirationDate,
						};

					})
					.ToList();

			var beginnerTarget = srcList
								.FirstOrDefault(r => r.type == evolib.Challenge.Type.Beginner &&
												r.unlocked && r.status != evolib.Challenge.Status.Clear);

			ret.currentSheet = String.Empty;
			if (beginnerTarget != null)
			{
				var challengeInfo = MasterData.GetChallenge(beginnerTarget.challengeId);
				if (challengeInfo != null)
				{
					ret.currentSheet = challengeInfo.sheetId;
				}
			}

			return Ok(ret);
		}
	}
}
