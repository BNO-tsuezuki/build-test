using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using evogmtool.Models;
using evogmtool.Repositories;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace evogmtool.Services
{
    public interface IAuthService
    {
        Task<bool> Authenticate(string account, string password);

        Task ChangePassword(User user, string password);

        Task<string> ResetPassword(User user);

        (string, string, string) GeneratePassword();
    }

    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Authenticate(string account, string password)
        {
            var user = (await _unitOfWork.UserRepository.Find(r => r.Account == account)).FirstOrDefault();

            if (user == null)
            {
                return false;
            }

            var passwordHash = GetPasswordHash(password, user.Salt);

            return user.PasswordHash == passwordHash;
        }

        public async Task ChangePassword(User user, string password)
        {
            user.PasswordHash = GetPasswordHash(password, user.Salt);

            await _unitOfWork.CommitAsync();
        }

        public async Task<string> ResetPassword(User user)
        {
            var newPassword = GenerateRandomPassword();

            user.PasswordHash = GetPasswordHash(newPassword, user.Salt);

            await _unitOfWork.CommitAsync();

            return newPassword;
        }

        public (string, string, string) GeneratePassword()
        {
            var password = GenerateRandomPassword();
            var salt = GenerateSalt();
            var passwordHash = GetPasswordHash(password, salt);

            return (password, salt, passwordHash);
        }

        private string GenerateRandomPassword()
        {
            // todo: implement random password generator
            return "password";
        }

        private string GenerateSalt()
        {
            var randomBytes = new byte[128 / 8];

            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomBytes);
            }

            return BitConverter.ToString(randomBytes).Replace("-", string.Empty);
        }

        private string GetPasswordHash(string password, string saltString)
        {
            var salt = Encoding.ASCII.GetBytes(saltString);

            //_logger.LogDebug($"Salt: {BitConverter.ToString(salt).Replace("-", string.Empty)}");

            var hash = BitConverter.ToString(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA512,
                    iterationCount: 10000,
                    numBytesRequested: 512 / 8
                )).Replace("-", string.Empty);

            //_logger.LogDebug($"Hash: {hash}");

            return hash;
        }
    }
}
