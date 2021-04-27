using AutoMapper;
using evogmtool.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace evogmtool.Controllers.Api
{
    [ApiController]
    [SkipStatusCodePages]
    public class ApiControllerBase : ControllerBase
    {
        protected readonly IMapper _mapper;

        protected readonly ILoginUserRepository _loginUserRepository;

        protected readonly int? loginUserId;

        public ApiControllerBase(
            IMapper mapper,
            ILoginUserRepository loginUserRepository)
        {
            _mapper = mapper;

            _loginUserRepository = loginUserRepository;

            loginUserId = _loginUserRepository.UserId;
        }
    }
}
