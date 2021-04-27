using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Repositories.Game;

namespace evogmtool.Services.Game
{
    public interface IMiscService
    {
        Task<EvoToolApiResponse> GetSeasonList();
        Task<EvoToolApiResponse> GetMobileSuitList();
    }

    public class MiscService : IMiscService
    {
        private readonly IMiscRepository _miscRepository;

        public MiscService(IMiscRepository miscRepository)
        {
            _miscRepository = miscRepository;
        }

        public async Task<EvoToolApiResponse> GetSeasonList()
        {
            return await _miscRepository.GetSeasonList();
        }

        public async Task<EvoToolApiResponse> GetMobileSuitList()
        {
            return await _miscRepository.GetMobileSuitList();
        }
    }
}
