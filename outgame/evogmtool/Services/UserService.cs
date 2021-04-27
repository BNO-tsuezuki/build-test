using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Repositories;

namespace evogmtool.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUserList(int? publisherId);
        Task<User> GetUserByUserId(int userId);
        Task<User> GetUserByAccount(string account);

        Task<(int, string)> RegisterUser(User user);

        Task UpdateUserName(User user, string name);
        Task UpdateUserRole(User user, string role);
        Task UpdateUserPublisher(User user, int publisherId);
        Task UpdateUserTimezone(User user, string timezoneCode);
        Task UpdateUserLanguage(User user, string languageCode);
        Task UpdateUserIsAvailable(User user, bool isAvailable);
    }

    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IAuthService _authService;

        public UserService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;

            _authService = authService;
        }

        public async Task<IEnumerable<User>> GetUserList(int? publisherId)
        {
            var userList = publisherId.HasValue
                ? await _unitOfWork.UserRepository.Find(r => r.PublisherId == publisherId.Value)
                : await _unitOfWork.UserRepository.GetAll();

            return userList;
        }

        public async Task<User> GetUserByUserId(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetById(userId);

            return user;
        }

        public async Task<User> GetUserByAccount(string account)
        {
            var user = (await _unitOfWork.UserRepository.Find(r => r.Account == account)).FirstOrDefault();

            return user;
        }

        public async Task<(int, string)> RegisterUser(User user)
        {
            (var password, var salt, var passwordHash) = _authService.GeneratePassword();

            user.Salt = salt;
            user.PasswordHash = passwordHash;
            user.IsAvailable = true;

            _unitOfWork.UserRepository.Add(user);

            await _unitOfWork.CommitAsync();

            return (user.UserId, password);
        }

        public async Task UpdateUserName(User user, string name)
        {
            user.Name = name;

            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateUserRole(User user, string role)
        {
            user.Role = role;

            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateUserPublisher(User user, int publisherId)
        {
            user.PublisherId = publisherId;

            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateUserTimezone(User user, string timezoneCode)
        {
            user.TimezoneCode = timezoneCode;

            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateUserLanguage(User user, string languageCode)
        {
            user.LanguageCode = languageCode;

            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateUserIsAvailable(User user, bool isAvailable)
        {
            user.IsAvailable = isAvailable;

            await _unitOfWork.CommitAsync();
        }
    }
}
