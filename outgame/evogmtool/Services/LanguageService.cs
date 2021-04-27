using System.Collections.Generic;
using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Repositories;

namespace evogmtool.Services
{
    public interface ILanguageService
    {
        Task<IEnumerable<Language>> GetLanguageList();
    }

    public class LanguageService : ILanguageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LanguageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Language>> GetLanguageList()
        {
            var languageList = await _unitOfWork.LanguageRepository.GetAll();

            return languageList;
        }
    }
}
