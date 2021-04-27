using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using evogmtool.Models;
using Microsoft.EntityFrameworkCore;

namespace evogmtool.Repositories
{
    public interface ILanguageRepository
    {
        Task<IEnumerable<Language>> GetAll();
    }

    public class LanguageRepository : ILanguageRepository
    {
        private readonly GmToolDbContext _dbContext;

        public LanguageRepository(GmToolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Language>> GetAll()
        {
            return await _dbContext.Languages
                .OrderBy(c => c.LanguageCode)
                .ToListAsync();
        }
    }
}
