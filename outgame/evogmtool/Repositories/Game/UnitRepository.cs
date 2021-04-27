using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Utils;
using evotool.ProtocolModels.GMTool.UnitApi;
using Microsoft.Extensions.Options;

namespace evogmtool.Repositories.Game
{
    public interface IUnitRepository
    {
        Task<EvoToolApiResponse> GetUnit();
        Task<EvoToolApiResponse> PutUnitTemporaryAvailability(string mobileSuitId, PutUnitTemporaryAvailabilityRequest request);
    }

    public class UnitRepository : RepositoryBase, IUnitRepository
    {
        public UnitRepository(IOptions<AppSettings> optionsAccessor, EvoToolClient evoToolClient)
            : base(optionsAccessor, evoToolClient)
        { }

        public async Task<EvoToolApiResponse> GetUnit()
        {
            var response = await GetAsync($"/api/gmtool/unit", null);

            return await BuildResponse(response);
        }

        public async Task<EvoToolApiResponse> PutUnitTemporaryAvailability(string mobileSuitId, PutUnitTemporaryAvailabilityRequest request)
        {
            var response = await PutAsync($"/api/gmtool/unit/{mobileSuitId}/temporaryavailability", request);

            return await BuildResponse(response);
        }
    }
}
