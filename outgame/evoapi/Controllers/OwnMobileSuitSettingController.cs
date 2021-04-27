using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


using evolib.Kvs.Models;
using evolib.Databases.personal;
using evolib;
using evolib.Log;
using evoapi.ProtocolModels;
using evoapi.ProtocolModels.OwnMobileSuitSetting;


namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class OwnMobileSuitSettingController : BaseController
	{
		class DbRecordsStocker
		{
			List<OwnMobileSuitSetting> MsList;
			List<OwnVoicePackSetting> VpList;
			List<ItemInventory> IiList;

			public async Task Init(PersonalDBShardManager pdbsm, List<long> playerIds, List<string> msIds = null)
			{
				MsList = new List<OwnMobileSuitSetting>();
				VpList = new List<OwnVoicePackSetting>();
				IiList = new List<ItemInventory>();

				var dbMap = new Dictionary<PersonalDBContext, List<long> >();
				playerIds.ForEach(id =>
				{
					var db = pdbsm.PersonalDBContext(id);
					if ( !dbMap.ContainsKey(db) )
					{
						dbMap[db] = new List<long>();
					}
					dbMap[db].Add(id);
				});


				foreach( var db in dbMap )
				{
					{
						var query = db.Key.OwnMobileSuitSettings.Where(r => db.Value.Contains(r.playerId));
						if (msIds != null)
						{
							query.Where(r => msIds.Contains(r.mobileSuitId));
						}

						MsList.AddRange(await query.ToListAsync());
					}

					{
						var query = db.Key.OwnVoicePackSettings.Where(r => db.Value.Contains(r.playerId));
						if (msIds != null)
						{
							query.Where(r => msIds.Contains(r.mobileSuitId));
						}

						VpList.AddRange(await query.ToListAsync());
					}

					{
						var query = db.Key.ItemInventories.Where(r => db.Value.Contains(r.playerId)
																		&& r.itemType == Item.Type.MobileSuit);
						IiList.AddRange(await query.ToListAsync());
					}
				}
			}

			public class GetResult
			{
				public OwnMobileSuitSetting msRecord { get; set; }
				public bool msRecordNew { get; set; }
				public List<OwnVoicePackSetting> vpRecords { get; set; }
				public List<bool> vpRecordsNew { get; set; }
				public bool owned { get; set; }

				public async Task AddToDb(PersonalDBShardManager pdbsm)
				{
					var db = pdbsm.PersonalDBContext(msRecord.playerId);

					if (msRecordNew)
					{
						await db.OwnMobileSuitSettings.AddAsync(msRecord);
					}

					for (int i = 0; i < vpRecords.Count; i++)
					{
						if (vpRecordsNew[i])
						{
							await db.OwnVoicePackSettings.AddAsync(vpRecords[i]);
						}
					}
				}

				public Setting ToSetting()
				{
					var setting = new Setting
					{
						mobileSuitId = msRecord.mobileSuitId,
						owned = owned,

						voicePackItemId = msRecord.voicePackItemId,
						ornamentItemId = msRecord.ornamentItemId,
						bodySkinItemId = msRecord.bodySkinItemId,
						weaponSkinItemId = msRecord.weaponSkinItemId,
						mvpCelebrationItemId = msRecord.mvpCelebrationItemId,

						stampSlotItemIds = new List<string>()
						{
							msRecord.stampSlotItemId1,
							msRecord.stampSlotItemId2,
							msRecord.stampSlotItemId3,
							msRecord.stampSlotItemId4,
						},
						emotionSlotItemIds = new List<string>()
						{
							msRecord.emotionSlotItemId1,
							msRecord.emotionSlotItemId2,
							msRecord.emotionSlotItemId3,
							msRecord.emotionSlotItemId4,
						},

						voicePackSettings = new List<Setting.VoicePackSetting>(),
					};
			
					foreach (var vpRecord in vpRecords)
					{
						setting.voicePackSettings.Add(new Setting.VoicePackSetting()
						{
							voicePackItemId = vpRecord.voicePackItemId,
							voiceSlotVoiceIds = new List<string>()
							{
								vpRecord.voiceId1,
								vpRecord.voiceId2,
								vpRecord.voiceId3,
								vpRecord.voiceId4,
							},
						});
					}

					return setting;
				}
			}
			public GetResult Get( long playerId, string msId, evolib.Services.MasterData.IMasterData masterData )
			{
				// TODO : code for cbt1 from ~
				var hashNo1 = evolib.Util.Hasher.ToUint($"11{playerId}11", uint.MaxValue);
				var hashNo2 = evolib.Util.Hasher.ToUint($"22{playerId}22", uint.MaxValue);


				var msMastreData = masterData.GetMobileSuit(msId);

				var msRecordNew = false;
				var msRecord = MsList.Find(r => r.playerId==playerId && r.mobileSuitId == msId);
				if (msRecord == null)
				{
					msRecordNew = true;
					msRecord = new OwnMobileSuitSetting();

					msRecord.playerId = playerId;
					msRecord.mobileSuitId = msId;

					// TODO : code for cbt1 from ~
					//msRecord.voicePackItemId = msMastreData.DefaultItemId(evolib.Item.Type.VoicePack);
					msRecord.voicePackItemId = $"IT07_{((hashNo1 % 12) + 1):D04}";
					// ~here

					msRecord.ornamentItemId = msMastreData.DefaultItemId(evolib.Item.Type.Ornament);
					msRecord.bodySkinItemId = msMastreData.DefaultItemId(evolib.Item.Type.BodySkin);
					msRecord.weaponSkinItemId = msMastreData.DefaultItemId(evolib.Item.Type.WeaponSkin);
					msRecord.mvpCelebrationItemId = msMastreData.DefaultItemId(evolib.Item.Type.MvpCelebration);

					msRecord.stampSlotItemId1 = msMastreData.DefaultItemId(evolib.Item.Type.Stamp);
					msRecord.stampSlotItemId2 = msMastreData.EmptyItemId(evolib.Item.Type.Stamp);
					msRecord.stampSlotItemId3 = msMastreData.EmptyItemId(evolib.Item.Type.Stamp);
					msRecord.stampSlotItemId4 = msMastreData.EmptyItemId(evolib.Item.Type.Stamp);

					msRecord.emotionSlotItemId1 = msMastreData.DefaultItemId(evolib.Item.Type.Emotion);
					msRecord.emotionSlotItemId2 = msMastreData.EmptyItemId(evolib.Item.Type.Emotion);
					msRecord.emotionSlotItemId3 = msMastreData.EmptyItemId(evolib.Item.Type.Emotion);
					msRecord.emotionSlotItemId4 = msMastreData.EmptyItemId(evolib.Item.Type.Emotion);
				}

				var vpRecordsNew = new List<bool>();
				var vpRecords = new List<OwnVoicePackSetting>();
				foreach (var voicePackItemId in masterData.AllVoicePackItemIds)
				{
					var vpRecordNew = false;
					var vpRecord = VpList.Find(r => r.playerId == playerId && r.mobileSuitId == msId && r.voicePackItemId == voicePackItemId);
					if (vpRecord == null)
					{
						vpRecordNew = true;
						vpRecord = new OwnVoicePackSetting();

						var vpMasterData = masterData.GetVoicePack(voicePackItemId);

						vpRecord.playerId = playerId;
						vpRecord.mobileSuitId = msId;
						vpRecord.voicePackItemId = voicePackItemId;

						vpRecord.voiceId1 = vpMasterData.DefaultVoiceId;
						vpRecord.voiceId2 = vpMasterData.EmptyVoiceId;
						vpRecord.voiceId3 = vpMasterData.EmptyVoiceId;
						vpRecord.voiceId4 = vpMasterData.EmptyVoiceId;

						// TODO : code for cbt1 from ~
						if ( msRecord.voicePackItemId == voicePackItemId)
						{
							vpRecord.voiceId1 = ((hashNo2 % 2) == 0) ? "0001" : "0002";
							vpRecord.voiceId2 = ((hashNo2 % 2) == 0) ? "0003" : "0004";
							vpRecord.voiceId3 = "0005";
							vpRecord.voiceId4 = "0006";
						}
						// ~here
					}
					vpRecordsNew.Add(vpRecordNew);
					vpRecords.Add(vpRecord);
				}

				return new GetResult
				{
					msRecord = msRecord,
					msRecordNew = msRecordNew,
					vpRecords = vpRecords,
					vpRecordsNew =vpRecordsNew,
					owned = masterData.CheckDefaultOwnedItem(msMastreData.itemId)
						|| null != IiList.Find(r => r.playerId == playerId && r.itemId == msMastreData.itemId),
				};
			}
		}


		public OwnMobileSuitSettingController(
			Services.IServicePack servicePack
		) : base(servicePack) { }



		async Task<bool> Validation(
			List<string> mobileSuitIds,
			evolib.Item.Type itemType,
			List<string> itemIds,
			PersonalDBContext db)
		{
			for (int i = 0; i < mobileSuitIds.Count; i++)
			{
				var msId = mobileSuitIds[i];
				var msMasterData = MasterData.GetMobileSuit(msId);
				if (msMasterData == null)
				{
					return false;
				}

				for (int j = 0; j < itemIds.Count; j++)
				{
					var itemId = itemIds[j];
					if (!msMasterData.CheckEnabledItemId(itemType, itemId))
					{
						return false;
					}
				}
			}

			var ownedRecords
				= await db.ItemInventories.Where(
					r => r.playerId == SelfHost.playerInfo.playerId
					&& r.itemType == itemType ).ToListAsync();


			for (int i = 0; i < itemIds.Count; i++)
			{
				var itemId = itemIds[i];

				if(	!MasterData.CheckDefaultOwnedItem(itemId)
						&& null == ownedRecords.Find(r => r.itemId == itemId) )
				{
					return false;
				}
			}

			return true;
		}

		[HttpPost]
		public async Task<IActionResult> GetSettingsList([FromBody]GetSettingsList.Request req)
		{
			var res = new GetSettingsList.Response();
			res.settingsList = new List<GetSettingsList.Settings>();

			var rs = new DbRecordsStocker();
			await rs.Init(PDBSM, req.playerIds);


			foreach (var playerId in req.playerIds)
			{
				var settings = new GetSettingsList.Settings
				{
					playerId = playerId,
					settings = new List<Setting>(),
				};

				foreach (var msId in MasterData.AllMobileSuitIds)
				{
					var result = rs.Get(playerId, msId, MasterData);

					if (SelfHost.hostType == HostType.Player && playerId == SelfHost.playerInfo.playerId)
					{
						await result.AddToDb(PDBSM);
					}

					settings.settings.Add(result.ToSetting());
				}

				res.settingsList.Add(settings);
			}

			if (SelfHost.hostType == HostType.Player)
			{
				await PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId).SaveChangesAsync();
			}

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> SetVoicePack([FromBody]SetVoicePack.Request req)
		{
			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			if (!await Validation(req.mobileSuitIds,
									evolib.Item.Type.VoicePack,
									new List<string>() { req.voicePackItemId },
									db ))
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}

			var voicePackMasterData = MasterData.GetVoicePack(req.voicePackItemId);
			if (voicePackMasterData == null)
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}

			for (int i = 0; i < req.voiceIds.Count; i++)
			{
				if (!voicePackMasterData.IsIncludeVoice(req.voiceIds[i]))
				{
					return BuildErrorResponse(Error.LowCode.BadParameter);
				}
			}

			var playerId = SelfHost.playerInfo.playerId;

			var res = new SetVoicePack.Response();
			res.settings = new List<Setting>();

			var rs = new DbRecordsStocker();
			await rs.Init(PDBSM, new List<long>() { playerId }, req.mobileSuitIds);

			for (int i = 0; i < req.mobileSuitIds.Count; i++)
			{
				var msId = req.mobileSuitIds[i];

				var result = rs.Get(playerId, msId, MasterData);
				await result.AddToDb(PDBSM);

				result.msRecord.voicePackItemId = req.voicePackItemId;

				var vpRecord = result.vpRecords.Find(r => r.voicePackItemId == req.voicePackItemId);
				vpRecord.voiceId1 = req.voiceIds[0];
				vpRecord.voiceId2 = req.voiceIds[1];
				vpRecord.voiceId3 = req.voiceIds[2];
				vpRecord.voiceId4 = req.voiceIds[3];

				res.settings.Add(result.ToSetting());

                Logger.Logging(
                    new LogObj().AddChild(new LogModels.ChangePackItem
                    {
                        PlayerId = playerId,
                        Date = DateTime.UtcNow,
                        UnitId = msId,
                        ItemType = evolib.Item.Type.VoicePack,
                        ItemId = req.voicePackItemId,
                        ItemIds = req.voiceIds,
                    })
                );
            }

			await db.SaveChangesAsync();

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> SetOrnament([FromBody]SetOrnament.Request req)
		{
			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			if (!await Validation(req.mobileSuitIds,
									evolib.Item.Type.Ornament,
									new List<string>() { req.ornamentItemId },
									db ))
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}

			var playerId = SelfHost.playerInfo.playerId;

			var res = new SetOrnament.Response();
			res.settings = new List<Setting>();

			var rs = new DbRecordsStocker();
			await rs.Init(PDBSM, new List<long>() { playerId }, req.mobileSuitIds);

			for (int i = 0; i < req.mobileSuitIds.Count; i++)
			{
				var msId = req.mobileSuitIds[i];

				var result = rs.Get(playerId, msId, MasterData);
				await result.AddToDb(PDBSM);

				result.msRecord.ornamentItemId = req.ornamentItemId;

				res.settings.Add(result.ToSetting());

                Logger.Logging(
                    new LogObj().AddChild(new LogModels.ChangeCustomItem
                    {
                        PlayerId = playerId,
                        Date = DateTime.UtcNow,
                        UnitId = msId,
                        ItemType = evolib.Item.Type.Ornament,
                        ItemId = req.ornamentItemId,
                    })
                );
			}

			await db.SaveChangesAsync();

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> SetBodySkin([FromBody]SetBodySkin.Request req)
		{
			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			if (!await Validation(new List<string>() { req.mobileSuitId },
									evolib.Item.Type.BodySkin,
									new List<string>() { req.bodySkinItemId },
									db ))
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}

			var playerId = SelfHost.playerInfo.playerId;
			var msId = req.mobileSuitId;

			var rs = new DbRecordsStocker();
			await rs.Init(PDBSM, new List<long>() { playerId }, new List<string>() { msId } );

			var result = rs.Get(playerId, msId, MasterData);
			await result.AddToDb(PDBSM);

			result.msRecord.bodySkinItemId = req.bodySkinItemId;

			await db.SaveChangesAsync();

            Logger.Logging(
                new LogObj().AddChild(new LogModels.ChangeCustomItem
                {
                    PlayerId = playerId,
                    Date = DateTime.UtcNow,
                    UnitId = msId,
                    ItemType = evolib.Item.Type.BodySkin,
                    ItemId = req.bodySkinItemId,
                })
            );

			return Ok(new SetBodySkin.Response
			{
				setting = result.ToSetting(),
			});
		}

		[HttpPost]
		public async Task<IActionResult> SetWeaponSkin([FromBody]SetWeaponSkin.Request req)
		{
			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			if (!await Validation(new List<string>() { req.mobileSuitId },
									evolib.Item.Type.WeaponSkin,
									new List<string>() { req.weaponSkinItemId },
									db ))
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}

			var playerId = SelfHost.playerInfo.playerId;
			var msId = req.mobileSuitId;

			var rs = new DbRecordsStocker();
			await rs.Init(PDBSM, new List<long>() { playerId }, new List<string>() { msId });

			var result = rs.Get(playerId, msId, MasterData);
			await result.AddToDb(PDBSM);

			result.msRecord.weaponSkinItemId = req.weaponSkinItemId;

			await db.SaveChangesAsync();

            Logger.Logging(
                new LogObj().AddChild(new LogModels.ChangeCustomItem
                {
                    PlayerId = playerId,
                    Date = DateTime.UtcNow,
                    UnitId = msId,
                    ItemType = evolib.Item.Type.WeaponSkin,
                    ItemId = req.weaponSkinItemId,
                })
            );

			return Ok(new SetWeaponSkin.Response
			{
				setting = result.ToSetting(),
			});
		}

		[HttpPost]
		public async Task<IActionResult> SetMvpCelebration([FromBody]SetMvpCelebration.Request req)
		{
			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			if (!await Validation(new List<string>() { req.mobileSuitId },
									evolib.Item.Type.MvpCelebration,
									new List<string>() { req.mvpCelebrationItemId },
									db ))
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}

			var playerId = SelfHost.playerInfo.playerId;
			var msId = req.mobileSuitId;

			var rs = new DbRecordsStocker();
			await rs.Init(PDBSM, new List<long>() { playerId }, new List<string>() { msId });

			var result = rs.Get(playerId, msId, MasterData);
			await result.AddToDb(PDBSM);

			result.msRecord.mvpCelebrationItemId = req.mvpCelebrationItemId;

			await db.SaveChangesAsync();

            Logger.Logging(
                new LogObj().AddChild(new LogModels.ChangeCustomItem
                {
                    PlayerId = playerId,
                    Date = DateTime.UtcNow,
                    UnitId = msId,
                    ItemType = evolib.Item.Type.MvpCelebration,
                    ItemId = req.mvpCelebrationItemId,
                })
            );

			return Ok(new SetMvpCelebration.Response
			{
				setting = result.ToSetting(),
			});
		}

		[HttpPost]
		public async Task<IActionResult> SetStampSlot([FromBody]SetStampSlot.Request req)
		{
			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			if (!await Validation(req.mobileSuitIds,
									evolib.Item.Type.Stamp,
									req.stampItemIds,
									db ))
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}

			var playerId = SelfHost.playerInfo.playerId;

			var rs = new DbRecordsStocker();
			await rs.Init(PDBSM, new List<long>() { playerId }, req.mobileSuitIds );

			var res = new SetStampSlot.Response();
			res.settings = new List<Setting>();

			for (int i = 0; i < req.mobileSuitIds.Count; i++)
			{
				var msId = req.mobileSuitIds[i];

				var result = rs.Get(playerId, msId, MasterData);
				await result.AddToDb(PDBSM);

				result.msRecord.stampSlotItemId1 = req.stampItemIds[0];
				result.msRecord.stampSlotItemId2 = req.stampItemIds[1];
				result.msRecord.stampSlotItemId3 = req.stampItemIds[2];
				result.msRecord.stampSlotItemId4 = req.stampItemIds[3];

				res.settings.Add(result.ToSetting());

                Logger.Logging(
                    new LogObj().AddChild(new LogModels.ChangePalletItem
                    {
                        PlayerId = playerId,
                        Date = DateTime.UtcNow,
                        UnitId = msId,
                        ItemType = evolib.Item.Type.Stamp,
                        ItemIds = req.stampItemIds,
                    })
                );
			}

			await db.SaveChangesAsync();

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> SetEmotionSlot([FromBody]SetEmotionSlot.Request req)
		{
			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			if (!await Validation(new List<string>() { req.mobileSuitId },
									evolib.Item.Type.Emotion,
									req.emotionItemIds,
									db ))
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}

			var playerId = SelfHost.playerInfo.playerId;
			var msId = req.mobileSuitId;

			var rs = new DbRecordsStocker();
			await rs.Init(PDBSM, new List<long>() { playerId }, new List<string>() { msId });

			var result = rs.Get(playerId, msId, MasterData);
			await result.AddToDb(PDBSM);

			result.msRecord.emotionSlotItemId1 = req.emotionItemIds[0];
			result.msRecord.emotionSlotItemId2 = req.emotionItemIds[1];
			result.msRecord.emotionSlotItemId3 = req.emotionItemIds[2];
			result.msRecord.emotionSlotItemId4 = req.emotionItemIds[3];

			await db.SaveChangesAsync();

            Logger.Logging(
                new LogObj().AddChild(new LogModels.ChangePalletItem
                {
                    PlayerId = playerId,
                    Date = DateTime.UtcNow,
                    UnitId = msId,
                    ItemType = evolib.Item.Type.Emotion,
                    ItemIds = req.emotionItemIds,
                })
            );

			return Ok(new SetEmotionSlot.Response
			{
				setting = result.ToSetting(),
			});
		}
	}
}
