using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using evoapi.ProtocolModels.Vivox;

namespace evoapi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class VivoxController : BaseController
    {
        public VivoxController(
            Services.IServicePack servicePack
        ) : base(servicePack) { }

        [HttpPost]
        public async Task<IActionResult> GetVivoxInfo([FromBody]GetVivoxInfo.Request req)
        {
            return Ok(new GetVivoxInfo.Response
            {
                apiEndPoint = evolib.VivoxInfo.endPoint,
                domain = evolib.VivoxInfo.domain,
                issuer = evolib.VivoxInfo.issuer,
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetVivoxLoginToken([FromBody]GetVivoxLoginToken.Request req)
        {
            var token = evolib.Vivox.VivoxGenerateToken(evolib.VivoxInfo.key, evolib.VivoxInfo.issuer, req.exp, "login", req.vxi, req.f, null);

            return Ok(new GetVivoxLoginToken.Response()
            {
                loginToken = token,
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetVivoxJoinToken([FromBody]GetVivoxJoinToken.Request req)
        {
            var token = evolib.Vivox.VivoxGenerateToken(evolib.VivoxInfo.key, evolib.VivoxInfo.issuer, req.exp, "join", req.vxi, req.f, req.t);

            return Ok(new GetVivoxJoinToken.Response()
            {
                joinToken = token,
            });
        }
    }
}
