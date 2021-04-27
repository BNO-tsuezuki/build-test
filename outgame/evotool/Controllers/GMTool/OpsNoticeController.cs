using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using evolib.Databases.common1;
using evotool.ProtocolModels.GMTool.OpsNoticeApi;
using evotool.ProtocolModels.OpsNotice;
using evotool.Services.GMTool;
using Microsoft.AspNetCore.Mvc;

namespace evotool.Controllers.GMTool
{
    [Route("api/gmtool/[controller]")]
    public class OpsNoticeController : BaseController
    {
        private readonly IOpsNoticeService _opsNoticeService;

        public OpsNoticeController(
            IMapper mapper,
            IOpsNoticeService opsNoticeService
        ) : base(mapper)
        {
            _opsNoticeService = opsNoticeService;
        }

        // todo: 全体的にCBT1向けの実装なのであとで諸々考慮した実装にする

        [HttpGet("chat")]
        public async Task<ActionResult<GetChatListResponse>> GetChatList([FromQuery]GetChatListRequest request)
        {
            var result = await _opsNoticeService.GetChatList(request);

            var response = _mapper.Map<(IList<OpsNotice>, int), GetChatListResponse>(result);

            return Ok(response);
        }

        [HttpGet("chat/{opsNoticeId}")]
        public async Task<ActionResult<GetChatResponse>> GetChat(long opsNoticeId)
        {
            var result = await _opsNoticeService.GetChat(opsNoticeId);

            var response = _mapper.Map<OpsNotice, GetChatResponse>(result);

            return Ok(response);
        }

        [HttpPost("chat")]
        public async Task<ActionResult<PostChatResponse>> PostChat(PostChatRequest request)
        {
            // todo: CBT後 validation text
            // todo: validation enabledなtextは空文字列不可

            var chatDesc = _mapper.Map<PostChatRequest, ChatDesc>(request);

            var result = await _opsNoticeService.RegisterChat(chatDesc);

            var response = _mapper.Map<OpsNotice, PostChatResponse>(result);

            return Ok(response);
        }

        [HttpPut("chat/{opsNoticeId}")]
        public async Task<ActionResult<PutChatResponse>> PutChat(long opsNoticeId, PutChatRequest request)
        {
            // todo: CBT後 validation text
            // todo: validation enabledなtextは空文字列不可

            var chatDesc = _mapper.Map<PutChatRequest, ChatDesc>(request);

            var result = await _opsNoticeService.UpdateChat(opsNoticeId, chatDesc);

            var response = _mapper.Map<OpsNotice, PutChatResponse>(result);

            return Ok(response);
        }

        [HttpDelete("chat/{opsNoticeId}")]
        public async Task<IActionResult> DeleteChat(long opsNoticeId)
        {
            await _opsNoticeService.DeleteChat(opsNoticeId);

            return Ok();
        }


        [HttpGet("popup")]
        public async Task<ActionResult<GetPopupListResponse>> GetPopupList([FromQuery]GetPopupListRequest request)
        {
            var result = await _opsNoticeService.GetPopupList(request);

            var response = _mapper.Map<(IList<OpsNotice>, int), GetPopupListResponse>(result);

            return Ok(response);
        }

        [HttpGet("popup/{opsNoticeId}")]
        public async Task<ActionResult<GetPopupResponse>> GetPopup(long opsNoticeId)
        {
            var result = await _opsNoticeService.GetPopup(opsNoticeId);

            var response = _mapper.Map<OpsNotice, GetPopupResponse>(result);

            return Ok(response);
        }

        [HttpPost("popup")]
        public async Task<ActionResult<PostPopupResponse>> PostPopup(PostPopupRequest request)
        {
            // todo: CBT後 validation text
            // todo: validation enabledなtext/titleは空文字列不可

            var PopupDesc = _mapper.Map<PostPopupRequest, PopupDesc>(request);

            var result = await _opsNoticeService.RegisterPopup(PopupDesc);

            var response = _mapper.Map<OpsNotice, PostPopupResponse>(result);

            return Ok(response);
        }

        [HttpPut("popup/{opsNoticeId}")]
        public async Task<ActionResult<PutPopupResponse>> PutPopup(long opsNoticeId, PutPopupRequest request)
        {
            // todo: CBT後 validation text
            // todo: validation enabledなtext/titleは空文字列不可

            var PopupDesc = _mapper.Map<PutPopupRequest, PopupDesc>(request);

            var result = await _opsNoticeService.UpdatePopup(opsNoticeId, PopupDesc);

            var response = _mapper.Map<OpsNotice, PutPopupResponse>(result);

            return Ok(response);
        }

        [HttpDelete("popup/{opsNoticeId}")]
        public async Task<IActionResult> DeletePopup(long opsNoticeId)
        {
            await _opsNoticeService.DeletePopup(opsNoticeId);

            return Ok();
        }
    }
}
