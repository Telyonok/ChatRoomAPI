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
    }
}
