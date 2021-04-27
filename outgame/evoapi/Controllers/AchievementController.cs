using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using evolib.Databases.personal;

using evoapi.ProtocolModels;
using evoapi.ProtocolModels.Achievement;

namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class AchievementController : BaseController
	{
		public AchievementController(
			Services.IServicePack servicePack
		) : base(servicePack) { }

		[HttpPost]
		public async Task<IActionResult> GetList([FromBody]GetList.Request req)
		{
			var res = new GetList.Response();
			res.list = new List<GetList.Response.Achievement>();

			for (int i = 0; i < MasterData.AllAchievement.Count; i++)
			{
				bool obtained = 0 == (i % 2);

				var achi = MasterData.AllAchievement[i];

				res.list.Add(new GetList.Response.Achievement
				{
					achievementId = achi.achievementId,
					achievementValue = achi.value,
					value = achi.value - (obtained ? 0 : 1),
					obtained = obtained,
					obtainedDate = obtained?DateTime.UtcNow:DateTime.MinValue,
				});
			}

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> GetStatus([FromBody]GetStatus.Request req)
		{
			var res = new GetStatus.Response();
			res.playerId = req.playerId.Value;
			res.list = new List<AchievementStatusInfo>();

			var db = PDBSM.PersonalDBContext(res.playerId);

			var query = db.Achievements.Where(
				x => x.playerId == res.playerId);
			var records = await query.ToListAsync();

			foreach (var recordData in records)
			{
				AchievementStatusInfo statusInfo = new AchievementStatusInfo();
				statusInfo.achievementId = recordData.achievementId;
				statusInfo.value = recordData.value;
				statusInfo.notified = recordData.notified;
				statusInfo.obtained = recordData.obtained;
				statusInfo.obtainedDate = recordData.obtainedDate;

				res.list.Add(statusInfo);
			}

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> SaveStatus([FromBody]SaveStatus.Request req)
		{
			var res = new SaveStatus.Response();

			// プレイヤー毎
			foreach (var playerStatusInfo in req.list)
			{
				var playerId = playerStatusInfo.playerId;
				var db = PDBSM.PersonalDBContext(playerId);

				var query = db.Achievements.Where(
					x => x.playerId == playerId);
				var records = await query.ToListAsync();

				foreach (var statusInfo in playerStatusInfo.list)
				{
					var recordData = records.Find(i =>
						i.achievementId == statusInfo.achievementId);

					var achievement = MasterData.GetAchievement(statusInfo.achievementId);

					if (recordData != null)
					{
						recordData.notified = statusInfo.notified;
						recordData.obtained = statusInfo.obtained;
						recordData.obtainedDate = statusInfo.obtainedDate;

						// 功績達成時は、功績達成値を超えないように設定
						if (achievement != null && statusInfo.obtained)
						{
							recordData.value = Math.Min(recordData.value, achievement.value);
						}
						else
						{
							recordData.value = statusInfo.value;
						}
					}
					else
					{
						recordData = new evolib.Databases.personal.Achievement();
						recordData.achievementId = statusInfo.achievementId;
						recordData.playerId = playerId;
						recordData.notified = statusInfo.notified;
						recordData.obtained = statusInfo.obtained;
						recordData.obtainedDate = statusInfo.obtainedDate;

						// 功績達成時は、功績達成値を超えないように設定
						if (achievement != null && statusInfo.obtained)
						{
							recordData.value = Math.Min(recordData.value, achievement.value);
						}
						else
						{
							recordData.value = statusInfo.value;
						}

						await db.Achievements.AddAsync(recordData);
					}
				}

				await db.SaveChangesAsync();
			}

			return Ok(res);
		}
	}
}
