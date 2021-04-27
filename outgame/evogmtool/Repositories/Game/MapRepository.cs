using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Utils;
using Microsoft.Extensions.Options;

namespace evogmtool.Repositories.Game
{
    public interface IMapRepository
    {
    }

    public class MapRepository : RepositoryBase, IMapRepository
    {
        public MapRepository(IOptions<AppSettings> optionsAccessor, EvoToolClient evoToolClient)
            : base(optionsAccessor, evoToolClient)
        { }
    }
}
