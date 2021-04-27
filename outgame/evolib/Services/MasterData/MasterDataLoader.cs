using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Amazon.S3;
using Amazon.S3.Model;


using evolib.Kvs.Models;
using evolib.Log;
using evolib.DeliveryData;
using evolib.Databases.common2;

namespace evolib.Services.MasterData
{
	public static class MasterDataLoader
	{
		public static IMasterData LatestMasterData { get; private set; }

		static DateTime LatestMasterDataUpdateDate { get; set; }


		static string RowStructName(JToken tkn) { return (string)tkn["RowStructName"]; }
		static string PathName(JToken tkn) { return (string)tkn["PathName"]; }

		public static void MasterDataPathRecord(this GenericData rec,
			string srcPath,
			string plainS3Key,
			string plainPath,
			string encryptPath,
			string version )
		{
			rec.type = GenericData.Type.MasterDataPath;
			rec.data1 = srcPath;
			rec.data2 = plainS3Key;
			rec.data3 = plainPath;
			rec.data4 = encryptPath;
			rec.data5 = version;
		}

		class JsonSrcRequester : Util.HttpRequester
		{
			public override string Path { get { return ""; } }

			static HttpClient _httpClient = new HttpClient();
			protected override HttpClient HttpClient { get { return _httpClient; } }
		}

		public static async Task LoadAsync()
		{
			var masterDataPath = new MasterDataPath();
			if (false == await masterDataPath.FetchAsync())
			{
				Logger.Logging(new LogObj().AddChild(new LogModels.ErrorReport
				{
					Msg = "Not found MasterDataPath@Kvs!",
				}));
				return;
			}

			if ((LatestMasterData != null && LatestMasterData.VersionStr == masterDataPath.Model.version)
				&& masterDataPath.Model.updateDate == LatestMasterDataUpdateDate)
			{// Already Loaded
				return;
			}

			var path = "";
			var rawJson = "";

			if (!string.IsNullOrEmpty(masterDataPath.Model.s3KeyPlain))
			{
				try
				{
					path = $"{DeliveryDataInfo.S3BucketName}/{masterDataPath.Model.s3KeyPlain}";
					Logger.Logging(new LogObj().AddChild(new LogModels.MasterDataStartLoading
					{
						Path = path,
						UpdateDate = masterDataPath.Model.updateDate,
						Date = DateTime.UtcNow,
					}));


					var s3Client = new AmazonS3Client(
						Amazon.RegionEndpoint.GetBySystemName(DeliveryDataInfo.S3BucketRegion)
					);

					var getReq = new GetObjectRequest
					{
						BucketName = DeliveryDataInfo.S3BucketName,

						Key = masterDataPath.Model.s3KeyPlain,
					};

					using (var getRes = await s3Client.GetObjectAsync(getReq))
					{
						using (var reader = new System.IO.StreamReader(getRes.ResponseStream))
						{
							rawJson = await reader.ReadToEndAsync();
						}
					}
				}
				catch (Exception ex)
				{
					return;
				}
			}
			else
			{
				path = $"{masterDataPath.Model.pathPlain}";
				Logger.Logging(new LogObj().AddChild(new LogModels.MasterDataStartLoading
				{
					Path = path,
					UpdateDate = masterDataPath.Model.updateDate,
					Date = DateTime.UtcNow,
				}));

				var requester = new JsonSrcRequester();
				var response = await requester.GetAsync(masterDataPath.Model.pathPlain);
				if (response.StatusCode != System.Net.HttpStatusCode.OK)
				{
					Logger.Logging(new LogObj().AddChild(new LogModels.ErrorReport
					{
						Msg = $"Not found JsonFile at \"{masterDataPath.Model.pathPlain}\".",
					}));
					return;
				}

				rawJson = response.Payload;
			}

			var masterData = await ParseAsync(
				rawJson, masterDataPath.Model.pathEncrypt, masterDataPath.Model.pathPlain);

			LatestMasterData = masterData;
			LatestMasterDataUpdateDate = masterDataPath.Model.updateDate;

			Logger.Logging(new LogObj().AddChild(new LogModels.MasterDataEndLoading
			{
				Path = path,
				UpdateDate = masterDataPath.Model.updateDate,
				Date = DateTime.UtcNow,
			}));
		}

