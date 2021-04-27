#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using evolib.Kvs.Models;
using evoapi.ProtocolModels;
using evoapi.ProtocolModels.Test;
using evolib.Log;


namespace evoapi.Controllers
{
	[Route("api/[controller]/[action]")]
	public class TestController : BaseController
	{
		public TestController(
			Services.IServicePack servicePack
		) : base(servicePack) { }

		[HttpPost]
		public async Task<IActionResult> DummyError([FromBody]DummyError.Request req)
		{
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

			// エラーメッセージ表示デバッグのために、クライアントから送ってきた値をそのままエラーとして返す

			if (req.statusCode != 0 && req.statusCode != 200)
			{
				return StatusCode(req.statusCode);
			}

			// BuildErrorResponseの中身を参考に。
			var err = new Error();
			err.error.errCode = req.errorCode;
			err.error.subCode = req.subCode;
			return Ok(err);
		}

 
		[HttpPost]
		public IActionResult TssVersion([FromBody]TssVersion.Request req)
		{
			return Ok(new TssVersion.Response
			{
				limitVersion = "0.1.0",
			});
		}

        [HttpPost]
        public async Task<IActionResult> GiveAssets([FromBody]GiveAssets.Request req)
        {
            var res = new GiveAssets.Response();

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

            var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

            bool take = false;
            if (req.amount < 0)
            {
                take = true;
            }

            var model = new evolib.GiveAndTake.Model
            {
                type = evolib.GiveAndTake.Type.Assets,
                assetsId = req.assetsId,
                amount = Math.Abs(req.amount),
            };

            if (take)
            {
                // Assetsを消費する
                var result = await evolib.GiveAndTake.TakeAsync(
                        MasterData, db, SelfHost.accountAccessToken,
                        SelfHost.playerInfo.playerId,
                        model);
            }
            else
            {
                // Assetsを獲得する
                var result = await evolib.GiveAndTake.GiveAsync(
                        MasterData, db, SelfHost.accountAccessToken,
                        SelfHost.playerInfo.playerId,
                        new evolib.GiveAndTake.GiveModel
                        {
                            model = model,
                            historyModel = new evolib.GiveAndTake.HistoryModel
                            {
                                giveType = evolib.PresentBox.Type.Management,
                                text = "GiveAssets",
                            }
                        });
            }

            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> GiveFreeEvoCoin([FromBody]GiveFreeEvoCoin.Request req)
        {
            var res = new GiveFreeEvoCoin.Response();

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

            if (SelfHost.accountType != evolib.Account.Type.Inky)
            {
                return BuildErrorResponse(Error.LowCode.BadRequest);
            }

            var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

            bool take = false;
            if (req.amount < 0)
            {
                take = true;
            }

            var reward = new evolib.GiveAndTake.Model
            {
                type = evolib.GiveAndTake.Type.Coin,
                amount = Math.Abs(req.amount),
            };

            if (take)
            {
                // EvoCoinを消費する
                var result = await evolib.GiveAndTake.TakeAsync(
                    MasterData, db, SelfHost.accountAccessToken,
                    SelfHost.playerInfo.playerId,
                    reward);
            }
            else
            {
                // EvoCoinを獲得する
                var result = await evolib.GiveAndTake.GiveAsync(
                    MasterData, db, SelfHost.accountAccessToken,
                    SelfHost.playerInfo.playerId,
                    new evolib.GiveAndTake.GiveModel
                    {
                        model = reward,
                        historyModel = new evolib.GiveAndTake.HistoryModel
                        {
                            giveType = evolib.PresentBox.Type.Management,
                            text = "GiveFreeEvoCoin",
                        }
                    });
            }

            return Ok(res);
        }

        [HttpPost]
		public async Task<IActionResult> EvoCoinBalance([FromBody]EvoCoinBalance.Request req)
		{
			var res = new EvoCoinBalance.Response();

			if (SelfHost.accountType != evolib.Account.Type.Inky)
			{
				return Ok(res);
			}

			var requester = new evolib.Multiplatform.Inky.GetAccountPlatinumBalance();
			var response = await requester.GetAsync(SelfHost.accountAccessToken);
			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				res.total = response.Payload.data.total_platinum;
			}

			return Ok(res);
		}

