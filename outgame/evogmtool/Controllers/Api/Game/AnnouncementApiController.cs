using System.Threading.Tasks;
using evogmtool.Attributes;
using evogmtool.Models;
using evogmtool.Repositories;
using evogmtool.Services.Game;
//using evotool.ProtocolModels.GMTool.AnnouncementApi;
using Microsoft.AspNetCore.Mvc;
using static evogmtool.Constants;

namespace evogmtool.Controllers.Api.Game
{
    [Route("api/game/announcement")]
    public class AnnouncementApiController : GameApiControllerBase
    {
        //private readonly IAnnouncementService _announcementService;

        public AnnouncementApiController(
            //IAnnouncementService announcementService,
            ILoginUserRepository loginUserRepository
            ) : base(loginUserRepository)
        {
            //_announcementService = announcementService;
        }

        //[HttpGet]
        //public async Task<ActionResult<GetAnnouncementResponse>> GetAnnouncement()
        //{
        //    var result = await _announcementService.GetAnnouncement();

        //    return BuildResponse(result);
        //}

        //[HttpPut]
        //[AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        //public async Task<ActionResult<PutAnnouncementResponse>> PutAnnouncement(PutAnnouncementRequest request)
        //{
        //    var result = await _announcementService.PutAnnouncement(request);

        //    return BuildResponse(result);
        //}
    }
}
