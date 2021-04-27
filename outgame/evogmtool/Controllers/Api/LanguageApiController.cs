using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using evogmtool.Attributes;
using evogmtool.Models;
using evogmtool.Models.LanguageApi;
using evogmtool.Repositories;
using evogmtool.Services;
using Microsoft.AspNetCore.Mvc;
using static evogmtool.Constants;

namespace evogmtool.Controllers.Api
{
    [Route("api/language")]
    [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator, Role.Watcher)]
    public class LanguageApiController : ApiControllerBase
    {
        private readonly ILanguageService _languageService;

        public LanguageApiController(
            IMapper mapper,
            ILanguageService languageService,
            ILoginUserRepository loginUserRepository
            ) : base(mapper, loginUserRepository)
        {
            _languageService = languageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetLanguageResponseDto>>> Get()
        {
            var languageList = await _languageService.GetLanguageList();

            return Ok(_mapper.Map<IEnumerable<Language>, IEnumerable<GetLanguageResponseDto>>(languageList));
        }
    }
}
