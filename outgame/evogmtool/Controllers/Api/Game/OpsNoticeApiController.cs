using System.Threading.Tasks;
using AutoMapper;
using evogmtool.Attributes;
using evogmtool.Models;
using evogmtool.Repositories;
using evogmtool.Services.Game;
using evotool.ProtocolModels.GMTool.OpsNoticeApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static evogmtool.Constants;

namespace evogmtool.Controllers.Api.Game
{
    [Route("api/game/opsnotice")]
    public class OpsNoticeApiController : GameApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IOpsNoticeService _opsNoticeService;

        public OpsNoticeApiController(
            IMapper mapper,
            IAuthorizationService authorizationService,
            IOpsNoticeService opsNoticeService,
            ILoginUserRepository loginUserRepository
            ) : base(loginUserRepository)
        {
            _mapper = mapper;
            _authorizationService = authorizationService;
            _opsNoticeService = opsNoticeService;
        }

        [HttpGet("chat")]
        public async Task<ActionResult<GetChatListResponse>> GetChatList([FromQuery]GetChatListRequest request)
        {
            var result = await _opsNoticeService.GetChatList(request);

            return BuildResponse(result);
        }

        [HttpGet("chat/{opsNoticeId}")]
        public async Task<ActionResult<GetChatResponse>> GetChat(long opsNoticeId)
        {
            var result = await _opsNoticeService.GetChat(opsNoticeId);

            return BuildResponse(result);
        }

        [HttpPost("chat")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PostChatResponse>> PostChat(PostChatRequest request)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, request.target, Policy.DomainRegionTarget);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var result = await _opsNoticeService.PostChat(request);

            return BuildResponse(result);
        }

        [HttpPut("chat/{opsNoticeId}")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PutChatResponse>> PutChat(long opsNoticeId, PutChatRequest request)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, request.target, Policy.DomainRegionTarget);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var registered = await _opsNoticeService.GetChat(opsNoticeId);

            if (registered.StatusCode != StatusCodes.Status200OK)
            {
                return BuildResponse(registered);
            }

            var registeredTarget = Newtonsoft.Json.JsonConvert.DeserializeObject<GetChatResponse>(registered.Content.ToString()).Target;

            authorizationResult = await _authorizationService.AuthorizeAsync(User, registeredTarget, Policy.DomainRegionTarget);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var result = await _opsNoticeService.PutChat(opsNoticeId, request);

            return BuildResponse(result);
        }

        [HttpDelete("chat/{opsNoticeId}")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult> DeleteChat(long opsNoticeId)
        {
            var registered = await _opsNoticeService.GetChat(opsNoticeId);

            if (registered.StatusCode != StatusCodes.Status200OK)
            {
                return BuildResponse(registered);
            }

            var registeredTarget = Newtonsoft.Json.JsonConvert.DeserializeObject<GetChatResponse>(registered.Content.ToString()).Target;

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, registeredTarget, Policy.DomainRegionTarget);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var result = await _opsNoticeService.DeleteChat(opsNoticeId);

            return BuildResponse(result);
        }

        [HttpGet("popup")]
        public async Task<ActionResult<GetPopupListResponse>> GetPopupList([FromQuery]GetPopupListRequest request)
        {
            var result = await _opsNoticeService.GetPopupList(request);

            return BuildResponse(result);
        }

        [HttpGet("popup/{opsNoticeId}")]
        public async Task<ActionResult<GetPopupResponse>> GetPopup(long opsNoticeId)
        {
            var result = await _opsNoticeService.GetPopup(opsNoticeId);

            return BuildResponse(result);
        }

        [HttpPost("popup")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PostPopupResponse>> PostPopup(PostPopupRequest request)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, request.target, Policy.DomainRegionTarget);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var result = await _opsNoticeService.PostPopup(request);

            return BuildResponse(result);
        }

        [HttpPut("popup/{opsNoticeId}")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult<PutPopupResponse>> PutPopup(long opsNoticeId, PutPopupRequest request)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, request.target, Policy.DomainRegionTarget);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var registered = await _opsNoticeService.GetPopup(opsNoticeId);

            if (registered.StatusCode != StatusCodes.Status200OK)
            {
                return BuildResponse(registered);
            }

            var registeredTarget = Newtonsoft.Json.JsonConvert.DeserializeObject<GetPopupResponse>(registered.Content.ToString()).Target;

            authorizationResult = await _authorizationService.AuthorizeAsync(User, registeredTarget, Policy.DomainRegionTarget);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var result = await _opsNoticeService.PutPopup(opsNoticeId, request);

            return BuildResponse(result);
        }

        [HttpDelete("popup/{opsNoticeId}")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator)]
        public async Task<ActionResult> DeletePopup(long opsNoticeId)
        {
            var registered = await _opsNoticeService.GetPopup(opsNoticeId);

            if (registered.StatusCode != StatusCodes.Status200OK)
            {
                return BuildResponse(registered);
            }

            var registeredTarget = Newtonsoft.Json.JsonConvert.DeserializeObject<GetPopupResponse>(registered.Content.ToString()).Target;

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, registeredTarget, Policy.DomainRegionTarget);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var result = await _opsNoticeService.DeletePopup(opsNoticeId);

            return BuildResponse(result);
        }
    }
}
