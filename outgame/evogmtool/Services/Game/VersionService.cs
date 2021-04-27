using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Repositories.Game;
using evotool.ProtocolModels.GMTool.VersionApi;

namespace evogmtool.Services.Game
{
    public interface IVersionService
    {
        Task<EvoToolApiResponse> GetVersion();
        Task<EvoToolApiResponse> PutVersion(PutVersionRequest request);
    }

    public class VersionService : IVersionService
    {
        private readonly IVersionRepository _versionRepository;

        public VersionService(IVersionRepository versionRepository)
        {
            _versionRepository = versionRepository;
        }

        public async Task<EvoToolApiResponse> GetVersion()
        {
            return await _versionRepository.GetVersion();
        }

        public async Task<EvoToolApiResponse> PutVersion(PutVersionRequest request)
        {
            return await _versionRepository.PutVersion(request);
        }
    }
}
