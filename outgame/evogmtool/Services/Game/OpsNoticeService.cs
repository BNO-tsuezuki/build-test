using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Repositories.Game;
using evotool.ProtocolModels.GMTool.OpsNoticeApi;

namespace evogmtool.Services.Game
{
    public interface IOpsNoticeService
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

    public class OpsNoticeService : IOpsNoticeService
    {
        private readonly IOpsNoticeRepository _opsNoticeRepository;

        public OpsNoticeService(IOpsNoticeRepository opsNoticeRepository)
        {
            _opsNoticeRepository = opsNoticeRepository;
        }

        public async Task<EvoToolApiResponse> GetChatList(GetChatListRequest request)
        {
            return await _opsNoticeRepository.GetChatList(request);
        }

        public async Task<EvoToolApiResponse> GetChat(long opsNoticeId)
        {
            return await _opsNoticeRepository.GetChat(opsNoticeId);
        }

        public async Task<EvoToolApiResponse> PostChat(PostChatRequest request)
        {
            return await _opsNoticeRepository.PostChat(request);
        }

        public async Task<EvoToolApiResponse> PutChat(long opsNoticeId, PutChatRequest request)
        {
            return await _opsNoticeRepository.PutChat(opsNoticeId, request);
        }

        public async Task<EvoToolApiResponse> DeleteChat(long opsNoticeId)
        {
            return await _opsNoticeRepository.DeleteChat(opsNoticeId);
        }

        public async Task<EvoToolApiResponse> GetPopupList(GetPopupListRequest request)
        {
            return await _opsNoticeRepository.GetPopupList(request);
        }

        public async Task<EvoToolApiResponse> GetPopup(long opsNoticeId)
        {
            return await _opsNoticeRepository.GetPopup(opsNoticeId);
        }

        public async Task<EvoToolApiResponse> PostPopup(PostPopupRequest request)
        {
            return await _opsNoticeRepository.PostPopup(request);
        }

        public async Task<EvoToolApiResponse> PutPopup(long opsNoticeId, PutPopupRequest request)
        {
            return await _opsNoticeRepository.PutPopup(opsNoticeId, request);
        }

        public async Task<EvoToolApiResponse> DeletePopup(long opsNoticeId)
        {
            return await _opsNoticeRepository.DeletePopup(opsNoticeId);
        }
    }
}
