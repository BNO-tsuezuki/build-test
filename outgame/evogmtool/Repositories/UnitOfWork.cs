using System.Threading.Tasks;
using evogmtool.Models;

namespace evogmtool.Repositories
{
    public interface IUnitOfWork
    {
        IDomainRegionRepository DomainRegionRepository { get; }
        ILogRepository LogRepository { get; }
        IPublisherRepository PublisherRepository { get; }
        ITimezoneRepository TimezoneRepository { get; }
        ILanguageRepository LanguageRepository { get; }
        IUserRepository UserRepository { get; }

        Task CommitAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly GmToolDbContext _dbContext;

        private IDomainRegionRepository _domainRegionRepository;
        private ILogRepository _logRepository;
        private IPublisherRepository _publisherRepository;
        private ITimezoneRepository _timezoneRepository;
        private ILanguageRepository _languageRepository;
        private IUserRepository _userRepository;

        public UnitOfWork(GmToolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IDomainRegionRepository DomainRegionRepository => _domainRegionRepository = _domainRegionRepository ?? new DomainRegionRepository(_dbContext);
        public ILogRepository LogRepository => _logRepository = _logRepository ?? new LogRepository(_dbContext);
        public IPublisherRepository PublisherRepository => _publisherRepository = _publisherRepository ?? new PublisherRepository(_dbContext);
        public ITimezoneRepository TimezoneRepository => _timezoneRepository = _timezoneRepository ?? new TimezoneRepository(_dbContext);
        public ILanguageRepository LanguageRepository => _languageRepository = _languageRepository ?? new LanguageRepository(_dbContext);
        public IUserRepository UserRepository => _userRepository = _userRepository ?? new UserRepository(_dbContext);

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
