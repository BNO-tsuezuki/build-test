using System.Collections.Generic;
using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Utils;
using evotool.ProtocolModels.GMTool.OpsNoticeApi;
using Microsoft.Extensions.Options;

namespace evogmtool.Repositories.Game
{
    public interface IOpsNoticeRepository
    {
        Task<EvoToolApiResponse> GetChatList(GetChatListRequest request);
        Task<EvoToolApiResponse> GetChat(long opsNoticeId);
        Task<EvoToolApiResponse> PostChat(PostChatRequest request);
        Task<EvoToolApiResponse> PutChat(long opsNoticeId, PutChatRequest request);
        Task<EvoToolApiResponse> DeleteChat(long opsNoticeId);

        Task<EvoToolApiResponse> GetPopupList(GetPopupListRequest request);
        Task<EvoToolApiResponse> GetPopup(long opsNoticeId);
        Task<EvoToolApiResponse> PostPopup(PostPopupRequest request);
        Task<EvoToolApiResponse> PutPopup(long opsNoticeId, PutPopupRequest request);
        Task<EvoToolApiResponse> DeletePopup(long opsNoticeId);
    }

    public class OpsNoticeRepository : RepositoryBase, IOpsNoticeRepository
    {
        public OpsNoticeRepository(IOptions<AppSettings> optionsAccessor, EvoToolClient evoToolClient)
            : base(optionsAccessor, evoToolClient)
        { }

        public async Task<EvoToolApiResponse> GetChatList(GetChatListRequest request)
        {
            // todo: datetime format
            var parameters = new Dictionary<string, string>();
            parameters.Add("countPerPage", request.CountPerPage.ToString());
            parameters.Add("pageNumber", request.PageNumber.ToString());
            if (request.From.HasValue) parameters.Add("from", request.From.Value.ToString("o"));
            if (request.To.HasValue) parameters.Add("to", request.To.Value.ToString("o"));
            if (request.Target.HasValue) parameters.Add("target", request.Target.Value.ToString());

            var response = await GetAsync($"/api/gmtool/opsnotice/chat", parameters);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> GetChat(long opsNoticeId)
        {
            var response = await GetAsync($"/api/gmtool/opsnotice/chat/{opsNoticeId}");

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> PostChat(PostChatRequest request)
        {
            var response = await PostAsync($"/api/gmtool/opsnotice/chat", request);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> PutChat(long opsNoticeId, PutChatRequest request)
        {
            var response = await PutAsync($"/api/gmtool/opsnotice/chat/{opsNoticeId}", request);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> DeleteChat(long opsNoticeId)
        {
            var response = await DeleteAsync($"/api/gmtool/opsnotice/chat/{opsNoticeId}", null);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> GetPopupList(GetPopupListRequest request)
        {
            // todo: datetime format
            var parameters = new Dictionary<string, string>();
            parameters.Add("countPerPage", request.CountPerPage.ToString());
            parameters.Add("pageNumber", request.PageNumber.ToString());
            if (request.From.HasValue) parameters.Add("from", request.From.Value.ToString("o"));
            if (request.To.HasValue) parameters.Add("to", request.To.Value.ToString("o"));
            if (request.Target.HasValue) parameters.Add("target", request.Target.Value.ToString());

            var response = await GetAsync($"/api/gmtool/opsnotice/popup", parameters);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> GetPopup(long opsNoticeId)
        {
            var response = await GetAsync($"/api/gmtool/opsnotice/popup/{opsNoticeId}");

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> PostPopup(PostPopupRequest request)
        {
            var response = await PostAsync($"/api/gmtool/opsnotice/popup", request);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> PutPopup(long opsNoticeId, PutPopupRequest request)
        {
            var response = await PutAsync($"/api/gmtool/opsnotice/popup/{opsNoticeId}", request);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> DeletePopup(long opsNoticeId)
        {
            var response = await DeleteAsync($"/api/gmtool/opsnotice/popup/{opsNoticeId}", null);

            return await BuildResponse(response);
        }
    }
}
