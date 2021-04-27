using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using evolib.Kvs.Models;
using evolib.Services.MasterData;

using evoapi.ProtocolModels.BattlePass;


namespace evoapi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class BattlePassController : BaseController
    {
        public BattlePassController(
            Services.IServicePack servicePack
        ) : base(servicePack) { }

        [HttpPost]
        public async Task<IActionResult> PassExpSave([FromBody]PassExpSave.Request req)
        {
            var res = new PassExpSave.Response();

            // プレイヤー毎
            foreach (var addData in req.passExp)
            {
                var playerId = addData.playerId;
                var db = PDBSM.PersonalDBContext(playerId);

                foreach (var info in addData.expInfo)
                {
                    var reco = await db.BattlePasses.FindAsync(playerId, info.passId);
                    if (reco == null)
                    {
                        var userData = new evolib.Databases.personal.BattlePass();
                        userData.playerId = playerId;
                        userData.passId = info.passId;
                        userData.isPremium = false;
                        userData.totalExp = (UInt64)info.addExp;
                        userData.createdDate = DateTime.UtcNow;
                        userData.updatedDate = DateTime.UtcNow;
                        // passIdと経験値からレベルを計算
                        userData.SetLevelDetail(MasterData);

                        // TODO:MasterDataからレベルに応じて報酬アイテムを獲得させる

                        // パス購入前のExp初加算時なので有償アイテムレベルは初期値
                        userData.rewardLevel = 0;

                        await db.BattlePasses.AddAsync(userData);
                    }
                    else
                    {
                        reco.totalExp += (UInt64)info.addExp;
                        reco.updatedDate = DateTime.UtcNow;
                        reco.SetLevelDetail(MasterData);

                        // TODO:MasterDataからレベルに応じて報酬アイテムを獲得させる

                    }
                }
                // 1プレイヤー保存
                await db.SaveChangesAsync();

                await new Player(playerId).Invalidate();
            }

            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> GetPassStatus([FromBody]GetPassStatus.Request req)
        {
            var res = new GetPassStatus.Response();

            // user data
            var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);
            var query = db.BattlePasses.Where(i =>
                i.playerId == SelfHost.playerInfo.playerId
            );
            var dataSrc = await query.ToListAsync();

            //
            var battlePasses = MasterData.GetEnableBattlePass();
            var dataDst = new List<GetPassStatus.PassStatus>();
			foreach (var pass in battlePasses)
			{
				var temp = new GetPassStatus.PassStatus();
				temp.passId = pass.id;
				temp.totalExp = 0;
				temp.isPremium = false;
				foreach (var data in dataSrc)
				{
					if (data.passId == pass.id)
					{
						temp.passId = data.passId;
						temp.totalExp = data.totalExp;
						temp.isPremium = data.isPremium;
						break;
					}
				}
				dataDst.Add(temp);
			}
            res.passStatusArray = dataDst;

            return Ok(res);
        }


    }// controller
}// namespace
