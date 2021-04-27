using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using evogmtool.Models;
using Microsoft.EntityFrameworkCore;

namespace evogmtool.Repositories
{
    public interface ITimezoneRepository
    {
        Task<IEnumerable<Timezone>> GetAll();
    }

    public class TimezoneRepository : ITimezoneRepository
    {
        private readonly GmToolDbContext _dbContext;

        public TimezoneRepository(GmToolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Timezone>> GetAll()
        {
            return await _dbContext.Timezones
                .OrderBy(c => c.TimezoneCode)
                .ToListAsync();
        }
    }
}
