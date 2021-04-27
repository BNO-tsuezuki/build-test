using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using evogmtool.Models;
using Microsoft.EntityFrameworkCore;

namespace evogmtool.Repositories
{
    public interface IPublisherRepository
    {
        Task<IEnumerable<Publisher>> GetAll();
    }

    public class PublisherRepository : IPublisherRepository
    {
        private readonly GmToolDbContext _dbContext;

        public PublisherRepository(GmToolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Publisher>> GetAll()
        {
            return await _dbContext.Publishers
                .OrderBy(r => r.PublisherId)
                .ToListAsync();
        }
    }
}
