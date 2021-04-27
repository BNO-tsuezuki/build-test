using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using evogmtool.Attributes;
using evogmtool.Models;
using evogmtool.Models.UserApi;
using evogmtool.Repositories;
using evogmtool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static evogmtool.Constants;

namespace evogmtool.Controllers.Api
{
    [Route("api/user")]
    [AuthorizeByAnyRole(Role.Super, Role.Administrator, Role.Publisher, Role.Operator, Role.Watcher)]
    public class UserApiController : ApiControllerBase
    {
        // todo: antiforgerytoken

        private readonly IAuthorizationService _authorizationService;
        private readonly IAuthService _authService; // todo: IAuthorizationServiceと似てて紛らわしいからrenameしたい
        private readonly IUserService _userService;

        public UserApiController(
            IMapper mapper,
            IAuthorizationService authorizationService,
            IAuthService authService,
            IUserService userService,
            ILoginUserRepository loginUserRepository
            ) : base(mapper, loginUserRepository)
        {
            _authorizationService = authorizationService;
            _authService = authService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserResponseDto>>> Get([FromQuery]GetUserRequestDto userRequestDto)
        {
            // todo: role check ?

            var userList = await _userService.GetUserList(userRequestDto.PublisherId);

            return Ok(_mapper.Map<IEnumerable<User>, IEnumerable<GetUserResponseDto>>(userList));
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<GetUserResponseDto>> Get(int userId)
        {
            // todo: role check ?

            var registeredUser = await _userService.GetUserByUserId(userId);

            if (registeredUser == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<User, GetUserResponseDto>(registeredUser));
        }

        [HttpPost]
        public async Task<ActionResult<PostUserResponseDto>> Post(PostUserRequestDto userRequestDto)
        {
            var userRequest = _mapper.Map<PostUserRequestDto, User>(userRequestDto);

            // todo: validation
            // todo: validation publisherid

            var registeredUser = await _userService.GetUserByAccount(userRequest.Account);

            if (registeredUser != null)
            {
                return Conflict();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, userRequest, Policy.RoleLevel);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            (var id, var password) = await _userService.RegisterUser(userRequest);

            var createdUser = new PostUserResponseDto
            {
                UserId = id,
                Password = password,
            };

            return Created(string.Empty, createdUser);
        }

        [HttpPut("{userId}/name")]
        public async Task<IActionResult> PutUserName(int userId, PutUserNameRequestDto userNameRequestDto)
        {
            var userRequest = _mapper.Map<PutUserNameRequestDto, User>(userNameRequestDto);

            // todo: validation

            var registeredUser = await _userService.GetUserByUserId(userId);

            if (registeredUser == null)
            {
                return NotFound();
            }

            if (userId != loginUserId)
            {
                var authorizationResult = await _authorizationService.AuthorizeAsync(User, registeredUser, Policy.RoleLevel);

                if (!authorizationResult.Succeeded)
                {
                    return Forbid();
                }
            }

            if (!registeredUser.IsAvailable)
            {
                // todo: error message
                return Forbid();
            }

            await _userService.UpdateUserName(registeredUser, userRequest.Name);

            return Ok();
        }

        [HttpPut("{userId}/role")]
        public async Task<IActionResult> PutUserRole(int userId, PutUserRoleRequestDto userRoleRequestDto)
        {
            var userRequest = _mapper.Map<PutUserRoleRequestDto, User>(userRoleRequestDto);

            // todo: validation

            var registeredUser = await _userService.GetUserByUserId(userId);

            if (registeredUser == null)
            {
                return NotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, registeredUser, Policy.RoleLevel);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            authorizationResult = await _authorizationService.AuthorizeAsync(User, userRequest, Policy.RoleLevel);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            if (!registeredUser.IsAvailable)
            {
                // todo: error message
                return Forbid();
            }

            await _userService.UpdateUserRole(registeredUser, userRequest.Role);

            return Ok();
        }

        [HttpPut("{userId}/publisher")]
        [AuthorizeByAnyRole(Role.Super, Role.Administrator)]
        public async Task<IActionResult> PutUserPublisher(int userId, PutUserPublisherRequestDto userPublisherRequestDto)
        {
            var userRequest = _mapper.Map<PutUserPublisherRequestDto, User>(userPublisherRequestDto);

            // todo: validation
            // todo: validation publisherid

            var registeredUser = await _userService.GetUserByUserId(userId);

            if (registeredUser == null)
            {
                return NotFound();
            }

            if (userId != loginUserId)
            {
                var authorizationResult = await _authorizationService.AuthorizeAsync(User, registeredUser, Policy.RoleLevel);

                if (!authorizationResult.Succeeded)
                {
                    return Forbid();
                }
            }

            if (!registeredUser.IsAvailable)
            {
                // todo: error message
                return Forbid();
            }

            await _userService.UpdateUserPublisher(registeredUser, userRequest.PublisherId);

            return Ok();
        }

        [HttpPut("{userId}/timezone")]
        public async Task<IActionResult> PutUserTimezone(int userId, PutUserTimezoneRequestDto userTimezoneRequestDto)
        {
            // todo: 大文字小文字が一致していなくても外部キー制約にひっかからないので要確認

            var userRequest = _mapper.Map<PutUserTimezoneRequestDto, User>(userTimezoneRequestDto);

            // todo: validation
            // todo: validation timezoneCode

            var registeredUser = await _userService.GetUserByUserId(userId);

            if (registeredUser == null)
            {
                return NotFound();
            }

            if (userId != loginUserId)
            {
                var authorizationResult = await _authorizationService.AuthorizeAsync(User, registeredUser, Policy.RoleLevel);

                if (!authorizationResult.Succeeded)
                {
                    return Forbid();
                }
            }

            if (!registeredUser.IsAvailable)
            {
                // todo: error message
                return Forbid();
            }

            await _userService.UpdateUserTimezone(registeredUser, userRequest.TimezoneCode);

            return Ok();
        }

        [HttpPut("{userId}/language")]
        public async Task<IActionResult> PutUserLanguage(int userId, PutUserLanguageRequestDto userLanguageRequestDto)
        {
            // todo: 大文字小文字が一致していなくても外部キー制約にひっかからないので要確認

            var userRequest = _mapper.Map<PutUserLanguageRequestDto, User>(userLanguageRequestDto);

            // todo: validation
            // todo: validation languageCode

            var registeredUser = await _userService.GetUserByUserId(userId);

            if (registeredUser == null)
            {
                return NotFound();
            }

            if (userId != loginUserId)
            {
                var authorizationResult = await _authorizationService.AuthorizeAsync(User, registeredUser, Policy.RoleLevel);

                if (!authorizationResult.Succeeded)
                {
                    return Forbid();
                }
            }

            if (!registeredUser.IsAvailable)
            {
                // todo: error message
                return Forbid();
            }

            await _userService.UpdateUserLanguage(registeredUser, userRequest.LanguageCode);

            return Ok();
        }

        [HttpPut("{userId}/ResetPassword")]
        public async Task<ActionResult<PutUserPasswordResetResponseDto>> PasswordReset(int userId)
        {
            // todo: validation

            var registeredUser = await _userService.GetUserByUserId(userId);

            if (registeredUser == null)
            {
                return NotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, registeredUser, Policy.RoleLevel);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            if (!registeredUser.IsAvailable)
            {
                // todo: error message
                return Forbid();
            }

            var password = await _authService.ResetPassword(registeredUser);

            var response = new PutUserPasswordResetResponseDto
            {
                Password = password,
            };

            return Ok(response);
        }

        [HttpPut("{userId}/IsAvailable")]
        public async Task<IActionResult> PutUserIsAvailable(int userId, PutUserIsAvailableRequestDto userIsAvailableRequestDto)
        {
            var userRequest = _mapper.Map<PutUserIsAvailableRequestDto, User>(userIsAvailableRequestDto);

            // todo: validation

            var registeredUser = await _userService.GetUserByUserId(userId);

            if (registeredUser == null)
            {
                return NotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, registeredUser, Policy.RoleLevel);

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _userService.UpdateUserIsAvailable(registeredUser, userRequest.IsAvailable);

            return Ok();
        }
    }
}
