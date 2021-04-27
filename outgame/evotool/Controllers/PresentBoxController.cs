using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using evolib.Services.MasterData;
using evotool.ProtocolModels.PresentBox;


namespace evotool.Controllers
{
	[Route("api/[controller]/[action]")]
	public class PresentBoxController : BaseController
	{
		public PresentBoxController(
			Services.IServicePack servicePack
		) : base(servicePack)
		{
		}


        [HttpPost]
        public async Task<IActionResult> TakePresent([FromBody]TakePresent.Request req)
        {
            var res = new TakePresent.Response();

            var db = PDBSM.PersonalDBContext(req.playerId.Value);
            var pbi = await db.PlayerBasicInformations.FindAsync(req.playerId);
            if (pbi == null)
            {
                return BuildErrorResponse("BadParameter");
            }

            await MasterDataLoader.LoadAsync();
            var masterData = MasterDataLoader.LatestMasterData;
            if (masterData == null)
            {
                return BuildErrorResponse("Server Side Err.");
            }

            var setting = new evolib.PresentBox.Setting
            {
                beginDate = req.beginDate,
                endDate = req.endDate,
                text = req.text,
            };

            var model = new evolib.PresentBox.Model
            {
                type = req.type,
                id = req.id,
                amount = req.amount,
                presentType = req.presentType,
                setting = setting,
            };

            // プレゼントボックスに保存する
            var result = await evolib.PresentBox.TakeAsync(
                masterData, db,
                req.playerId.Value,
                model);

            return Ok(res);
        }
    }
}
