using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Back_Auth_Jwt.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase {
        
        [HttpGet("Get-Message")]
        public async Task<IActionResult> GetMessage()
        {
            return Ok(new {data =  "Bonjour L\'admin " });
        }
    }
}
