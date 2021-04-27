using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using evolib.Databases.personal;
using evolib.Kvs.Models;
using evolib.Kvs.Models.ConnectionQueueData;
using evolib.Services;

namespace evolib
{
	public static class ChallengeList
	{
		public class CountupInfo
		{
			public string id { get; set; }

			public int value { get; set; }
		}

		static async Task<List<Databases.personal.Challenge>> UpdateAsync(
			long playerId,
			Services.MasterData.IMasterData masterdata,
			PersonalDBContext db)
		{
			var changed = false;

			var list = await db.Challenges.Where(r => r.playerId == playerId).ToListAsync();

			var now = DateTime.UtcNow;

			for(int i=0; i<list.Count; )
			{
				var m = list[i];

				if (m.expirationDate <= now )
				{
					db.Challenges.Remove(m);
					list.RemoveAt(i);
					changed = true;

					continue;
				}

				i++;
			}

			var addedList = new List<Databases.personal.Challenge>();
			if(list.Find(r => r.type == Challenge.Type.Beginner) == null)
			{
				var expirationDate = GetExpirationDate(masterdata, Challenge.Type.Beginner, now);

				var allBeginners = masterdata.AllChallenge.Where(challenge => challenge.type == evolib.Challenge.Type.Beginner).ToList();

				var firstOrder = 0;
				if (0 < masterdata.AllBeginnerChallengeSheet.Count)
				{
					firstOrder = masterdata.AllBeginnerChallengeSheet.Min(i => i.order);
				}

				foreach (var info in allBeginners)
				{
					var sheetMaster = masterdata.GetBeginnerChallengeSheet(info.sheetId);

					var r = new Databases.personal.Challenge
					{
						playerId = playerId,
						challengeId = info.challengeId,
						type = evolib.Challenge.Type.Beginner,
						order = sheetMaster.order,
						expirationDate = expirationDate,
						unlocked = firstOrder == sheetMaster.order ? true : false,
					};
					addedList.Add(r);
				}
				changed = true;
			}
			if (list.Find(r => r.type == Challenge.Type.Weekly) == null)
			{
				var allWeeklies = masterdata.AllChallenge.Where(challenge => challenge.type == evolib.Challenge.Type.Weekly).ToList();

				// TODO: 抽選ロジックは仮
				var indexList = Challenge.DrawLots(allWeeklies.Count, 3);

				var weeklyDrawLots = allWeeklies.Where((challenge, index) => indexList.Contains(index)).ToList();

				var expirationDate = GetExpirationDate(masterdata, Challenge.Type.Weekly, now);

				// TODO: 有償ならロックを掛ける
				var unlocked = true;

				foreach (var info in weeklyDrawLots)
				{
					var r = new Databases.personal.Challenge
					{
						playerId = playerId,
						challengeId = info.challengeId,
						type = evolib.Challenge.Type.Weekly,
						order = 0,
						expirationDate = expirationDate,
						unlocked = unlocked,
					};
					addedList.Add(r);
				}
				changed = true;
			}
			if (list.Find(r => r.type == Challenge.Type.Daily) == null)
			{
				var allDailies = masterdata.AllChallenge.Where(challenge => challenge.type == evolib.Challenge.Type.Daily).ToList();

				// TODO: 抽選ロジックは仮
				var indexList = Challenge.DrawLots(allDailies.Count, 3);

				var dailyDrawLots = allDailies.Where((challenge, index) => indexList.Contains(index)).ToList();

				var expirationDate = GetExpirationDate(masterdata, Challenge.Type.Daily, now);

				// TODO: 有償ならロックを掛ける
				var unlocked = true;

				foreach (var info in dailyDrawLots)
				{
					var r = new Databases.personal.Challenge
					{
						playerId = playerId,
						challengeId = info.challengeId,
						type = evolib.Challenge.Type.Daily,
						order = 0,
						expirationDate = expirationDate,
						unlocked = unlocked,
					};
					addedList.Add(r);
				}
				changed = true;
			}

			if (changed)
			{
				await db.Challenges.AddRangeAsync(addedList);
				await db.SaveChangesAsync();
			}

			list.AddRange(addedList);

			return list;

		}

		public static async Task<List<Databases.personal.Challenge>> GetAsync(
			long playerId,
			Services.MasterData.IMasterData masterdata,
			PersonalDBContext db)
		{
			return await UpdateAsync(playerId, masterdata, db);
		}

