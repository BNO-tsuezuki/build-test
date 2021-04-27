using System.Collections.Generic;
using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Repositories;

namespace evogmtool.Services
{
    public interface ITimezoneService
    {
        Task<IEnumerable<Timezone>> GetTimezoneList();
    }

    public class TimezoneService : ITimezoneService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TimezoneService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Timezone>> GetTimezoneList()
        {
            var timezoneList = await _unitOfWork.TimezoneRepository.GetAll();

            return timezoneList;
        }
    }
}
