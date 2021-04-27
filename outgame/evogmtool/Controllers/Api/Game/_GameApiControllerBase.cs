using evogmtool.Models;
using evogmtool.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace evogmtool.Controllers.Api.Game
{
    [ApiController]
    [SkipStatusCodePages]
    [Authorize]
    public class GameApiControllerBase : ControllerBase
    {
        protected readonly ILoginUserRepository _loginUserRepository;

        protected readonly int? loginUserId;

        public GameApiControllerBase(ILoginUserRepository loginUserRepository)
        {
            _loginUserRepository = loginUserRepository;

            loginUserId = _loginUserRepository.UserId;
        }

        protected ObjectResult BuildResponse(EvoToolApiResponse response)
        {
            return StatusCode(response.StatusCode, response.Content);
        }
    }
}