		public static async Task CountupAsync(
			long playerId,
			Services.MasterData.IMasterData masterdata,
			PersonalDBContext db,
			evolib.Challenge.OutgameServerChallengeType challengeType,
			string sessionId)
		{
			if (challengeType == evolib.Challenge.OutgameServerChallengeType.None) return;

			// 指定するカテゴリのチャレンジをリストアップする
			var countupInfos = masterdata.AllChallenge
				.Where(master =>
				   master.reporterType == evolib.Challenge.ReporterType.OutgameServer &&
				   master.outgameServerChallengeType == challengeType)
				.Select(master =>
					new CountupInfo()
					{
						id = master.challengeId,
						value = 1,
					})
					.ToList();

			await CountupAsync(playerId, masterdata, db, countupInfos, sessionId);
		}

		public static async Task CountupAsync(
			long playerId,
			Services.MasterData.IMasterData masterdata,
			PersonalDBContext db,
			List<CountupInfo> countupInfos,
			string sessionId)
		{
			var challengeList = await UpdateAsync(playerId, masterdata, db);
			var clearList = new List<string>();

			var allBeginnerMaster = masterdata.AllChallenge.Where(challenge => challenge.type == evolib.Challenge.Type.Beginner).ToList();

			foreach (var countupInfo in countupInfos)
			{
				var challenge = challengeList.Find(r => r.challengeId == countupInfo.id);

				if (challenge == null) continue;

				if (challenge.status == Challenge.Status.Clear) continue;

				if (!challenge.unlocked) continue;

				var challengeMaster = masterdata.GetChallenge(challenge.challengeId);

				if (challengeMaster == null) continue;

				challenge.value += countupInfo.value;

				if (challengeMaster.requiredNum <= challenge.value)
				{
					challenge.status = Challenge.Status.Clear;

					// 最大値を超えないように設定
					challenge.value = Math.Min(challenge.value, challengeMaster.requiredNum);

					//報酬
					//db.PresentBoxs.Add(new Databases.personal.PresentBox
					//{

					//});

					// ビギナーチャレンジのシート達成確認
					if (challenge.type == Challenge.Type.Beginner)
					{
						var notClearList = challengeList.FindAll(r => r.type == evolib.Challenge.Type.Beginner && r.status != evolib.Challenge.Status.Clear);

						if (notClearList != null)
						{
							notClearList.Sort((a,b) => a.order - b.order);
							var nextOrder = notClearList.First().order;

							// シートが次のステップに変わったなら、シート達成
							if (challenge.order != nextOrder)
							{
								//　次のシートのロックを解除する
								var unlockedList = notClearList.Where(r => r.order == nextOrder)
												.Select(r =>
												{
													r.unlocked = true;
													return r;
												})
												.ToList();
							}
						}
					}

					// アウトゲームでカウントする項目のみ通知 = ビギナーチャレンジ の為、
					// ビギナーチャレンジの時には通知する
					if (challenge.type == Challenge.Type.Beginner)
					{
						clearList.Add(countupInfo.id);
					}
				}
			}

			await db.SaveChangesAsync();

			if(0 <= clearList.Count)
			{
				await new ConnectionQueue(sessionId).EnqueueAsync(new ClearChallenge
				{
					challengeIds = clearList,
				});
			}
		}

		public static DateTime GetExpirationDate(Services.MasterData.IMasterData masterdata, evolib.Challenge.Type type, DateTime today)
		{
			var expirationMaster = masterdata.GetChallengeExpiration(type);

			if (expirationMaster == null) return DateTime.MaxValue;

			var finishDate = new DateTime(today.Year, today.Month, today.Day);

			switch (type)
			{
				case Challenge.Type.Beginner:
					finishDate = DateTime.MaxValue;
					break;

				case Challenge.Type.Daily:
					{
						finishDate += expirationMaster.expirationOffsetTime;

						//終了日より未来なら、終了日に次の日を指定する
						if (finishDate <= today)
						{
							finishDate = finishDate.AddHours(24);
						}
					}
					break;

				case Challenge.Type.Weekly:
					{
						// 終了曜日算出の為、一旦日曜日に設定する
						var sundayAddDays = (int)System.DayOfWeek.Sunday - (int)finishDate.DayOfWeek;
						finishDate = finishDate.AddDays(sundayAddDays);

						finishDate += expirationMaster.expirationOffsetTime;

						//終了曜日より未来なら、終了日に次週を指定する
						if (finishDate <= today)
						{
							finishDate = finishDate.AddHours(7*24);
						}
					}
					break;
			}

			return finishDate;
		}
	}
}
