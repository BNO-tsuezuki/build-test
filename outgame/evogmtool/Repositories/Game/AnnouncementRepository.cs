using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Utils;
//using evotool.ProtocolModels.GMTool.AnnouncementApi;
using Microsoft.Extensions.Options;

namespace evogmtool.Repositories.Game
{
    public interface IAnnouncementRepository
    {
        //Task<EvoToolApiResponse> GetAnnouncement();
        //Task<EvoToolApiResponse> PutAnnouncement(PutAnnouncementRequest request);
    }

    public class AnnouncementRepository : RepositoryBase, IAnnouncementRepository
    {
        public AnnouncementRepository(IOptions<AppSettings> optionsAccessor, EvoToolClient evoToolClient)
            : base(optionsAccessor, evoToolClient)
        { }

        //public async Task<EvoToolApiResponse> GetAnnouncement()
        //{
        //    var response = await GetAsync($"/api/gmtool/announcement");

        //    return await BuildResponse(response);
        //}

        //public async Task<EvoToolApiResponse> PutAnnouncement(PutAnnouncementRequest request)
        //{
        //    var response = await PutAsync($"/api/gmtool/announcement", request);

        //    return await BuildResponse(response);
        //}
    }
}
