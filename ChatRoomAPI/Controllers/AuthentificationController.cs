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
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("/api/token")]
        [AllowAnonymous]
        public async Task<TokenResponse> RequestToken(TokenRequest tokenRequest)
        {
            var token = await _authenticationService.RequestTokenAsync(tokenRequest);
            return token;
        }

        [HttpPost]
        [Route("/api/refreshToken")]
        [AllowAnonymous]
        public async Task<TokenResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            var token = await _authenticationService.RefreshTokenAsync(refreshTokenRequest);
            return token;
        }
    }
}
