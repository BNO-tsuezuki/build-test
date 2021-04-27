using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Repositories.Game;
//using evotool.ProtocolModels.GMTool.AnnouncementApi;

namespace evogmtool.Services.Game
{
    public interface IAnnouncementService
    {
        //Task<EvoToolApiResponse> GetAnnouncement();
        //Task<EvoToolApiResponse> PutAnnouncement(PutAnnouncementRequest request);
    }

    public class AnnouncementService : IAnnouncementService
    {
        //private readonly IAnnouncementRepository _announcementRepository;

        public AnnouncementService(IAnnouncementRepository announcementRepository)
        {
            //_announcementRepository = announcementRepository;
        }

        //public async Task<EvoToolApiResponse> GetAnnouncement()
        //{
        //    return await _announcementRepository.GetAnnouncement();
        //}

        //public async Task<EvoToolApiResponse> PutAnnouncement(PutAnnouncementRequest request)
        //{
        //    return await _announcementRepository.PutAnnouncement(request);
        //}
    }
}
