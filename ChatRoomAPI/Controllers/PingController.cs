using ChatRoomAPI.Models;
using ChatRoomWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ChatRoomAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : Controller
    {
        private readonly CryptoSettings _cryptoSettings;

        public PingController(IOptions<CryptoSettings> cryptoSettings)
        {
            _cryptoSettings = cryptoSettings.Value;
        }

        [HttpGet]
        [Route("/api/settings")]
        public IActionResult GetSettings()
        {
            return Ok(new { Response = $"Password is: {_cryptoSettings.JwtSigningKey}" });
        }

        [HttpGet]
        [Route("/api/ping")]
        public IActionResult Get()
        {
            return Ok(new { Response = "Pong" });
        }

        [HttpGet]
        [Route("/api/protectedping")]
        [Authorize]
        public async Task<IActionResult> GetProtectedPing()
        {
            return Ok(new { Response = "Protected Pong" });
        }
    }
}
