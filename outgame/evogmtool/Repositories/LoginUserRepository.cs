using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace evogmtool.Repositories
{
    public interface ILoginUserRepository
    {
        int? UserId { get; }
        string IpAddress { get; }
    }

    public class LoginUserRepository : ILoginUserRepository
    {
        private IHttpContextAccessor _httpContextAccessor;

        public LoginUserRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                var userIdString = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return int.TryParse(userIdString, out int userId) ? userId : (int?)null;
            }
        }

        public string IpAddress
        {
            get
            {
                return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
        }
    }
}
