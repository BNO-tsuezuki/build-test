using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using evogmtool.Models;
using evogmtool.Models.AuthApi;
using evogmtool.Repositories;
using evogmtool.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static evogmtool.Constants;
using static evogmtool.Enums;

namespace evogmtool.Controllers.Api
{
    [Route("api/auth")]
    [Authorize]
    public class AuthApiController : ApiControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        private readonly IAuthLoggerRepository _authLogger;

        public AuthApiController(
            IMapper mapper,
            IAuthService authService,
            IUserService userService,
            IAuthLoggerRepository authLoggerRepository,
            ILoginUserRepository loginUserRepository
            ) : base(mapper, loginUserRepository)
        {
            _authService = authService;
            _userService = userService;

            _authLogger = authLoggerRepository;
        }

        [HttpGet]
        public async Task<ActionResult<GetAuthResponseDto>> Get()
        {
            var user = await _userService.GetUserByUserId(loginUserId.Value);

            return Ok(_mapper.Map<User, GetAuthResponseDto>(user));
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(PutAuthPasswordChangeRequestDto passwordChangeRequestDto)
        {
            var credencials = _mapper.Map<PutAuthPasswordChangeRequestDto, PasswordChangeCredencials>(passwordChangeRequestDto);

            var registeredUser = await _userService.GetUserByUserId(loginUserId.Value);

            var isAuthenticated = await _authService.Authenticate(registeredUser.Account, credencials.Password);

            if (!isAuthenticated)
            {
                return BadRequest();
            }

            await _authService.ChangePassword(registeredUser, credencials.NewPassword);

            return Ok();
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<PostAuthLoginResponseDto>> Login(PostAuthLoginRequestDto loginRequestDto)
        {
            // todo: lockout ?
            // todo: 2fa ?

            var credencials = _mapper.Map<PostAuthLoginRequestDto, LoginCredencials>(loginRequestDto);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var user = await _userService.GetUserByAccount(credencials.Account);

            if (user == null)
            {
                _authLogger.Log(credencials.Account, AuthResult.InvalidAccount);
                return Unauthorized();
            }

            if (!user.IsAvailable)
            {
                // todo: status code
                // todo: error message
                return Forbid();
            }

            var isAuthenticated = await _authService.Authenticate(credencials.Account, credencials.Password);

            if (!isAuthenticated)
            {
                _authLogger.Log(credencials.Account, AuthResult.InvalidPassword);
                return Unauthorized();
            }

            // HACK: 初期パスワードの変更を要求するならここで判定する

            if (!Role.IsDefined(user.Role))
            {
                _authLogger.Log(credencials.Account, AuthResult.Forbidden);
                return Forbid();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(MyClaimTypes.PublisherId, user.PublisherId.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                // Refreshing the authentication session should be allowed.

                ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. Required when setting the 
                // ExpireTimeSpan option of CookieAuthenticationOptions 
                // set with AddCookie. Also required when setting 
                // ExpiresUtc.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            _authLogger.Log(credencials.Account, AuthResult.Success);

            var response = new PostAuthLoginResponseDto
            {
                ReturnUrl = Url.IsLocalUrl(loginRequestDto.ReturnUrl)
                    ? loginRequestDto.ReturnUrl
                    : string.Empty,
            };

            return Ok(response);
        }

        [HttpPost("Logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }
    }
}
