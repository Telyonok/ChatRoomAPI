using ChatRoomAPI.Models;
using ChatRoomAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoomWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUsersService _userService;
        public AuthenticationController(IAuthenticationService authenticationService, IUsersService usersService)
        {
            _authenticationService = authenticationService;
            _userService = usersService;
        }

        [HttpPost]
        [Route("/api/token")]
        [AllowAnonymous]
        public async Task<TokenResponse> RequestTokenAsync(TokenRequest tokenRequest)
        {
            var token = await _authenticationService.RequestTokenAsync(tokenRequest);
            return token;
        }

        [HttpPost]
        [Route("/api/refreshToken")]
        [AllowAnonymous]
        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
        {
            var token = await _authenticationService.RefreshTokenAsync(refreshTokenRequest);
            return token;
        }
    }
}
