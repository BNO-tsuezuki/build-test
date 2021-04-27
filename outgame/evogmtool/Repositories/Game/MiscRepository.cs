using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Utils;
using Microsoft.Extensions.Options;

namespace evogmtool.Repositories.Game
{
    public interface IMiscRepository
    {
        Task<EvoToolApiResponse> GetSeasonList();
        Task<EvoToolApiResponse> GetMobileSuitList();
    }

    public class MiscRepository : RepositoryBase, IMiscRepository
    {
        public MiscRepository(IOptions<AppSettings> optionsAccessor, EvoToolClient evoToolClient)
            : base(optionsAccessor, evoToolClient)
        { }

        public async Task<EvoToolApiResponse> GetSeasonList()
        {
            var response = await GetAsync($"/api/gmtool/misc/seasonlist");

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> GetMobileSuitList()
        {
            var response = await GetAsync($"/api/gmtool/misc/mobilesuitlist");

            return await BuildResponse(response);
        }
    }
}
