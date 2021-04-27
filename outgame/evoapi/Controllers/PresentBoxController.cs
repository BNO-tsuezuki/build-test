using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using evolib.Databases.personal;

using evoapi.ProtocolModels;
using evoapi.ProtocolModels.PresentBox;

namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class PresentBoxController : BaseController
	{
		public PresentBoxController(
			Services.IServicePack servicePack
		) : base(servicePack) { }

		[HttpPost]
		public async Task<IActionResult> GetList([FromBody]GetList.Request req)
		{
            var res = new GetList.Response();

            int takeNum = req.getNum.Value;
            int skipNum = req.pageNum * takeNum;

            var now = DateTime.UtcNow;

            var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);
            var query = db.PresentBoxs.Where(i =>
                i.playerId == SelfHost.playerInfo.playerId &&
                i.beginDate < now &&
                i.endDate > now
            );
            var source = await query.ToListAsync();
            source.Sort((a, b) => (a.endDate.CompareTo(b.endDate)));

            var count = source.Count;
            res.count = count;

            var dataSrc = new List<PresentBox>();
            dataSrc.AddRange(source.Skip(skipNum).Take(takeNum));

            var dataDst = new List<GetList.Response.Present>();

            dataSrc.ForEach(
                i => dataDst.Add(new GetList.Response.Present()
                {
                    id = i.Id,
                    datetime = i.endDate,
                    type = i.type,
                    presentId = (i.presentId != null) ? i.presentId : "",
                    amount = i.amount,
                    giveType = i.giveType,
                    text = (i.text != null) ? i.text : "",
                })
            );

            res.list = dataDst;

            return Ok(res);
		}

        [HttpPost]
        public async Task<IActionResult> GivePresent([FromBody]GivePresent.Request req)
        {
            var res = new GivePresent.Response();

            var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

            // プレゼントボックスから資産(プレゼント)を獲得する
            var results = await evolib.PresentBox.GiveAsync(
                MasterData, db, SelfHost.accountAccessToken,
                SelfHost.playerInfo.playerId,
                req.ids);

            var changed = false;

            var models = new List<GivePresent.GiveModel>();
            var openItems = new List<evolib.Item.OpenItem>();
            var balances = new List<evolib.GiveAndTake.BalanceModel>();

            // 獲得結果を集計する
            foreach (var r in results)
            {
                if (r.result == evolib.PresentBox.GiveResult.Ok)
                {
                    if (r.model.type == evolib.GiveAndTake.Type.Coin)
                    {
                        changed = true;
                    }
                    if (r.model.type == evolib.GiveAndTake.Type.Assets)
                    {
                        changed = true;
                    }
                    if (r.model.type == evolib.GiveAndTake.Type.Item)
                    {
                        openItems.Add(new evolib.Item.OpenItem
                        {
                            itemId = r.model.itemId,
                            close = false,
                        });
                    }

                    models.Add(new GivePresent.GiveModel
                    {
                        id = r.id,
                        model = r.model,
                    });
                }
            }

            if (changed)
            {
                // 資産残高照会。最新の残高をDBから取得する
                balances = await evolib.GiveAndTake.BalanceAsync(
                                            MasterData,
                                            db,
                                            SelfHost.accountAccessToken,
                                            SelfHost.playerInfo.playerId);
            }

            res.results = models;
            res.openItems = openItems;
            res.balances = balances;

            return Ok(res);
        }
    }
}
