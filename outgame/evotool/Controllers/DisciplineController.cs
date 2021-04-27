using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using evolib.Log;

using evotool.ProtocolModels.Discipline;

namespace evotool.Controllers
{
	[Route("api/[controller]/[action]")]
	public class DisciplineController : BaseController
	{
		public DisciplineController(
			Services.IServicePack servicePack
		) : base(servicePack)
		{
		}

		[HttpGet("{playerId}")]
		public async Task<IActionResult> Discipline(long playerId)
		{
			var db = PDBSM.PersonalDBContext(playerId);
			var rec = await db.Disciplines.Where(r => r.playerId == playerId).FirstOrDefaultAsync();

			if (rec == null || rec.level == evolib.Discipline.Level.None)
			{
				return Ok(new DisciplineInfo
				{
					playerId = playerId,
					level = evolib.Discipline.Level.None,
					title = "",
					text = "",
					expirationDate = DateTime.MinValue,
				});
			}

			return Ok(new DisciplineInfo
			{
				playerId = rec.playerId,
				level = rec.level,
				title = rec.title,
				text = rec.text,
				expirationDate = rec.expirationDate,
			});
		}

		[HttpPost]
		public async Task<IActionResult> Discipline([FromBody]Discipline.Request req)
		{
			var db = PDBSM.PersonalDBContext(req.playerId);
			var rec = await db.Disciplines.Where(r => r.playerId == req.playerId).FirstOrDefaultAsync();
			if (rec == null )
			{
				rec = new evolib.Databases.personal.Discipline
				{
					playerId = req.playerId,
				};
				await db.Disciplines.AddAsync(rec);
			}

			rec.level = req.level;
			rec.title = req.title ?? "";
			rec.text = req.text ?? "";
			rec.expirationDate = req.expirationDate;
			await db.SaveChangesAsync();

			if (evolib.Discipline.Level.Ban <= rec.level)
			{
				var player = new evolib.Kvs.Models.Player(req.playerId);
				if (await player.FetchAsync())
				{
					var session = new evolib.Kvs.Models.Session(player.Model.sessionId);
					session.Model.banned = true;
					await session.SaveAsync(TimeSpan.FromSeconds(60));
				}
			}

			if(evolib.Discipline.Level.None != rec.level)
			{
				Logger.Logging(new LogObj().AddChild(new LogModels.Discipline
				{
					playerId = rec.playerId,
					level = rec.level,
					title = rec.title,
					text = rec.text,
					expirationDate = rec.expirationDate,
				}));
			}

			return Ok(new Discipline.Response
			{
				playerId = rec.playerId,
				level = rec.level,
				title = rec.title,
				text = rec.text,
				expirationDate = rec.expirationDate,
			});
		}


		[HttpGet]
		public async Task<IActionResult> LoginReject()
		{
			var all = await Common1DB.LoginRejects.ToListAsync();
			return Ok(all);
		}

		[HttpPost]
		public async Task<IActionResult> LoginReject([FromBody]LoginReject.Request req)
		{
			var res = new LoginReject.Response
			{
				reset = req.reset.Value,
				list = new List<LoginReject.Info>(),
			};

			foreach( var i in req.list)
			{
				var rec = await Common1DB.LoginRejects
					.Where(r => r.target == i.target && r.value == i.value).FirstOrDefaultAsync();
				if(rec == null && !req.reset.Value)
				{
					await Common1DB.LoginRejects.AddAsync(new evolib.Databases.common1.LoginReject
					{
						target = i.target,
						value = i.value,
					});
					res.list.Add(i);
				}
				else if(rec != null && req.reset.Value)
				{
					Common1DB.Remove(rec);
					res.list.Add(i);
				}
			}

			await Common1DB.SaveChangesAsync();

			return Ok(res);
		}
	}
}
