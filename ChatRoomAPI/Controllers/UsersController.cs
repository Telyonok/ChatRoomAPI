using ChatRoomAPI.Models;
using ChatRoomAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoomAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost]
        [Route("/api/signup")]
        public async Task<IActionResult> SignUpPostAsync(User user)
        {
            await _usersService.InsertUserAsync(user);
            return NoContent();
        }

        [HttpGet]
        [Route("/api/VerifyEmail/{verificationData}")]
        public async Task<IActionResult> VerifyEmail(string verificationData)
        {
            var userId = await _usersService.GetUserIdByVerification(verificationData);
            
            var verifyEmailResponse = new VerifyEmailResponse
            {
                UserId = userId
            };

            if (userId < 0)
            {
                return NotFound(verifyEmailResponse);
            }
            
            return Ok(verifyEmailResponse);
        }
    }
}
