using ChatRoomAPI.Models;
using ChatRoomAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

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
            try
            {
                await _usersService.InsertUserAsync(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpGet]
        [Route("/api/VerifyEmail/{verificationData}")]
        public async Task<IActionResult> VerifyEmail(string verificationData)
        {
            var userId = await _usersService.GetUserIdByVerificationAsync(verificationData);
            
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

        [HttpGet]
        [Route("/api/IsUniqueEmail/{email}")]
        public async Task<IActionResult> IsUniqueEmailAsync (string email)
        {
            return Ok(await _usersService.IsUniqueEmailAsync(email));
        }
        
        [HttpGet]
        [Route("/api/IsUniqueUsername/{username}")]
        public async Task<IActionResult> IsUniqueUsernameAsync (string username)
        {
            return Ok(await _usersService.IsUniqueUsernameAsync(username));
        }
    }
}
