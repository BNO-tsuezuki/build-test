using AutoMapper;
using evotool.Filters;
using Microsoft.AspNetCore.Mvc;

namespace evotool.Controllers.GMTool
{
    [ApiController]
    [StatusCodeExceptionFilter]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMapper _mapper;

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
