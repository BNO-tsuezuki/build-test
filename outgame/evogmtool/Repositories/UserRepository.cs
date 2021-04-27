using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using evogmtool.Models;
using Microsoft.EntityFrameworkCore;

namespace evogmtool.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetById(int userId);
        Task<IEnumerable<User>> GetAll();
        Task<IEnumerable<User>> Find(Expression<Func<User, bool>> predicate);
        void Add(User user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly GmToolDbContext _dbContext;

        public UserRepository(GmToolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetById(int userId)
        {
            return await _dbContext.Users
                .Include(user => user.Publisher)
                .Include(user => user.Timezone)
                .Include(user => user.Language)
                .SingleOrDefaultAsync(r => r.UserId == userId);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _dbContext.Users
                .Include(user => user.Publisher)
                .Include(user => user.Timezone)
                .Include(user => user.Language)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> Find(Expression<Func<User, bool>> predicate)
        {
            return await _dbContext.Users
                .Where(predicate)
                .Include(user => user.Publisher)
                .ToListAsync();
        }

        public void Add(User user)
        {
            _dbContext.Users.Add(user);
        }
    }
}
