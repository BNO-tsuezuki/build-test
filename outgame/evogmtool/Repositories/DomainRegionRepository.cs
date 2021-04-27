using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using evogmtool.Models;
using Microsoft.EntityFrameworkCore;

namespace evogmtool.Repositories
{
    public interface IDomainRegionRepository
    {
        Task<IEnumerable<DomainRegion>> GetAll();
    }

    public class DomainRegionRepository : IDomainRegionRepository
    {
        private readonly GmToolDbContext _dbContext;

        public DomainRegionRepository(GmToolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DomainRegion>> GetAll()
        {
            // todo: sort?
            return await _dbContext.DomainRegions
                .Include(r => r.Domain)
                .Include(r => r.Region)
                .Include(r => r.Publisher)
                .Include(r => r.DomainRegionLanguages)
                    .ThenInclude(r => r.Language)
                //.OrderBy(r => new { r.DomainId, r.RegionId })
                .ToListAsync();
        }
    }
}
