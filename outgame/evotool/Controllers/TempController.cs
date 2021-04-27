using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using evolib.Services.MasterData;


namespace evotool.Controllers
{
	[Route("api/[controller]/[action]")]
	public class TempController : BaseController
	{
		public TempController(
			Services.IServicePack servicePack
		) : base(servicePack)
		{
		}

		[HttpGet]
		public async Task<IActionResult> LimitPackageVersion()
		{
			var all = await Common1DB.EnabledVersions.ToListAsync();

			var a = all.Select(r => new
			{
				target = r.checkTarget.ToString(),
				r.major,
				r.minor,
				r.patch,
				r.build,
			});

			return Ok(a);
		}



		[HttpGet("{target}/{major}/{minor}/{patch}/{build}")]
		public async Task<IActionResult> LimitPackageVersion(string target, int major, int minor, int patch, int build)
		{
			var notFound = true;
			var checkTarget = evolib.VersionChecker.CheckTarget.Login;

			foreach (evolib.VersionChecker.CheckTarget t in Enum.GetValues(typeof(evolib.VersionChecker.CheckTarget)))
			{
				if (t.ToString() == target)
				{
					checkTarget = t;
					notFound = false;
					break;
				}
			}

			if (notFound)
			{
				return BadRequest();
			}


			var ver = await Common1DB.EnabledVersions.FindAsync(
										checkTarget,
										evolib.VersionChecker.ReferenceSrc.PackageVersion);
			if (ver == null)
			{
				var tmp = await Common1DB.EnabledVersions.AddAsync(new evolib.Databases.common1.EnabledVersion
				{
					checkTarget = checkTarget,
					referenceSrc = evolib.VersionChecker.ReferenceSrc.PackageVersion,
				});
				ver = tmp.Entity;
			}

			ver.major = major;
			ver.minor = minor;
			ver.patch = patch;
			ver.build = build;

			await Common1DB.SaveChangesAsync();

			return Ok("Succeeded!");
		}


		[HttpGet("{itemid}/{disabled}")]
		public async Task<IActionResult> DisabledMobileSuit(string itemId, bool disabled)
		{
			await MasterDataLoader.LoadAsync();
			var masterData = MasterDataLoader.LatestMasterData;

			var item = masterData.GetItemFromItemId(itemId);
			if (item == null
				|| item.itemType != evolib.Item.Type.MobileSuit)
			{
				return BadRequest();
			}

			var record = await Common2DB.DisabledMobileSuits
				.Where(r => r.itemId == itemId)
				.FirstOrDefaultAsync();

			if (disabled)
			{
				if (record == null)
				{
					await Common2DB.DisabledMobileSuits.AddAsync(
						new evolib.Databases.common2.DisabledMobileSuit
						{
							itemId = itemId,
						}
					);
					await Common2DB.SaveChangesAsync();
				}
			}
			else
			{
				if (record != null)
				{
					Common2DB.DisabledMobileSuits.Remove(record);
					await Common2DB.SaveChangesAsync();
				}
			}

			return Ok(new { itemId, disabled });
		}

		[HttpGet]
		public async Task<IActionResult> GetDisabledMobileSuitsList()
		{
			var res = "itemId(availabled): disabled\n";
			res += "--------\n";


			var records = await Common2DB.DisabledMobileSuits.ToListAsync();

			await MasterDataLoader.LoadAsync();
			var masterData = MasterDataLoader.LatestMasterData;

			foreach (var msId in masterData.AllMobileSuitIds)
			{
				var ms = masterData.GetMobileSuit(msId);

				res += $"{ms.itemId}({ms.availabled}): {records.Find(r => r.itemId == ms.itemId) != null}\n";
			}

			return Ok(res);
		}

		[HttpGet]
		public async Task<IActionResult> GetCurrentSessionCnt()
		{
			var res1 = "";
			var res2 = "";
			var res3 = "";

			var loginTotal = 0;
			var entriedPlayerTotal = 0;
			var standbyBattleServerTotal = 0;



			foreach (evolib.MatchingArea area in Enum.GetValues(typeof(evolib.MatchingArea)))
			{
				if (area == evolib.MatchingArea.Unknown) continue;

				var currentSessionCount = new evolib.Kvs.Models.CurrentSessionCount(area);
				if( await currentSessionCount.FetchAsync())
				{
					res1 += $"  {area.ToString()}:{currentSessionCount.Model.count} {currentSessionCount.Model.breakDown}\n";

					res2 += $"  {area.ToString()}:{currentSessionCount.Model.entriedPlayersCnt}\n";

					res3 += $"  {area.ToString()}:{currentSessionCount.Model.enabledMatchesCnt}\n";

					loginTotal += currentSessionCount.Model.count;
					entriedPlayerTotal += currentSessionCount.Model.entriedPlayersCnt;
					standbyBattleServerTotal += currentSessionCount.Model.enabledMatchesCnt;
				}
			}

			var res = "";
			res += "-- Login Player Count --\n";
			res += $"Total: {loginTotal}\n";
			res += res1;

			res += $"\n";
			res += "-- Entried Player Count --\n";
			res += $"Total: {entriedPlayerTotal}\n";
			res += res2;

			res += $"\n";
			res += "-- Standby BattleServer Count --\n";
			res += $"Total: {standbyBattleServerTotal}\n";
			res += res3;

			return Ok(res);
		}
	}
}
