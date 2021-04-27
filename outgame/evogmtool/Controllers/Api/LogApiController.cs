using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using evogmtool.Attributes;
using evogmtool.Models;
using evogmtool.Models.LogApi;
using evogmtool.Repositories;
using evogmtool.Services;
using Microsoft.AspNetCore.Mvc;
using static evogmtool.Constants;

namespace evogmtool.Controllers.Api
{
    [Route("api/log")]
    [AuthorizeByAnyRole(Role.Super, Role.Administrator)]
    public class LogApiController : ApiControllerBase
    {
        private readonly ILogService _logService;

        public LogApiController(
            IMapper mapper,
            ILogService logService,
            ILoginUserRepository loginUserRepository
            ) : base(mapper, loginUserRepository)
        {
            _logService = logService;
        }

        [HttpGet("auth")]
        public async Task<ActionResult<GetAuthLogListResponseDto>> GetAuthLogList([FromQuery]GetAuthLogListRequestDto requestDto)
        {
            var result = await _logService.GetAuthLogList(
                requestDto.From,
                requestDto.To,
                requestDto.Account,
                requestDto.Result,
                requestDto.IpAddress);

            return Ok(_mapper.Map<(IEnumerable<AuthLog>, int), GetAuthLogListResponseDto>(result));
        }

        [HttpGet("auth/{logId}")]
        public async Task<ActionResult<GetAuthLogResponseDto>> GetAuthLog(int logId)
        {
            var log = await _logService.GetAuthLog(logId);

            return Ok(_mapper.Map<AuthLog, GetAuthLogResponseDto>(log));
        }

        [HttpGet("operation")]
        public async Task<ActionResult<GetOperationLogListResponseDto>> GetOperationLogList([FromQuery]GetOperationLogListRequestDto requestDto)
        {
            var result = await _logService.GetOperationLogList(
                requestDto.From,
                requestDto.To,
                requestDto.UserId,
                requestDto.StatusCode,
                requestDto.Method,
                requestDto.Url,
                requestDto.QueryString,
                requestDto.RequestBody,
                requestDto.ResponseBody,
                requestDto.Exception,
                requestDto.IpAddress);

            return Ok(_mapper.Map<(IEnumerable<OperationLog>, int), GetOperationLogListResponseDto>(result));
        }

        [HttpGet("operation/{logId}")]
        public async Task<ActionResult<GetOperationLogResponseDto>> GetOperationLog(int logId)
        {
            var log = await _logService.GetOperationLog(logId);

            return Ok(_mapper.Map<OperationLog, GetOperationLogResponseDto>(log));
        }
    }
}
