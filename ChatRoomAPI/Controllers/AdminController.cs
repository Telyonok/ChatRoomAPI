using Microsoft.AspNetCore.Mvc;

namespace ChatRoomAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        [HttpGet]
        [Route("/api/admin")]
        public IActionResult Get()
        {
            return Ok("Admin was called");
        }
    }
}
