using System.Threading.Tasks;
using evogmtool.Repositories.Game;

namespace evogmtool.Services.Game
{
    public interface IMapService
    {
    }

    public class MapService : IMapService
    {
        private readonly IMapRepository _mapRepository;

        public MapService(IMapRepository mapRepository)
        {
            _mapRepository = mapRepository;
        }
    }
}
