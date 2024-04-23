using Back_Auth_Jwt.Dto;
using Back_Auth_Jwt.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Back_Auth_Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly IAuthentification _authentification;
        

        public AuthentificationController(IAuthentification authentification)
        {
            _authentification = authentification;
        }



        [HttpGet("users")]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(_authentification.GetUsers());
        }



        [HttpPost("ajoute-role")]
        public async Task<IActionResult> AddRole(string role)
        {
            bool isAdded = await _authentification.CreateRole(role);
            return isAdded ? Ok($"Role {role} est ajoute") :BadRequest($"Role {role} n'ajoute pas");
        }


        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(_authentification.GetRoles());
        }


        [HttpPost("ajoute-user")]
        public async Task<IActionResult> RegistreNewUser([FromBody] RegistreDto user)
        {
            bool isAded = await _authentification.CreateNewUser(user);
            return isAded 
                ? Ok(
                    new
                    {
                        Message = $"L\'utilisateur {user.UserName} est bien ajoute"
                    })
                : BadRequest(
                     new
                     {
                         Message = $"L'utilisateur {user.UserName} n\' ajoute pas "
                     });
        }
        [HttpGet("Get-Roles-Users")]
        public async Task<IActionResult> GetRolesOfUsers()
        {
            List<RolesOfUsers> RolesOfUsers = await _authentification.GetAllUserOfRoles();
            return Ok(RolesOfUsers);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            TokenLoginDto tokenLogin = await _authentification.LoginDto(login);
            
            if(!tokenLogin.IsAuthenticated){
                return NotFound(tokenLogin.Message);
            }
            tokenLogin.expires = tokenLogin.expires;
            JwtSecurityToken jwt = await _authentification.GetJwtSecurityToken(tokenLogin.User);

            return Ok(
                new{ 
                    token = new JwtSecurityTokenHandler().WriteToken(jwt), 
                    expires = jwt.ValidTo ,
                    roles = tokenLogin.roles ,
                    UserName = tokenLogin.User.UserName
                });
        }
    }
}
