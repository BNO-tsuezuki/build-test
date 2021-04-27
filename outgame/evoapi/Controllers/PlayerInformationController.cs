using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using evolib;
using evolib.Kvs.Models;
using evolib.Databases.common2;
using evolib.Databases.personal;
using evolib.Log;
using evoapi.ProtocolModels;
using evoapi.ProtocolModels.PlayerInformation;


namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class PlayerInformationController : BaseController
	{
		public PlayerInformationController(
			Services.IServicePack servicePack
		) : base(servicePack) { }

		[HttpPost]
		public async Task<IActionResult> Self([FromBody]Self.Request req)
		{
			if( SelfHost.hostType == HostType.BattleServer)
			{
				return Ok();
			}

			var player = new Player(SelfHost.playerInfo.playerId);
			if (!await player.Validate(PDBSM))
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}

			var useMsTop3 = await getUseMobileSuitSeasonTop3List(player.playerId);

			return Ok(new Self.Response()
			{
				playerId = player.playerId,
				playerName = player.Model.playerName,
				playerIconItemId = player.Model.playerIconItemId,

				battleRating = (int)player.Model.battleRating,

				playerLevel = player.Model.playerLevel,
                exp = player.Model.exp,
                nextLevelExp = player.Model.nextLevelExp,

                //TODO The following items are all dummy data.
                battleRatingMax = 89,
				battleRatingPrevMax = 67,

				openType = player.Model.openType,
				pretendOffline = player.Model.pretendOffline,

				useMobileSuitInfos = useMsTop3,
			});
		}

		[HttpPost]
		public async Task<IActionResult> Basic([FromBody]Basic.Request req)
		{
			var res = new Basic.Response();
			res.players = new List<Basic.Info>();

			foreach( var pid in req.playerIds )
			{
				var player = new Player(pid);
				if (!await player.Validate(PDBSM))
				{
					return BuildErrorResponse(Error.LowCode.BadParameter);
				}

				res.players.Add(new Basic.Info()
				{
					playerId = player.playerId,
					playerName = player.Model.playerName,
					playerLevel = player.Model.playerLevel,
					playerIconItemId = player.Model.playerIconItemId,
					matchingArea = player.Model.matchingArea,
				});
			}

			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> Detail([FromBody]Detail.Request req)
		{
			var player = new Player(req.playerId.Value);
			if (!await player.Validate(PDBSM))
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}

			var useMsTop3 = await getUseMobileSuitSeasonTop3List(player.playerId);

			return Ok(new Detail.Response()
			{
				basicInfo = new Basic.Info()
				{
					playerId = player.playerId,
					playerName = player.Model.playerName,
					playerIconItemId = player.Model.playerIconItemId,
					playerLevel = player.Model.playerLevel,
                },

				battleRating = (int)player.Model.battleRating,

				exp = player.Model.exp,
                nextLevelExp = player.Model.nextLevelExp,
                //TODO The following items are all dummy data.
                battleRatingMax = 89,
				battleRatingPrevMax = 67,

				openType = player.Model.openType,

				useMobileSuitInfos = useMsTop3,
			});
		}

		[HttpPost]
		public async Task<IActionResult> SetFirstOnetime([FromBody]SetFirstOnetime.Request req)
		{
			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			var pbi = await db.PlayerBasicInformations.FindAsync(SelfHost.playerInfo.playerId);
			if (pbi == null)
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}

			if( 0 != (pbi.initialLevel & PlayerInformation.InitialLevelFlg.Name) )
			{
				return BuildErrorResponse(Error.LowCode.BadRequest);
			}

			if( MasterData.CheckNgWords(req.playerName) )
			{
				return BuildErrorResponse(Error.LowCode.NgWord);
			}


			try
			{
				await Common2DB.PlayerNames.AddAsync(new PlayerName
				{
					playerName = req.playerName,
					playerId = pbi.playerId,
				});

				await Common2DB.SaveChangesAsync();
			}
			catch//(Exception ex)
			{
				return BuildErrorResponse(Error.LowCode.OverlapName);
			}

			pbi.playerName = req.playerName;
			pbi.initialLevel |= PlayerInformation.InitialLevelFlg.Name;
			await db.SaveChangesAsync();

			await new Player(SelfHost.playerInfo.playerId).Invalidate();

			var session = new Session(SelfHost.sessionId);
			session.Model.initialLevel = pbi.initialLevel;
			await session.SaveAsync();

			return Ok(new SetFirstOnetime.Response
			{
				initialLevel = pbi.initialLevel,
			});
		}

		[HttpPost]
		public async Task<IActionResult> ReportBattleResult([FromBody]ReportBattleResult.Request req)
		{
			var cntEarthnoid = 0;
			var cntSpacenoid = 0;
			for (int i = 0; i < req.personals.Length; i++)
			{
				if (req.personals[i].side == Battle.Side.Earthnoid) cntEarthnoid++;
				if (req.personals[i].side == Battle.Side.Spacenoid) cntSpacenoid++;
			}
			if( cntEarthnoid==0 || cntSpacenoid==0)
			{
				return Ok(new ReportBattleResult.Response());
			}

			var updateDBs = new Dictionary<PersonalDBContext, int>();
			var sideEarthnoid = new List<PlayerBattleInformatin>();
			var sideSpacenoid = new List<PlayerBattleInformatin>();
			var winSide = Battle.Side.Other;

			for (int i = 0; i < req.personals.Length; i++)
			{
				var personal = req.personals[i];
				// pbi: PlayerBattleInformation

				var db = PDBSM.PersonalDBContext(personal.playerId);
				updateDBs[db] = 12345;
				var pbi = await db.PlayerBattleInformations.FindAsync(personal.playerId);
				if (pbi == null)
				{
					pbi = new PlayerBattleInformatin();
					pbi.playerId = personal.playerId;
					pbi.rating = Battle.InitialRating;
					pbi.victory = 0;
					pbi.defeat = 0;
					pbi.draw = 0;
					await db.PlayerBattleInformations.AddAsync(pbi);
				}

				if( personal.result == Battle.Result.VICTORY )
				{
					pbi.victory++;
				}
				if (personal.result == Battle.Result.DEFEAT)
				{
					pbi.defeat++;
				}
				if (personal.result == Battle.Result.DRAW)
				{
					pbi.draw++;
				}

				if (personal.side == Battle.Side.Earthnoid)
				{
					sideEarthnoid.Add(pbi);
					if (personal.result == Battle.Result.VICTORY) winSide = Battle.Side.Earthnoid;
				}
				if (personal.side == Battle.Side.Spacenoid)
				{
					sideSpacenoid.Add(pbi);
					if (personal.result == Battle.Result.VICTORY) winSide = Battle.Side.Spacenoid;
				}
			}

			if (winSide == Battle.Side.Earthnoid || winSide == Battle.Side.Spacenoid)
			{
				var ratingEarthnoid = 0f;
				var ratingSpacenoid = 0f;
				sideEarthnoid.ForEach(p => { ratingEarthnoid += p.rating; });
				sideSpacenoid.ForEach(p => { ratingSpacenoid += p.rating; });
				ratingEarthnoid /= sideEarthnoid.Count;
				ratingSpacenoid /= sideSpacenoid.Count;

				
				Func<float,float,float> GainPoint = (winSideRating, loseSideRating) =>
				{
					float winPercentage = 1f / (MathF.Pow(10, (loseSideRating - winSideRating) / 400f) + 1f);

					var BASE = 25f;
					var COEFFICIENT = 20f;
					float gain = BASE + COEFFICIENT * (0.5f - winPercentage);

					return gain;
				};

				Action<List<PlayerBattleInformatin>, float, float, bool> TeamAdjustment =
					(pbiList, ratingAvg, gain, win) =>
				{
					pbiList.ForEach(async pbi =>
					{
						var personalGain = (gain + (win ? 1f : -1f) * gain * (ratingAvg - pbi.rating) / ratingAvg)
												* (win ? 1f : -1f);
						pbi.rating = Math.Clamp(pbi.rating + personalGain, 1f, 5000f);

						await new Player(pbi.playerId).Invalidate();
					});
				};


				if ( winSide == Battle.Side.Earthnoid )
				{
					var gain = GainPoint( ratingEarthnoid, ratingSpacenoid );
					TeamAdjustment(sideEarthnoid, ratingEarthnoid, gain, true);
					TeamAdjustment(sideSpacenoid, ratingSpacenoid, gain, false);
				}
				else
				{
					var gain = GainPoint( ratingSpacenoid, ratingEarthnoid );
					TeamAdjustment(sideEarthnoid, ratingEarthnoid, gain, false);
					TeamAdjustment(sideSpacenoid, ratingSpacenoid, gain, true);
				}
			}

			foreach( var db in updateDBs.Keys)
			{
				await db.SaveChangesAsync();
			}


			var matchDate = DateTime.UtcNow;
			for (int i = 0; i < req.personals.Length; i++)
			{
				var self = req.personals[i];

				var recentPlayers = new RecentPlayers(self.playerId.ToString());

				for (int j = 0; j < req.personals.Length; j++)
				{
					var other = req.personals[j];
					if (other.playerId == self.playerId) continue;

					await recentPlayers.SetFieldAsync(other.playerId, new RecentPlayers.Data()
					{
						playerName = other.playerName,
						matchDate = matchDate,
						opponent = self.side != other.side,
					});
				}

				var list = await recentPlayers.GetAllAsync();
				list.Sort((a, b) => { return (a.value.matchDate < b.value.matchDate) ? -1 : 1; });

				while (Social.MaxRecentPlayersCnt < list.Count)
				{
					await recentPlayers.RemoveFieldAsync(list[0].field);
				}
			}


			return Ok(new ReportBattleResult.Response
			{
			});
		}

		[HttpPost]
		public async Task<IActionResult> SetPlayerIcon([FromBody]SetPlayerIcon.Request req)
		{
			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			var item = MasterData.GetItemFromItemId(req.playerIconItemId);
			if( item==null || item.itemType != Item.Type.PlayerIcon)
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}

			if ( !MasterData.CheckDefaultOwnedItem(req.playerIconItemId)//not default
				&& !await db.ItemInventories.AnyAsync(//don't have
						i=>i.playerId==SelfHost.playerInfo.playerId && i.itemId== req.playerIconItemId) )
			{
				//don't have
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}

			var pbi = await db.PlayerBasicInformations.FindAsync(SelfHost.playerInfo.playerId);
			pbi.playerIconItemId = req.playerIconItemId;
			await db.SaveChangesAsync();

			await new Player(SelfHost.playerInfo.playerId).Invalidate();

            Logger.Logging(
                new LogObj().AddChild(new LogModels.ChangeCustomItem
                {
                    PlayerId = SelfHost.playerInfo.playerId,
                    Date = DateTime.UtcNow,
                    UnitId = "",
                    ItemType = evolib.Item.Type.PlayerIcon,
                    ItemId = req.playerIconItemId,
                })
            );

            return Ok(new SetPlayerIcon.Response
			{
				playerIconItemId = req.playerIconItemId,
			});
		}

		[HttpPost]
		public async Task<IActionResult> TutorialEnd([FromBody]TutorialEnd.Request req)
		{
			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			var pbi = await db.PlayerBasicInformations.FindAsync(SelfHost.playerInfo.playerId);
			if (pbi == null)
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}

			pbi.initialLevel |= PlayerInformation.InitialLevelFlg.Tutorial;
			await db.SaveChangesAsync();

			var session = new Session(SelfHost.sessionId);
			session.Model.initialLevel = pbi.initialLevel;
			await session.SaveAsync();

			return Ok(new TutorialEnd.Response
			{
				initialLevel = pbi.initialLevel,
			});
		}

        [HttpPost]
        public async Task<IActionResult> TutorialProgress([FromBody]TutorialProgress.Request req)
        {
            var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

            var pbi = await db.PlayerBasicInformations.FindAsync(SelfHost.playerInfo.playerId);
            if (pbi == null)
            {
                return BuildErrorResponse(Error.LowCode.ServerInternalError);
            }

            pbi.tutorialProgress = (int)req.tutorialType.Value;
            await db.SaveChangesAsync();

            Logger.Logging(
                new LogObj().AddChild(new LogModels.TutorialProgress
                {
                    PlayerId = pbi.playerId,
                    Date = DateTime.UtcNow,
                    TutorialType = req.tutorialType.Value,
                })
            );

            await new Player(SelfHost.playerInfo.playerId).Invalidate();

            return Ok(new TutorialProgress.Response
            {
                tutorialType = req.tutorialType.Value,
            });
        }

        [HttpPost]
		public async Task<IActionResult> SetOpenType([FromBody]SetOpenType.Request req)
		{
			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			var pbi = await db.PlayerBasicInformations.FindAsync(SelfHost.playerInfo.playerId);
			if (pbi == null)
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}

			pbi.openType = req.openType.Value;
			await db.SaveChangesAsync();

            Logger.Logging(
                new LogObj().AddChild(new LogModels.SetOpenType
                {
                    PlayerId = pbi.playerId,
                    Date = DateTime.UtcNow,
                    OpenType = pbi.openType,
                })
            );

			await new Player(SelfHost.playerInfo.playerId).Invalidate();

			return Ok(new SetOpenType.Response
			{
				openType = pbi.openType,
			});
		}

		[HttpPost]
		public async Task<IActionResult> ChangePretendOffline([FromBody]ChangePretendOffline.Request req)
		{
			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			var pbi = await db.PlayerBasicInformations.FindAsync(SelfHost.playerInfo.playerId);
			if (pbi == null)
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}

			pbi.pretendOffline = req.enabled.Value;
			await db.SaveChangesAsync();

            Logger.Logging(
                new LogObj().AddChild(new LogModels.ChangePretendOffline
                {
                    PlayerId = pbi.playerId,
                    Date = DateTime.UtcNow,
                    PretendOffline = pbi.pretendOffline,
                })
            );

            var session = new Session(SelfHost.sessionId);
			session.Model.pretendedOffline = req.enabled.Value;
			await session.SaveAsync();

			if (session.Model.pretendedOffline)
			{
				var onlineState = new OnlineState(SelfHost.playerInfo.playerId);
				await onlineState.DeleteAsync();
			}
			else
			{
				var onlineState = new OnlineState(SelfHost.playerInfo.playerId);
				onlineState.Model.state = req.onlineState;
				onlineState.Model.sessionId = SelfHost.sessionId;
				await onlineState.SaveAsync();
			}

			await new Player(SelfHost.playerInfo.playerId).Invalidate();

			return Ok(new ChangePretendOffline.Response
			{
				enabled = req.enabled.Value,
			});
		}

		[HttpPost]
		public async Task<IActionResult> ChangePlayerName([FromBody]ChangePlayerName.Request req)
		{
			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			var pbi = await db.PlayerBasicInformations.FindAsync(SelfHost.playerInfo.playerId);
			if (pbi == null)
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}

			if (MasterData.CheckNgWords(req.playerName))
			{
				return BuildErrorResponse(Error.LowCode.NgWord);
			}

			// TODO ここは仕様が決まってから

			return Ok(new ChangePlayerName.Response
			{
				playerName = pbi.playerName,
			});
		}

		private async Task<List<UseMobileSuitInfo>> getUseMobileSuitSeasonTop3List(long playerId)
		{
			var useMsTop3 = new List<UseMobileSuitInfo>();

			evolib.Services.MasterData.ISeason currentSeason = MasterData.GetCurrentSeason();

			//現在のシーズンが取得出来なかったなら、機体の使用率のリストを空で終了する
			if (currentSeason == null)
			{
				return useMsTop3;
			}

			int seasonNoOfRankMatch = currentSeason.seasonNo;

			var db = PDBSM.PersonalDBContext(playerId);

			// 戦績から現在のシーズンの機体の使用時間のリストを取得する（使用時間の長い順にソート）
			var useMsList = await db.CareerRecords
				.Where(x =>
				x.playerId == playerId &&
				x.matchType == evolib.Battle.MatchType.Rank &&
				x.seasonNo == seasonNoOfRankMatch &&
				x.recordItemId == evolib.CareerRecord.RECORD_ID_PLAY_TIME)
				.OrderByDescending(x => x.value)
				.ToListAsync();

			// プレイヤーがシーズンでプレイした時間（全機体の使用時間の合計）を取得する
			double totalUseTime = useMsList.Sum(x => x.value);

			// プレイヤーの使用時間の長い機体TOP3を取得する
			foreach (var playTime in useMsList)
			{
				useMsTop3.Add(new UseMobileSuitInfo
				{
					mobileSuitId = playTime.mobileSuitId,
					useRate = (totalUseTime <= 0.0f) ? 0.0f :  (float)((playTime.value / totalUseTime) * 100.0f),
				});

				if (3 <= useMsTop3.Count())
				{
					break;
				}
			}

			return useMsTop3;
		}
	}
}
