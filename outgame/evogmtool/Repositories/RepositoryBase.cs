using evogmtool.Models;
using Microsoft.Extensions.Options;

namespace evogmtool.Repositories
{
    public class RepositoryBase
    {
        protected readonly string ConnectionString;

        // todo: delete
        public RepositoryBase(IOptions<AppSettings> optionsAccessor)
        {
            //ConnectionString = optionsAccessor.Value.RdbConnections.ConnectionStrings;
        }
    }
}
