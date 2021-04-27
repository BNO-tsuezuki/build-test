using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Collections.Specialized;

using evolib;
using evolib.Databases.personal;
using evolib.Databases.common3;
using evolib.Kvs.Models;
using evoapi.ProtocolModels;
using evoapi.ProtocolModels.ViewMatch;

namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class ViewMatchController : BaseController
	{
		public ViewMatchController(
            Services.IServicePack servicePack
        ) : base(servicePack) { }

        [HttpPost]
        public async Task<IActionResult> SearchAllReplay([FromBody]SearchAllReplay.Request req)
        {
			// get data from DB
			int selectNum = req.getNum.Value;
			var query = Common3DB.ReplayInfoAllMatch.OrderByDescending(
				i => i.date
			).Take(selectNum);
            var replaySrc = await query.ToListAsync();

			// check version
			var checker = VersionChecker.Get(VersionChecker.CheckTarget.Replay);
			for (int i = replaySrc.Count - 1; i >= 0; i--)
			{
				checker.PackageVersion = replaySrc[i].packageVersion;
				checker.MasterDataVersion = replaySrc[i].masterDataVersion;
				if (!checker.Check())
				{
					replaySrc.RemoveAt(i);
				}
			}

			var infoDst = new List<SearchAllReplay.UserReplayInfo>();
            // response data
            replaySrc.ForEach(
                i => infoDst.Add(new SearchAllReplay.UserReplayInfo()
                {
                    date = i.date,
                    matchType = i.matchType,
                    gameMode = i.gameMode,
                    totalTime = i.totalTime,
                    mvpUnit = i.mvpUnitId,
                    matchId = i.matchId,
                    result = i.result,
                    mapId = i.mapId,
                    mvpPlayer = i.mvpUserName,
                })
            );

            var res = new SearchAllReplay.Response();
            res.ReplayInfos = infoDst;

            return Ok(res);
        }


        [HttpPost]
		public async Task<IActionResult> SearchUserReplay([FromBody]SearchUserReplay.Request req)
		{
			// get UserPlay data
			int selectNum = req.getNum.Value;
            var personalDb = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);
            var userSelect = personalDb.ReplayUserHistory.Where(i =>
                i.playerId == SelfHost.playerInfo.playerId
            );
            if (req.matchType >= 0)
            {
                userSelect = userSelect.Where(i => i.matchType == req.matchType);
            }
            userSelect = userSelect.OrderByDescending(
                i => i.date
            );
            userSelect = userSelect.Take(selectNum);
            var userSrc = await userSelect.ToListAsync();


			// check version
			var checker = VersionChecker.Get(VersionChecker.CheckTarget.Replay);
			for (int i = userSrc.Count - 1; i >= 0; i--)
			{
				checker.PackageVersion = userSrc[i].packageVersion;
				checker.MasterDataVersion = userSrc[i].masterDataVersion;
				if (!checker.Check())
				{
					userSrc.RemoveAt(i);
				}
            }
			// Make match-id array
			string[] ids = new string[userSrc.Count];
			for (int i=0; i< userSrc.Count; i++)
			{
				ids[i] = userSrc[i].matchId;
			}

			// main data
			var query = Common3DB.ReplayInfoAllMatch.Where(i =>
                ids.Contains(i.matchId)
            );
            var replaySrc = await query.ToListAsync();

            // response data
            var infoDst = new List<SearchUserReplay.UserReplayInfo>();
            foreach (var oneData in userSrc)
            {
                var temp = new SearchUserReplay.UserReplayInfo();
                temp.date = oneData.date;
                temp.matchType = oneData.matchType;
                temp.matchId = oneData.matchId;
                temp.result = oneData.resultType.ToString(); // Win or Defeat string

                var rData = replaySrc.FirstOrDefault(i => i.matchId == oneData.matchId);
                if (rData != null)
                {
                    temp.gameMode = rData.gameMode;
                    temp.totalTime = rData.totalTime;
                    temp.mvpUnit = rData.mvpUnitId;
                    temp.matchId = rData.matchId;
                    temp.mapId = rData.mapId;
                    temp.mvpPlayer = rData.mvpUserName;
                }

                infoDst.Add(temp);
            }

            var res = new SearchUserReplay.Response();
            res.ReplayInfos = infoDst;

            return Ok(res);
		}


        [HttpPost]
        public async Task<IActionResult> SearchRankReplay([FromBody]SearchRankReplay.Request req)
        {
            int selectNum = req.getNum.Value;

			// 出撃機体フィルタ作成
			UInt64 spawn = 0;
			if (req.filter.spawnUnits.Count > 0)
			{
				foreach (int bit_idx in req.filter.spawnUnits)
				{
					UInt64 mask = (UInt64)(1 << bit_idx);
					spawn |= mask;
				}
			}
			// 表彰機体フィルタ作成
			UInt64 award = 0;
			if (req.filter.awardUnits.Count > 0)
			{
				foreach (int bit_idx in req.filter.awardUnits)
				{
					UInt64 mask = (UInt64)(1 << bit_idx);
					award |= mask;
				}
			}

			var checker = VersionChecker.Get(VersionChecker.CheckTarget.Replay);
			var rankSrc = new List<ReplayInfoRankMatch>();

			// 指定件数の2倍を仮取得
			var tempData = await Common3DB.ReplayInfoRankMatch.OrderByDescending(
				i => i.date
			).Take(selectNum*2).ToListAsync();

			// TODO：このリクエストは全ユーザ共通のDB検索結果となるため、tempDataはAPIサーバーにローカルキャッシュするなどして
			//       せめて同じAPIサーバーを利用するユーザ間で共有したい。

			// 仮取得したデータを全チェック
			foreach (var oneLow in tempData)
			{
				checker.PackageVersion = oneLow.packageVersion;
				checker.MasterDataVersion = oneLow.masterDataVersion;
				if (!checker.Check())
				{
					break;// 日付降順に取得しているのでバージョン不適合があればそれより昔のデータは全部不適合なので終了
				}

				if (req.filter.gameMode > 0 && oneLow.gameMode != req.filter.gameMode)
				{
					continue;//GameMode が違う
				}
				if (req.filter.mapId.Length > 0 && oneLow.mapId != req.filter.mapId)
				{
					continue;// Map が違う
				}
				if (req.filter.ratingBottom > 0)
				{
					if (oneLow.ratingAverage < req.filter.ratingBottom || req.filter.ratingUpper <= oneLow.ratingAverage)
					{
						continue;//指定ランク帯と違う
					}
				}
				if (spawn > 0 && (spawn & oneLow.spawnUnits) == 0)
				{
					continue;// 出撃機体指定が1つも当てはまらない
				}
				if (award > 0 && (award & oneLow.awardUnits) == 0)
				{
					continue;// 表彰機体指定が1つも当てはまらない
				}

				// 厳しい条件を乗り越え、検索結果として有効だったデータ
				rankSrc.Add(oneLow);

				// 指定件数集まれば即終了
				if (rankSrc.Count >= selectNum)
				{
					break;
				}
			}

			//-------- 検索ここまで、これ以降は詳細データのアタッチ ---------------

			// Make match-id array
			string[] ids = new string[selectNum];
            for (int i = 0; i < rankSrc.Count; i++)
            {
                ids[i] = rankSrc[i].matchId;
            }

            // main data
            var query = Common3DB.ReplayInfoAllMatch.Where(i =>
                ids.Contains(i.matchId)
            );
            var replaySrc = await query.ToListAsync();

            // View Num
            var viewquery = Common3DB.ReplayViewNum.Where(i =>
                ids.Contains(i.matchId)
            );
            var viewnumSrc = await viewquery.ToListAsync();

            // response data
            var infoDst = new List<SearchRankReplay.UserReplayInfo>();
            foreach (var oneData in rankSrc)
            {
                var temp = new SearchRankReplay.UserReplayInfo();
                temp.date = oneData.date;
                temp.matchType = evolib.Battle.MatchType.Rank;
                temp.matchId = oneData.matchId;
                temp.ratingAverage = oneData.ratingAverage;// for Rank

                var rData = replaySrc.FirstOrDefault(i => i.matchId == oneData.matchId);
                if (rData != null)
                {
                    temp.gameMode = rData.gameMode;
                    temp.totalTime = rData.totalTime;
                    temp.mvpUnit = rData.mvpUnitId;
                    temp.matchId = rData.matchId;
                    temp.result = rData.result;
                    temp.mapId = rData.mapId;
                    temp.mvpPlayer = rData.mvpUserName;
                }

                var numdata = viewnumSrc.FirstOrDefault(i => i.matchId == oneData.matchId);
                if (numdata != null)
                {
                    temp.viewNum = numdata.ViewNum;
                }

                infoDst.Add(temp);
            }

            var res = new SearchRankReplay.Response();
            res.ReplayInfos = infoDst;

            return Ok(res);
        }


        [HttpPost]
        public async Task<IActionResult> SearchFeaturedReplay([FromBody]SearchFeaturedReplay.Request req)
        {
            int selectNum = req.getNum.Value;

            // RankMatch data at FeaturedFlag ON
            var rankSelect = Common3DB.ReplayInfoRankMatch.Where(i =>
                i.isFeatured == true
            ).OrderByDescending(
                i => i.date
            ).Take(selectNum);
            var rankSrc = await rankSelect.ToListAsync();

			
			// check version
			var checker = VersionChecker.Get(VersionChecker.CheckTarget.Replay);
			for (int i = rankSrc.Count - 1; i >= 0; i--)
			{
				checker.PackageVersion = rankSrc[i].packageVersion;
				checker.MasterDataVersion = rankSrc[i].masterDataVersion;
				if (!checker.Check())
				{
					rankSrc.RemoveAt(i);
				}
			}
			// Make match-id array
			string[] ids = new string[rankSrc.Count];
			for (int i = 0; i < rankSrc.Count; i++)
			{
				ids[i] = rankSrc[i].matchId;
			}

            // main data
            var query = Common3DB.ReplayInfoAllMatch.Where(i =>
                ids.Contains(i.matchId)
            );
            var replaySrc = await query.ToListAsync();

            // Match Members
            var memberquery = Common3DB.MatchMember.Where(i =>
                ids.Contains(i.matchId)
            );
            var membersSrc = await memberquery.ToListAsync();

            // View Num
            var viewquery = Common3DB.ReplayViewNum.Where(i =>
                ids.Contains(i.matchId)
            );
            var viewnumSrc = await viewquery.ToListAsync();

            // response data
            var infoDst = new List<SearchFeaturedReplay.FeaturedInfo>();
            foreach (var oneData in rankSrc)
            {
                var temp = new SearchFeaturedReplay.FeaturedInfo();
                temp.date = oneData.date;
                temp.matchId = oneData.matchId;
                temp.ratingAverage = oneData.ratingAverage;// for Rank

                var rData = replaySrc.FirstOrDefault(i => i.matchId == oneData.matchId);
                if (rData != null)
                {
                    temp.gameMode = rData.gameMode;
                    temp.totalTime = rData.totalTime;
                    temp.mvpUnit = rData.mvpUnitId;
                    temp.matchId = rData.matchId;
                    temp.result = rData.result;
                    temp.mapId = rData.mapId;
                    temp.mvpPlayer = rData.mvpUserName;
                }

                var memberinfo = membersSrc.FirstOrDefault(i => i.matchId == oneData.matchId);
                if (memberinfo != null)
                {
                    temp.members = memberinfo.playersInfo;
                }

                var numdata = viewnumSrc.FirstOrDefault(i => i.matchId == oneData.matchId);
                if (numdata != null)
                {
                    temp.viewNum = numdata.ViewNum;
                }

                infoDst.Add(temp);
            }

            var res = new SearchFeaturedReplay.Response();
            res.FeaturedInfos = infoDst;

            return Ok(res);
        }


        [HttpPost]
        public async Task<IActionResult> ReplayInfoSave([FromBody]ReplayInfoSave.Request req)
        {
			// parse BattleServer-Package-version
			int[] pver = req.packageVersion.Split('.').Select(a => int.Parse(a)).ToArray();
			if (pver.Length < 4 )
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}
			UInt64 packageVersion = VersionChecker.Valued(VersionChecker.ReferenceSrc.PackageVersion, pver);
			//Console.WriteLine("ReplayInfoSave:packageVersion={0}", packageVersion);

			// parse BattleServer-MasterData-version
			int[] mver = req.masterDataVersion.Split('-').Select(a => int.Parse(a)).ToArray();
			if (mver.Length < 3)
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}
			UInt64 masterDataVersion = VersionChecker.Valued(VersionChecker.ReferenceSrc.MasterDataVersion, mver);
			//Console.WriteLine("ReplayInfoSave:masterDataVersion={0}", masterDataVersion);

			var info = new ReplayInfoAllMatch();
			info.packageVersion = packageVersion;
			info.masterDataVersion = masterDataVersion;
			info.date = req.baseInfo.date;
            info.matchId = req.baseInfo.matchId;
            info.matchType = req.baseInfo.matchType;
            info.mapId = req.baseInfo.mapId;
            info.gameMode = req.baseInfo.gameMode;
            info.totalTime = req.baseInfo.totalTime;
            info.result = req.baseInfo.result;
            info.mvpUserName = req.baseInfo.mvpPlayer;
            info.mvpUnitId = req.baseInfo.mvpUnit;
 
            await Common3DB.ReplayInfoAllMatch.AddAsync(info);

            // ランクマッチテーブル登録
            if (req.saveRank)
            {
                var rank = new ReplayInfoRankMatch();
				rank.packageVersion = packageVersion;
				rank.masterDataVersion = masterDataVersion;
				rank.date = req.baseInfo.date;
                rank.matchId = req.baseInfo.matchId;
                rank.mapId = req.baseInfo.mapId;
                rank.gameMode = req.baseInfo.gameMode;
                rank.mvpUnitId = req.baseInfo.mvpUnit;
                rank.ratingAverage = req.rateAverage;

                rank.isFeatured = req.isFeatured;

				// 使われた機体をbitで管理
				UInt64 spawn = 0;
				foreach (int bit_idx in req.spawnUnits)
				{
					UInt64 mask = (UInt64)(1 << bit_idx);
					spawn |= mask;
				}
				// 表彰機体をbitで管理
				UInt64 award = 0;
				foreach (int bit_idx in req.awardUnits)
				{
					UInt64 mask = (UInt64)(1 << bit_idx);
					award |= mask;
				}
				//Console.WriteLine("spawn = {0}", spawn);
				//Console.WriteLine("award = {0}", award);

				rank.spawnUnits = spawn;
                rank.awardUnits = award;

                await Common3DB.ReplayInfoRankMatch.AddAsync(rank);
            }

            // フィーチャードマッチ用データ登録
            if (req.isFeatured)
            {
                var member = new MatchMember();
                member.matchId = req.baseInfo.matchId;
                member.playersInfo = req.members;

                await Common3DB.MatchMember.AddAsync(member);
            }

            // 更新適用
            await Common3DB.SaveChangesAsync();


            // ユーザ毎の保存
            foreach (var userresult in req.playerResult)
            {
                var personalDb = PDBSM.PersonalDBContext(userresult.playerId);

                var userdata = new ReplayUserHistory();
                userdata.playerId = userresult.playerId;
                userdata.resultType = userresult.result;

				userdata.packageVersion = packageVersion;
				userdata.masterDataVersion = masterDataVersion;
				userdata.date = req.baseInfo.date;
                userdata.matchId = req.baseInfo.matchId;
                userdata.matchType = req.baseInfo.matchType;

                await personalDb.ReplayUserHistory.AddAsync(userdata);
                await personalDb.SaveChangesAsync();
            }

            var res = new ReplayInfoSave.Response();
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddViewNum([FromBody]AddViewNum.Request req)
        {
            var record = await Common3DB.ReplayViewNum.FindAsync(req.matchId);
            if (record == null)
            {
                var temp = new ReplayViewNum();
                temp.matchId = req.matchId;
                temp.ViewNum = 1;
                await Common3DB.ReplayViewNum.AddAsync(temp);
            }
            else
            {
                record.ViewNum++;
            }

            await Common3DB.SaveChangesAsync();

            var res = new AddViewNum.Response();
            return Ok(res);
        }

		[HttpPost]
		public async Task<IActionResult> GetReplayInfo([FromBody]GetReplayInfo.Request req)
		{
			// player privilegeLevel check
			var player = new Player(SelfHost.playerInfo.playerId);
			if (!await player.Validate(PDBSM))
			{
				return BuildErrorResponse(Error.LowCode.ServerInternalError);
			}
			var checkCheatCommand = player.Model.privilegeLevel & (1 << (int)evolib.Privilege.Type.CheatCommand);
			if (checkCheatCommand <= 0)
			{
				return BuildErrorResponse(Error.LowCode.BadRequest);
			}

			// get data from DB
			var selectQuery = Common3DB.ReplayInfoAllMatch.Where(
				i => i.matchId == req.matchId
			);
			var record = await selectQuery.FirstOrDefaultAsync();

			var infoDst = new GetReplayInfo.UserReplayInfo();

			// set
			if (record != null)
			{
				infoDst.date = record.date;
				infoDst.gameMode = record.gameMode;
				infoDst.mapId = record.mapId;
				infoDst.matchId = record.matchId;
				infoDst.matchType = record.matchType;
				infoDst.mvpPlayer = record.mvpUserName;
				infoDst.mvpUnit = record.mvpUnitId;
				infoDst.totalTime = record.totalTime;
				infoDst.result = record.result;
			}
			else
			{
				return BuildErrorResponse(Error.LowCode.BadParameter);
			}

			var res = new GetReplayInfo.Response();
			res.ReplayInfo = infoDst;

			return Ok(res);
		}

	}// End ViewMatchController
}
