using System.Threading.Tasks;
using evogmtool.Repositories.Game;

namespace evogmtool.Services.Game
{
    public interface IGashaService
    {
    }

    public class GashaService : IGashaService
    {
        private readonly IGashaRepository _gashaRepository;

        public GashaService(IGashaRepository gashaRepository)
        {
            _gashaRepository = gashaRepository;
        }
    }
}