		public async Task<IActionResult> PurchasesInEvoCoin([FromBody]PurchasesInEvoCoin.Request req)
		{
			var res = new PurchasesInEvoCoin.Response();

			if (SelfHost.accountType != evolib.Account.Type.Inky)
			{
				return Ok(res);
			}

			var requester = new evolib.Multiplatform.Inky.PurchaseItem();
			requester.request.items = new List<evolib.Multiplatform.Inky.PurchaseItem.Request.PurchasedProductItems>()
			{
				new evolib.Multiplatform.Inky.PurchaseItem.Request.PurchasedProductItems
				{
					//item_name = "testitem",
					//item_type = 1,
					//item_id = "testId",
					//item_qty = 1,
					//deduct_platinum = req.prices,

					deduct_platinum = req.prices,
					item_id = "testItemId",
					item_name = "testItem",
					item_qty = 1,
					item_type = 1,
				}
			};

			var response = await requester.PostAsync(SelfHost.accountAccessToken);
			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				return Ok(res);
			}

			return BuildErrorResponse(Error.LowCode.ServerInternalError);
		}

        public async Task<IActionResult> xyBCZqwxqVRduBuOKu5FSKyFmILUT8IY()
		{
			var db = PDBSM.PersonalDBContext(SelfHost.playerInfo.playerId);

			var response = "";

			{
				var reward = new evolib.GiveAndTake.Model
				{
					type = evolib.GiveAndTake.Type.Coin,
					amount = 1,
				};

                var result = await evolib.GiveAndTake.GiveAsync(
                        MasterData, db, SelfHost.accountAccessToken,
                        SelfHost.playerInfo.playerId,
                        new evolib.GiveAndTake.GiveModel
                        {
                            model = reward,
                            historyModel = new evolib.GiveAndTake.HistoryModel
                            {
                                giveType = evolib.PresentBox.Type.Management,
                                text = "Test",
                            }
                        });

				response += $"{result}:{Newtonsoft.Json.JsonConvert.SerializeObject(reward)}\n";
            }

            {
				var reward = new evolib.GiveAndTake.Model
				{
					type = evolib.GiveAndTake.Type.Assets,
					assetsId = "AS01_0001",
					amount = 10,
				};

                var result = await evolib.GiveAndTake.GiveAsync(
                    MasterData, db, SelfHost.accountAccessToken,
                    SelfHost.playerInfo.playerId,
                    new evolib.GiveAndTake.GiveModel
                    {
                        model = reward,
                        historyModel = new evolib.GiveAndTake.HistoryModel
                        {
                            giveType = evolib.PresentBox.Type.Management,
                            text = "Test",
                        }
                    });

                response += $"{result}:{Newtonsoft.Json.JsonConvert.SerializeObject(reward)}\n";
            }

            {
				var reward = new evolib.GiveAndTake.Model
				{
					type = evolib.GiveAndTake.Type.Item,
					itemId = "IT01_MS0003_0001",
				};

                var result = await evolib.GiveAndTake.GiveAsync(
                    MasterData, db, SelfHost.accountAccessToken,
                    SelfHost.playerInfo.playerId,
                    new evolib.GiveAndTake.GiveModel
                    {
                        model = reward,
                        historyModel = new evolib.GiveAndTake.HistoryModel
                        {
                            giveType = evolib.PresentBox.Type.Management,
                            text = "Test",
                        }
                    });

                response += $"{result}:{Newtonsoft.Json.JsonConvert.SerializeObject(reward)}\n";
            }
            {
				var reward = new evolib.GiveAndTake.Model
				{
					type = evolib.GiveAndTake.Type.Item,
					itemId = "IT05_0003_v05",
				};

                var result = await evolib.GiveAndTake.GiveAsync(
                    MasterData, db, SelfHost.accountAccessToken,
                    SelfHost.playerInfo.playerId,
                    new evolib.GiveAndTake.GiveModel
                    {
                        model = reward,
                        historyModel = new evolib.GiveAndTake.HistoryModel
                        {
                            giveType = evolib.PresentBox.Type.Management,
                            text = "Test",
                        }
                    });

                response += $"{result}:{Newtonsoft.Json.JsonConvert.SerializeObject(reward)}\n";
            }

            return Ok(response);
		}

	}// controller
}// namespace
#endif // ~#if DEBUG
