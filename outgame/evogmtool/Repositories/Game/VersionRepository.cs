using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Utils;
using evotool.ProtocolModels.GMTool.VersionApi;
using Microsoft.Extensions.Options;

namespace evogmtool.Repositories.Game
{
    public interface IVersionRepository
    {
        Task<EvoToolApiResponse> GetVersion();
        Task<EvoToolApiResponse> PutVersion(PutVersionRequest request);
    }

    public class VersionRepository : RepositoryBase, IVersionRepository
    {
        public VersionRepository(IOptions<AppSettings> optionsAccessor, EvoToolClient evoToolClient)
            : base(optionsAccessor, evoToolClient)
        { }

        public async Task<EvoToolApiResponse> GetVersion()
        {
            var response = await GetAsync($"/api/gmtool/version");

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> PutVersion(PutVersionRequest request)
        {
            var response = await PutAsync($"/api/gmtool/version", request);

            return await BuildResponse(response);
        }
    }
}
