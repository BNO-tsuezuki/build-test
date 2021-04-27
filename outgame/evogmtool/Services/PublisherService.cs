using System.Collections.Generic;
using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Repositories;

namespace evogmtool.Services
{
    public interface IPublisherService
    {
        Task<IEnumerable<Publisher>> GetPublisherList();
    }

    public class PublisherService : IPublisherService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PublisherService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Publisher>> GetPublisherList()
        {
            var publisherList = await _unitOfWork.PublisherRepository.GetAll();

            return publisherList;
        }
    }
}
