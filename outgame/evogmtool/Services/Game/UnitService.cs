using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Repositories.Game;
using evotool.ProtocolModels.GMTool.UnitApi;

namespace evogmtool.Services.Game
{
    public interface IUnitService
    {
        Task<EvoToolApiResponse> GetUnit();
        Task<EvoToolApiResponse> PutUnitTemporaryAvailability(string mobileSuitId, PutUnitTemporaryAvailabilityRequest request);
    }

    public class UnitService : IUnitService
    {
        private readonly IUnitRepository _unitRepository;

        public UnitService(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public async Task<EvoToolApiResponse> GetUnit()
        {
            return await _unitRepository.GetUnit();
        }

        public async Task<EvoToolApiResponse> PutUnitTemporaryAvailability(string mobileSuitId, PutUnitTemporaryAvailabilityRequest request)
        {
            return await _unitRepository.PutUnitTemporaryAvailability(mobileSuitId, request);
        }
    }
}
