using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Utils;
using Microsoft.Extensions.Options;

namespace evogmtool.Repositories.Game
{
    public interface IGashaRepository
    {
    }

    public class GashaRepository : RepositoryBase, IGashaRepository
    {
        public GashaRepository(IOptions<AppSettings> optionsAccessor, EvoToolClient evoToolClient)
            : base(optionsAccessor, evoToolClient)
        { }
    }
}
