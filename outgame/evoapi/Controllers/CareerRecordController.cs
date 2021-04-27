using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using evolib;
using evolib.Kvs.Models;
using evoapi.ProtocolModels;
using evoapi.ProtocolModels.CareerRecord;

namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class CareerRecordController : BaseController
	{
		public CareerRecordController(
			Services.IServicePack servicePack
		) : base(servicePack) { }

		[HttpPost]
		public async Task<IActionResult> GetSelf([FromBody]GetSelf.Request req)
		{
			var res = new GetSelf.Response();

			var playerId = SelfHost.playerInfo.playerId;
			var db = PDBSM.PersonalDBContext(playerId);

			// カジュアルマッチの戦績取得
			{
				int seasonNoOfCasualMatch = 0;
				var casualMatchRecords = await db.CareerRecords
					.Where(x =>
					x.playerId == playerId &&
					x.matchType == evolib.Battle.MatchType.Casual &&
					x.seasonNo == seasonNoOfCasualMatch)
					.ToListAsync();

				res.casual = createRecordInfoList(casualMatchRecords);
			}

			// ランクマッチの戦績取得
			{
				evolib.Services.MasterData.ISeason currentSeason = MasterData.GetCurrentSeason();

				//現在のシーズンが取得出来なかったなら、ランクマッチの戦績リストを空で終了する
				if (currentSeason == null)
				{
					res.rank = new List<RecordInfo>();
				}
				else
				{
					int seasonNoOfRankMatch = currentSeason.seasonNo;

					var rankMatchRecords = await db.CareerRecords
						.Where(x =>
						x.playerId == playerId &&
						x.matchType == evolib.Battle.MatchType.Rank &&
						x.seasonNo == seasonNoOfRankMatch)
						.ToListAsync();

					res.rank = createRecordInfoList(rankMatchRecords);
				}
			}

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> GetSocial([FromBody]GetSocial.Request req)
		{
			var player = new Player(SelfHost.playerInfo.playerId);
			if (!await player.Validate(PDBSM))
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}

			var res = new GetSocial.Response();
			res.myTierRecordInfo = new GetSocial.Response.SocialRecordInfo
			{
				casualMatchRecordList = new List<RecordInfo>(),
				thisSeasonRankMatchRecordList = new List<RecordInfo>(),
			};
			res.top50RecordInfo = new GetSocial.Response.SocialRecordInfo
			{
				casualMatchRecordList = new List<RecordInfo>(),
				thisSeasonRankMatchRecordList = new List<RecordInfo>(),
			};
			res.allRecordInfo = new GetSocial.Response.SocialRecordInfo
			{
				casualMatchRecordList = new List<RecordInfo>(),
				thisSeasonRankMatchRecordList = new List<RecordInfo>(),
			};

			return Ok(res);
		}

		public async Task<IActionResult> GetPastUrl([FromBody]GetPastUrl.Request req)
		{
			var res = new GetPastUrl.Response();
			res.records = new List<GetPastUrl.Response.RecordUrl>();

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> Save([FromBody]Save.Request req)
		{
			var res = new Save.Response();

			evolib.Services.MasterData.ISeason currentSeason = MasterData.GetCurrentSeason();

			// プレイヤー毎
			foreach (var registerSaveData in req.playerRecordInfoList)
			{
				await SaveCareerRecordOfPlayer(req, registerSaveData, currentSeason);
			}

			return Ok(res);
		}

		private async Task SaveCareerRecordOfPlayer(Save.Request req, Save.Request.PlayerRecordInfo registerSaveData, evolib.Services.MasterData.ISeason currentSeason)
		{
			//ランクマッチ時に現在のシーズンが取得出来なかったなら、処理を終了する
			//(カジュアルマッチ時は処理が行える為、呼び出し側ではなく内部でチェックする)
			if (req.matchType == evolib.Battle.MatchType.Rank && currentSeason == null)
			{
				return;
			}

			var playerId = registerSaveData.playerId;
			var db = PDBSM.PersonalDBContext(playerId);

			int seasonNo = req.matchType == evolib.Battle.MatchType.Rank ? currentSeason.seasonNo : 0;

			var query = db.CareerRecords.Where(
				x => x.playerId == playerId &&
				x.matchType == req.matchType &&
				x.seasonNo == seasonNo);
			var records = await query.ToListAsync();

			foreach (var info in registerSaveData.recordList)
			{
				var careerRecord = MasterData.GetCareerRecord(info.id);

				if (careerRecord == null)
				{
					continue;
				}

				foreach (var blockInfo in info.blocks)
				{
					var blockRecord =  new evolib.Databases.personal.CareerRecord();
					blockRecord.playerId = playerId;
					blockRecord.recordItemId = info.id;
					blockRecord.matchType = req.matchType;
					blockRecord.seasonNo = seasonNo;
					blockRecord.mobileSuitId = blockInfo.ms;
					blockRecord.value = double.Parse(blockInfo.val);
					blockRecord.numForAverage = blockInfo.num;

					// CareerRecordの1項目を特定
					var recordData = records.Find(i =>
						i.recordItemId == info.id &&
						i.mobileSuitId == blockInfo.ms);

					if (recordData == null)
					{
						await db.CareerRecords.AddAsync(blockRecord);
					}
					else
					{
						switch (careerRecord.valueType)
						{
							// 累積する場合
							case evolib.CareerRecord.ValueType.Sum:
							case evolib.CareerRecord.ValueType.SumAverage:
							{
								recordData.value += blockRecord.value;
								recordData.numForAverage += blockRecord.numForAverage;
							}
							break;

							// 値が高い方を更新する場合
							default:
							{
								// 平均値の場合
								if (careerRecord.valueType == evolib.CareerRecord.ValueType.MaxAverage)
								{
									var averageValue = blockRecord.numForAverage == 0 ? blockRecord.value : blockRecord.value / blockRecord.numForAverage;
									var recordAverageValue = recordData.numForAverage == 0 ? recordData.value : recordData.value / recordData.numForAverage;
									if (recordAverageValue < averageValue)
									{
										recordData.value = blockRecord.value;
										recordData.numForAverage = blockRecord.numForAverage;
									}
								}
								// 平均値以外
								else
								{
									if (recordData.value < blockRecord.value)
									{
										recordData.value = blockRecord.value;
										recordData.numForAverage = blockRecord.numForAverage;
									}
								}
							}
							break;
						}

						// 最大値を超えないように設定
						recordData.value = Math.Min(recordData.value, evolib.CareerRecord.RECORD_MAX_VALUE);
						recordData.numForAverage = Math.Min(recordData.numForAverage, evolib.CareerRecord.RECORD_MAX_VALUE);
					}
				}
			}

			await db.SaveChangesAsync();
		}

		private List<RecordInfo> createRecordInfoList(List<evolib.Databases.personal.CareerRecord> careerRecords)
		{
			var recordInfoList = new Dictionary<string, List<RecordBlockInfo>>();

			// レコードをレスポンス用にする為に一旦Mapにまとめる
			foreach (var careerRecord in careerRecords)
			{
				if (!recordInfoList.ContainsKey(careerRecord.recordItemId))
				{
					var recordBlockList = new List<RecordBlockInfo>();
					recordInfoList.Add(careerRecord.recordItemId, recordBlockList);
				}

				var careerRecordMaster = MasterData.GetCareerRecord(careerRecord.recordItemId);

				if (careerRecordMaster == null)
				{
					continue;
				}

				// カテゴリが機体固有の特殊カテゴリで、機体が異なるなら、レスポンスにまとめる必要がないので次に進める
				if (careerRecordMaster.categoryType == evolib.CareerRecord.CategoryType.Special &&
					careerRecordMaster.mobileSuitId != careerRecord.mobileSuitId)
				{
					continue;
				}
				// カテゴリが機体固有の特殊カテゴリで、既にレコードが設定されているなら、除外する（通常、１戦績項目で、同機体の複数レコードは存在しない）
				else if (careerRecordMaster.categoryType == evolib.CareerRecord.CategoryType.Special &&
					1 <= recordInfoList[careerRecord.recordItemId].Count)
				{
					continue;
				}

				var recordBlockInfo = new RecordBlockInfo
				{
					ms = careerRecord.mobileSuitId,
					val = careerRecord.value.ToString(),
					num = careerRecord.numForAverage
				};

				recordInfoList[careerRecord.recordItemId].Add(recordBlockInfo);
			}

			var recordList = new List<RecordInfo>();

			// Mapにまとめた情報をレスポンス用のレコードリストに設定する
			foreach (var recordItemId in recordInfoList.Keys)
			{
				recordList.Add(new RecordInfo
				{
					id = recordItemId,
					blocks = recordInfoList[recordItemId],
				});
			}

			return recordList;
		}

#region TODO:以下の処理は仮データ用意用の処理

		private void getDebugValueByTierType(
			PlayerInformation.BattleRatingTierType tierType,
			out float value, out int numForAverage)
		{
			switch (tierType)
			{
				case PlayerInformation.BattleRatingTierType.Unrank:
					value = 200.0f;
					numForAverage = 3;
					break;

				case PlayerInformation.BattleRatingTierType.Bronze:
					value = 500.0f;
					numForAverage = 6;
					break;

				case PlayerInformation.BattleRatingTierType.Silver:
					value = 800.0f;
					numForAverage = 9;
					break;

				case PlayerInformation.BattleRatingTierType.Gold:
					value = 1100.0f;
					numForAverage = 12;
					break;

				case PlayerInformation.BattleRatingTierType.Platinum:
					value = 1400.0f;
					numForAverage = 15;
					break;

				default:
					value = 1700.0f;
					numForAverage = 18;
					break;
			}
		}

		private List<RecordInfo> createDebugRecordInfoList(
			float value, int numForAverage)
		{
			var recordList = new List<RecordInfo>();

			var allCareerRecords = MasterData.AllCareerRecords;
			foreach (var careerRecord in allCareerRecords)
			{
				recordList.Add(new RecordInfo
				{
					id = careerRecord.recordItemId,
					blocks = createDebugRecordBlockInfoList(careerRecord, value, numForAverage),
				});
			}

			return recordList;
		}

		private List<RecordBlockInfo> createDebugRecordBlockInfoList(
			evolib.Services.MasterData.ICareerRecord careerRecord,
			float value, int numForAverage)
		{
			var recordBlockList = new List<RecordBlockInfo>();

			foreach (var msId in MasterData.AllMobileSuitIds)
			{
				recordBlockList.Add(createDebugRecordBlockInfo(careerRecord, msId, value, numForAverage));

				// カテゴリがユニット共通ではないなら、機体は１機体だけのデータだけで良い
				if (careerRecord.categoryType != evolib.CareerRecord.CategoryType.General)
				{
					break;
				}
			}

			return recordBlockList;
		}

		private RecordBlockInfo createDebugRecordBlockInfo(
			evolib.Services.MasterData.ICareerRecord careerRecord, string mobileSuitId,
			float value, int numForAverage)
		{
			return (new RecordBlockInfo
			{
				ms = mobileSuitId,
				val = value.ToString(),
				num = numForAverage
			});
		}

		private GetSocial.Response.SocialRecordInfo createDebugSocialRecordInfo(
			float value, int numForAverage)
		{
			return new GetSocial.Response.SocialRecordInfo
			{
				casualMatchRecordList = createDebugRecordInfoList(value, numForAverage),
				thisSeasonRankMatchRecordList = createDebugRecordInfoList(value, numForAverage),
			};
		}
#endregion

	}
}
