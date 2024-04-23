using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Back_Auth_Jwt.Controllers
{
    [Authorize(Roles ="User,Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("Get-Message")]
        public async Task<IActionResult> GetMessage()
        {
            return Ok(new { data = "Bonjour L\'utilisateur"});
        }
    }
}