		public static async Task<IMasterData> ParseAsync(string json, string downLoadPath, string secretePath)
		{
			var masterData = new MasterData()
			{
				DownloadPath = downLoadPath,
				SecretePath = secretePath,
			};

			var src = JsonConvert.DeserializeObject<JToken>(json);

			////////////////////////////////////////////////////////////////////
			// MasterDataVersion
			////////////////////////////////////////////////////////////////////
			masterData.Version = new int[] { 0, 0, 0 };
			if (src["Version"] != null)
			{
				var version = new List<int>();
				foreach (var row in src["Version"])
				{
					version.Add((int)row);
				}
				if (0 < version.Count) masterData.Version[0] = version[0];
				if (1 < version.Count) masterData.Version[1] = version[1];
				if (2 < version.Count) masterData.Version[2] = version[2];
			}


			var root = src["Root"];
			var children = src["Children"];

			Func<string, JObject> SearchDataTable = (tableKey) =>
			{
				if (tableKey == null) return null;
				if (tableKey == "None") return null;

				var Prefix = "DataTable'";
				if (!tableKey.StartsWith(Prefix)) return null;
				tableKey = tableKey.Substring(Prefix.Length).Trim('\'');

				var ret = (JObject)children[tableKey];
				if (ret != null)
				{
					return ret;
				}

				foreach (var table in root)
				{
					if (PathName(table) == tableKey)
					{
						return (JObject)table;
					}
				}

				return null;
			};

			var commonChoicesDataTableKeys = new Dictionary<string, int>();
			var voicePackDataTables = new Dictionary<string, List<string>>();

			foreach (var table in root)
			{
				if ("EvoItemDataTableRow" != RowStructName(table)) continue;

				foreach (var row in table["Rows"])
				{
					var tableKey = (string)row["AssetDataTable"];
					var sourceDataTable = SearchDataTable(tableKey);
					if (sourceDataTable == null)
					{
						continue;
						//throw new Exception("Not exist AssetDataTable :" + row["Name"]);
					}

					var itemId = (string)row["Name"];

					//var displayNameSeparated = row["DisplayName"].ToString().Split("\"");
					//var displayName = (5 < displayNameSeparated.Length) ? displayNameSeparated[5] : "no name";
					var displayName = itemId;

					var isDefaultSetting = (bool)row["bIsDefault"];

					evolib.Item.RankType rankType;
					Enum.TryParse((string)row["Rank"], out rankType);

					var itemType = Item.ItemTypeFromRowStructName(RowStructName(sourceDataTable));

					if (evolib.Item.Type.Ornament == itemType ||
						evolib.Item.Type.Stamp == itemType ||
						evolib.Item.Type.VoicePack == itemType)
					{
						commonChoicesDataTableKeys[tableKey] = 12345;
					}
					if (evolib.Item.Type.VoicePack == itemType)
					{
						voicePackDataTables[itemId]
							= new List<string> { tableKey, (string)row["AssetRowName"] };
					}

					masterData.AddItem(
						new Item()
						{
							itemId = itemId,
							displayName = displayName,
							itemType = itemType,
							rankType = rankType,
							isDefaultSetting = isDefaultSetting,
						},
						sourceDataTable.Path,
						(string)row["AssetRowName"]
					);

					if (isDefaultSetting)
					{
						masterData.AddDefaultOwnedItem(itemId);
					}
				}
				break;
			}

			foreach (var table in voicePackDataTables)
			{
				var voicePack = new VoicePack(table.Key);

				var voicePackDataTable = SearchDataTable(table.Value[0]);
				foreach (var row in voicePackDataTable["Rows"])
				{
					if ((string)row["Name"] != table.Value[1]) continue;

					var customizeVoiceDataTable = SearchDataTable((string)row["CustomizeVoiceDataTable"]);
					if (customizeVoiceDataTable == null)
					{
						throw new Exception("Not exist CustomizeVoiceDataTable :" + row["Name"]);
					}

					foreach (var sRow in customizeVoiceDataTable["Rows"])
					{
						voicePack.addVoice(
							(string)sRow["Name"],
							(bool)sRow["bIsDefault"],
							(bool)sRow["bIsEmpty"]
						);
					}

					break;
				}

				masterData.AddVoicePack(voicePack);
			}

			foreach (var table in src["Root"])
			{
				if ("EvoCharacterRow" != RowStructName(table)) continue;

				foreach (var row in table["Rows"])
				{
					var msItem = masterData.GetItemFromAssetDataPath(table.Path, (string)row["Name"]);
					if (null == msItem)
					{
						continue;
					}

					var mobileSuit = new MobileSuit()
					{
						mobileSuitId = (string)row["Name"],
						itemId = msItem.itemId,
						availabled = (bool)row["bIsAvailable"],
					};

					Action<string> AddChoices = (tableKey) =>
					{
						var sourceDataTable = SearchDataTable(tableKey);
						if (sourceDataTable == null) return;

						foreach (var sRow in sourceDataTable["Rows"])
						{
							var item = masterData.GetItemFromAssetDataPath(
									sourceDataTable.Path, (string)sRow["Name"]);
							if (item == null) continue;

							var isEmpty = (bool)sRow["bIsEmpty"];
							mobileSuit.AddChoices(
								item.itemType,
								item.itemId,
								item.isDefaultSetting,
								isEmpty
							);
							if (isEmpty)
							{
								masterData.AddDefaultOwnedItem(item.itemId);
							}
						}
					};
					AddChoices((string)row["BodySkinDataTable"]);
					AddChoices((string)row["WeaponSkinDataTable"]);
					AddChoices((string)row["MVPDataTable"]);
					AddChoices((string)row["EmotionDataTable"]);
					foreach (var tableKey in commonChoicesDataTableKeys.Keys)
					{
						AddChoices(tableKey);
					}

					masterData.AddMobileSuit(mobileSuit);
				}
				break;
			}

			////////////////////////////////////////////////////////////////////
			// NgWord
			////////////////////////////////////////////////////////////////////
			Util.NotSensitiveCharsRegex.Init();

			foreach (var table in root)
			{
				if ("EvoDenyNameRow" != RowStructName(table)) continue;

				foreach (var row in table["Rows"])
				{
					if ((bool)row["bIsAvailable"])
					{
						var word = Util.NotSensitiveCharsRegex.Replace((string)row["DenyName"]);

						var type = (string)row["MatchType"];
						if (type == "partial")
						{// 部分一致
							masterData.AddNgWord("^(?=.*" + word + ").*$", true);
						}
						else if (type == "forward")
						{// 前方一致
							masterData.AddNgWord("^" + word + ".*$", true);
						}
						else if (type == "exact")
						{// 完全一致
							masterData.AddNgWord("^" + word + "$", true);
						}
					}
				}
				break;
			}
			foreach (var table in root)
			{
				if ("EvoAllowNameRow" != RowStructName(table)) continue;

				foreach (var row in table["Rows"])
				{
					if ((bool)row["bIsAvailable"])
					{
						var word = Util.NotSensitiveCharsRegex.Replace((string)row["AllowName"]);
						masterData.AddNgWord("^(?=.*" + word + ").*$", false);

					}
				}
				break;
			}


			////////////////////////////////////////////////////////////////////
			// CareerRecord
			////////////////////////////////////////////////////////////////////
			foreach (var table in root)
			{
				if ("EvoCareerRecordRow" != RowStructName(table)) continue;

				foreach (var row in table["Rows"])
				{
					var recordItemId = (string)row["Name"];

					evolib.CareerRecord.CategoryType categoryType;
					Enum.TryParse((string)row["CategoryType"], out categoryType);

					evolib.CareerRecord.ValueType valueType;
					Enum.TryParse((string)row["ValueType"], out valueType);

					evolib.CareerRecord.FormatType formatType;
					Enum.TryParse((string)row["FormatType"], out formatType);

					var mobileSuitId = (string)row["MobileSuitId"];

					var careerRecord = new CareerRecord(recordItemId, valueType, categoryType, formatType, mobileSuitId);
					masterData.AddCareerRecord(careerRecord);
				}
			}

			////////////////////////////////////////////////////////////////////
			// BattleRatingTier
			////////////////////////////////////////////////////////////////////
			foreach (var table in root)
			{
				if ("EvoBattleRatingTierRow" != RowStructName(table)) continue;

				foreach (var row in table["Rows"])
				{
					var battleRatingTierId = (string)row["Name"];

					PlayerInformation.BattleRatingTierType tierType;
					Enum.TryParse((string)row["TierType"], out tierType);

					var startRange = (int)row["StartRange"];
					var endRange = (int)row["EndRange"];
					var isUnrank = (bool)row["IsUnrank"];

					var battleRatingTier = new BattleRatingTier(battleRatingTierId, tierType, startRange, endRange, isUnrank);
					masterData.AddBattleRatingTier(battleRatingTier);
				}
			}

			////////////////////////////////////////////////////////////////////
			// BattlePass
			////////////////////////////////////////////////////////////////////
			foreach (var table in root)
			{
				// シーズン
				if ("EvoSeasonRow" == RowStructName(table))
				{
					foreach (var row in table["Rows"])
					{
						//var rowName = (int)row["Name"];
						var seasonNo = (int)row["SeasonNo"];
						var seasonName = (string)row["SeasonName"];

						// UE4 DataTableはDateTime型を使えないので時刻はStringで共有される
						var startDateStr = (string)row["StartDateUtc"];
						var endDateStr = (string)row["EndDateUtc"];
						DateTime startDate;
						DateTime endDate;
						if (!DateTime.TryParse(startDateStr, out startDate) || !DateTime.TryParse(endDateStr, out endDate))
						{
							throw new Exception("MasterData_Season DateTime Error :" + row["Name"]);
						}

						var addRow = new Season(seasonNo, seasonName, startDate, endDate);
						masterData.AddSeasonRow(addRow);
					}
				}

				// バトルパス
				else if ("EvoBattlePassRow" == RowStructName(table))
				{
					foreach (var row in table["Rows"])
					{
						var id = (int)row["Id"];
						evolib.BattlePass.PassType passType;
						Enum.TryParse((string)row["Type"], out passType);
						var seasonNo = (int)row["SeasonNo"];
						var maxLevel = (int)row["MaxLevel"];
						var itemId = (string)row["ItemId"];
						var expTableKey = (string)row["NeedExpDataTable"];//[DataTable'/Game/Blueprints/～']の形式
						var rewardTableKey = (string)row["RewardDataTable"];
						var useDate = (bool)row["IsUseDate"];
						var startDateStr = (string)row["StartDateUtc"];
						var endDateStr = (string)row["EndDateUtc"];
						DateTime startDate = DateTime.MinValue;
						DateTime endDate = DateTime.MinValue;
						if (useDate)
						{
							if (!DateTime.TryParse(startDateStr, out startDate) || !DateTime.TryParse(endDateStr, out endDate))
							{
								throw new Exception("MasterData_Season DateTime Error :" + row["Name"]);
							}
						}
						//
						var addRow = new BattlePass(id, passType, seasonNo, maxLevel, itemId, expTableKey, rewardTableKey, useDate, startDate, endDate);
						masterData.AddBattlePassRow(addRow);

						// exp setting
						if (!masterData.CheckExpSetting(expTableKey))
						{
							var expDataTable = SearchDataTable(expTableKey);
							if (expDataTable == null)
							{
								// これがNullなのはおかしい
								throw new Exception("Not exist NeedExpDataTable :" + row["Name"]);
							}
							var addExpTable = new PassNeedExp();
							foreach (var expRow in expDataTable["Rows"])
							{
								var info = new PassNeedExp.ExpInfo();
								info.level = (int)expRow["Name"];
								info.needExp = (int)expRow["NextExp"];
								info.levelCoefficient = (int)expRow["LevelCoefficient"];
								info.repeat = (bool)expRow["bRepeat"];
								//
								addExpTable.AddSetting(info.level, info);
							}
							masterData.AddPassNeedExpData(expTableKey, addExpTable);
						}

						// reward setting
						if (!masterData.CheckPassRewardSetting(rewardTableKey))
						{
							var rewardTable = SearchDataTable(rewardTableKey);
							// 報酬は無い場合もある
							if (rewardTable != null)
							{
								var addRewardTable = new PassReward();
								foreach (var rewardRow in rewardTable["Rows"])
								{
									var info = new PassReward.RewardInfo();
									info.level = (int)rewardRow["Level"];
									info.isPremium = (bool)rewardRow["bIsPremium"];
									info.itemId = (string)rewardRow["ItemId"];
									//
									addRewardTable.AddSetting(info);
								}
								masterData.AddPassRewardData(rewardTableKey, addRewardTable);
							}
						}
					}
				}

				{
					var tmp = new Databases.personal.BattlePass
					{
						playerId = 0,
						passId = (int)evolib.BattlePass.PlayerLevelPassId,
						totalExp = 0,
						isPremium = false,
						rewardLevel = 0,
					};
					tmp.SetLevelDetail(masterData);
					masterData.InitialPlayerLevel.level = tmp.level;
					masterData.InitialPlayerLevel.levelExp = tmp.levelExp;
					masterData.InitialPlayerLevel.nextExp = tmp.nextExp;
				}

			}//battle pass root
			masterData.CreateEnableBattlePass();

			////////////////////////////////////////////////////////////////////
            // Reward
            ////////////////////////////////////////////////////////////////////
            foreach (var table in root)
            {
                if ("EvoRewardRow" != RowStructName(table)) continue;

                foreach (var row in table["Rows"])
                {
                    evolib.Reward.GiveMethods inGiveMethods;
                    evolib.GiveAndTake.Type inRewardType;
                    if (Enum.TryParse((string)row["GiveMethods"], out inGiveMethods)
                        && Enum.IsDefined(typeof(evolib.Reward.GiveMethods), inGiveMethods)
                        && Enum.TryParse((string)row["RewardType"], out inRewardType)
                        && Enum.IsDefined(typeof(evolib.GiveAndTake.Type), inRewardType))
                    {
                        masterData.AddReward(new Reward
                        {
                            rewardId = (string)row["Name"],
                            giveMethods = inGiveMethods,
                            rewardType = inRewardType,
                            itemId = (string)row["ItemId"],
                            assetsId = (string)row["AssetsId"],
                            amount = (int)row["Amount"],
                        });
                    }
                }
                break;
            }

			////////////////////////////////////////////////////////////////////
			// Achievement
			////////////////////////////////////////////////////////////////////
			foreach (var table in root)
			{
				if ("EvoAchievementDataTableRow" != RowStructName(table)) continue;

				foreach (var row in table["Rows"])
				{
					evolib.Achievement.Type type;
					if (Enum.TryParse((string)row["AchievementType"], out type)
						&& Enum.IsDefined(typeof(evolib.Achievement.Type), type))
					{
						masterData.AddAchievement(new Achievement
						{
							achievementId = (string)row["Name"],
							type = type,
							value = (int)row["AchievementValue"],
						});
					}
				}
				break;
			}

            ////////////////////////////////////////////////////////////////////
            // Assets
            ////////////////////////////////////////////////////////////////////
            foreach (var table in root)
            {
                if ("EvoAssetsDataTableRow" != RowStructName(table)) continue;

                foreach (var row in table["Rows"])
                {
                    masterData.AddAssetsInfo(new AssetsInfo
                    {
                        assetsId = (string)row["Name"],
                        maxValue = (Int64)row["MaxValue"],
                        type = (string)row["Type"],
                    });
                }
			}

			////////////////////////////////////////////////////////////////////
            // PresentBox
            ////////////////////////////////////////////////////////////////////
            foreach (var table in root)
            {
                if ("EvoPresentDataTableRow" != RowStructName(table)) continue;

                foreach (var row in table["Rows"])
                {
                    evolib.PresentBox.Type type;
                    if (Enum.TryParse((string)row["Type"], out type)
                        && Enum.IsDefined(typeof(evolib.PresentBox.Type), type))
                    {
                        masterData.AddPresentBox(new PresentBox
                        {
                            type = type,
                            value = (int)row["EndRange"],
                        });
                    }
                }
                break;
            }

            ////////////////////////////////////////////////////////////////////
            // SupplyPod
            ////////////////////////////////////////////////////////////////////
            foreach (var table in root)
			{
				if ("EvoSupplyPodRow" != RowStructName(table)) continue;

				foreach (var row in table["Rows"])
				{
					var supplyPodId = (string)row["Name"];
					var itemDataTableKey = (string)row["ItemDataTable"];

					var startDateStr = (string)row["StartDateUtc"];
					var endDateStr = (string)row["EndDateUtc"];
					DateTime startDate;
					DateTime endDate;
					if (!DateTime.TryParse(startDateStr, out startDate))
					{
						startDate = DateTime.MinValue;
					}
					if (!DateTime.TryParse(endDateStr, out endDate))
					{
						endDate = DateTime.MaxValue;
					}
					evolib.SupplyPod.Type type;
					Enum.TryParse((string)row["Type"], out type);

					var supplyPod = new SupplyPod(supplyPodId, startDate, endDate, type);

					var isUseCoin = (bool)row["bIsUseCoin"];
					if (isUseCoin)
					{ // EC or CP
						evolib.SupplyPod.ConsumeType consumeType;
						Enum.TryParse((string)row["Coin"]["CoinType"], out consumeType);

						var info = new SupplyPod.ConsumeInfo();
                        info.assetsId = "";
                        if (consumeType == evolib.SupplyPod.ConsumeType.CP)
                        {
                            var assetsInfo = masterData.GetAssetsInfoByType("CP");
                            if (assetsInfo != null)
                            {
                                info.assetsId = assetsInfo.assetsId;
                            }
                        }
						info.singleValue = (int)row["Coin"]["SingleValue"];
						info.packageValue = (int)row["Coin"]["PackageValue"];
						supplyPod.AddSupplyPodConsumeInfo(consumeType, info);
					}
					var isUseTicket = (bool)row["bIsUseTicket"];
					if (isUseTicket)
					{
						evolib.SupplyPod.ConsumeType consumeType = evolib.SupplyPod.ConsumeType.Ticket;

						var info = new SupplyPod.ConsumeInfo();
						info.assetsId = (string)row["Ticket"]["AssetsId"];
						info.singleValue = (int)row["Ticket"]["AssetsSingleValue"];
						info.packageValue = (int)row["Ticket"]["AssetsPackageValue"];
						supplyPod.AddSupplyPodConsumeInfo(consumeType, info);
					}

					masterData.AddSupplyPod(supplyPod);

					var itemDataTable = SearchDataTable(itemDataTableKey);
					if (itemDataTable != null)
					{
						var supplyPodLineup = new SupplyPodLineup(supplyPodId);
						foreach (var itemRow in itemDataTable["Rows"])
						{
							var info = new SupplyPodLineup.ItemInfo();
							var itemId = (string)itemRow["ItemId"];
							info.odds = (int)itemRow["Odds"];
							info.dbIndex = (int)itemRow["DBIndex"];

							var item = masterData.GetItemFromItemId(itemId);
							if (item == null)
							{
								throw new Exception("MasterData_SupplyPod itemId Error :" + row["itemId"]);
							}

							// 通常抽選用のラインナップ
							supplyPodLineup.AddSupplyPodLineup(itemId, info);

							// 高レア抽選用のラインナップ
							if (item.rankType >= evolib.SupplyPod.HighRankType)
							{
								supplyPodLineup.AddSupplyPodRareLineup(itemId, info);
							}

						}
						masterData.AddSupplyPodLineup(supplyPodLineup);
					}
				}
				break;
			}

			////////////////////////////////////////////////////////////////////
			// MaterialConversion
			////////////////////////////////////////////////////////////////////
			foreach (var table in root)
			{
				if ("EvoSupplyPodConversionRow" != RowStructName(table)) continue;

				foreach (var row in table["Rows"])
				{
					evolib.Item.RankType type;
					if (Enum.TryParse((string)row["Rank"], out type)
						&& Enum.IsDefined(typeof(evolib.Item.RankType), type))
					{
						masterData.AddMaterialConversion(new MaterialConversion
						{
							rankType = type,
							value = (int)row["MaterialValue"],
						});
					}
				}

				break;
			}

			////////////////////////////////////////////////////////////////////
			// Challenge
			////////////////////////////////////////////////////////////////////
			foreach (var table in root)
			{
				var rowStructName = RowStructName(table);
				if ("EvoDailyChallengeRow" == rowStructName ||
					"EvoWeeklyChallengeRow" == rowStructName ||
					"EvoBeginnerChallengeRow" == rowStructName)
				{
					foreach (var row in table["Rows"])
					{
						var challengeType = evolib.Challenge.Type.Daily;
						if (rowStructName.Contains("Weekly"))
						{
							challengeType = evolib.Challenge.Type.Weekly;
						}
						else if (rowStructName.Contains("Beginner"))
						{
							challengeType = evolib.Challenge.Type.Beginner;
						}

						var challengeId = (string)row["Name"];
						var requiredNum = (int)row["RequiredNum"];

						evolib.Challenge.ReporterType reporterType;
						Enum.TryParse((string)row["ChallengeReporter"], out reporterType);

						evolib.Challenge.OutgameServerChallengeType outgameServerChallengeType;
						Enum.TryParse((string)row["OutgameServerChallengeType"], out outgameServerChallengeType);

						var sheetId = row["SheetId"] != null ? (string)row["SheetId"] : string.Empty;

						masterData.AddChallenge(new Challenge
						{
							challengeId = challengeId,
							type = challengeType,
							sheetId = sheetId,
							reporterType = reporterType,
							outgameServerChallengeType = outgameServerChallengeType,
							requiredNum = requiredNum,
						});
					}
				}
				else if ("EvoChallengeExpirationRow" == RowStructName(table))
				{
					foreach (var row in table["Rows"])
					{
						evolib.Challenge.Type challengeType;
						Enum.TryParse((string)row["Type"], out challengeType);

						System.DayOfWeek dayOfWeek;
						Enum.TryParse((string)row["FinishDayOfWeek"], out dayOfWeek);

						TimeSpan expirationOffsetTime;
						TimeSpan.TryParse((string)row["ExpirationOffsetTime"], out expirationOffsetTime);

						var baseDate = new DateTime(2021, 4, 18);
						baseDate += TimeSpan.FromDays((int)dayOfWeek);
						baseDate += expirationOffsetTime;

						if (challengeType == evolib.Challenge.Type.Daily)
						{
							expirationOffsetTime = new TimeSpan(baseDate.Hour, baseDate.Minute, 0);
						}
						else if (challengeType == evolib.Challenge.Type.Weekly)
						{
							expirationOffsetTime = new TimeSpan((int)baseDate.DayOfWeek*24+baseDate.Hour, baseDate.Minute, 0);
						}

						masterData.AddChallengeExpiration(new ChallengeExpiration
						{
							type = challengeType,
							expirationOffsetTime = expirationOffsetTime,
						});
					}
				}
				else if ("EvoBeginnerChallengeSheetRow" == RowStructName(table))
				{
					foreach (var row in table["Rows"])
					{
						var sheetId = (string)row["Name"];
						var order = (int)row["Order"];

						masterData.AddBeginnerChallengeSheet(new BeginnerChallengeSheet
						{
							sheetId = sheetId,
							order = order,
						});
					}
				}
			}
			return masterData;
		}
	}
}

//var testStrings = new List<string[]>
//{
//new string[]{ "ass", "NG", },
//new string[]{ "Asshimass", "OK", },
			
//};

//foreach (var str in testStrings)
//{
//	var sw = new System.Diagnostics.Stopwatch();
//	sw.Start();
//	var r = masterData.CheckNgWords(str[0]) ? "NG" : "OK";
//	sw.Stop();


//	Console.WriteLine(
//		((r == str[1]) ? "〇" : "×")
//		+ str[0]
//		+ ":" + r
//		+ " (" + sw.ElapsedMilliseconds + "ms)");
//}
