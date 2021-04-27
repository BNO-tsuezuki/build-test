using System.Collections.Generic;
using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Repositories;

namespace evogmtool.Services
{
    public interface IDomainRegionService
    {
        Task<IEnumerable<DomainRegion>> GetDomainRegionList();
    }

    public class DomainRegionService : IDomainRegionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DomainRegionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DomainRegion>> GetDomainRegionList()
        {
            var domainRegionList = await _unitOfWork.DomainRegionRepository.GetAll();

            return domainRegionList;
        }
    }
}
