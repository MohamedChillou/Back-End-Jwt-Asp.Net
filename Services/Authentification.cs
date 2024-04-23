using Back_Auth_Jwt.Dto;
using Back_Auth_Jwt.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Back_Auth_Jwt.Services
{
    public class Authentification : IAuthentification
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Authentification(UserManager<AppUser> userManger,RoleManager<IdentityRole> roleManager,IConfiguration configuration)
        {
            _userManager = userManger;   
            _roleManager = roleManager;
            _configuration = configuration; 
        }

        public async Task<JwtSecurityToken> GetJwtSecurityToken(AppUser user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name,user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier,user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()));
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role,role)); 
            }
            SymmetricSecurityKey  key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            SigningCredentials signing = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer : _configuration["JWT:Issuer"],
                claims : claims,
                audience: _configuration["JWT:Audiance"],
                expires: DateTime.Now.AddHours(1),
                signingCredentials:signing
            );
        }

        public async Task<bool> CreateNewUser(RegistreDto registre)
        {
            AppUser user = new AppUser()
            {
                Email = registre.Email,
                UserName = registre.UserName,
            };
            var result = await _userManager.CreateAsync(user,registre.Password);
            
            if (result.Succeeded)
            {
                if (registre.Roles != null && await CheckedRole(registre.Roles))
                {
                    foreach (var role in registre.Roles)
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }
                }
                return true;
            }
            return false; 
        }

        public async Task<bool> CheckedRole(List<string> roles)
        {
            foreach(var role in roles)
            {
               if(! await _roleManager.RoleExistsAsync(role))
                    return false;
            }
            return true;
        }

        public async Task<bool> CreateRole(string role)
        {
            if (await _roleManager.RoleExistsAsync(role))
            {
                return true;
            }

            IdentityRole identityRole = new IdentityRole(role);
            var result = await _roleManager.CreateAsync(identityRole);
            return result.Succeeded ? true : false ;
        }

        public async Task<List<IdentityRole>> GetRoles()
        {
            var roles =  _roleManager.Roles.ToList();
            return roles;
        }

        public async Task<List<AppUser>> GetUsers() { 
            return _userManager.Users.ToList();
        }

        public async Task<TokenLoginDto> LoginDto(LoginDto login)
        {
            AppUser ? user = await _userManager.FindByNameAsync(login.UserName);

            if (user == null ||  !await _userManager.CheckPasswordAsync(user, login.Password))
            {
                return new TokenLoginDto { 
                  Message = $"L\'utilisateur {login.UserName} n\'existe pas ou bien le mot de passe est incorrecte "
                };

            }
            IList<string> roles = await _userManager.GetRolesAsync(user);
            return new TokenLoginDto
            {
                Message = "La verification ca passe bien",
                IsAuthenticated = true,
                User = user,
                roles = roles
            };
        }

        public async Task<List<RolesOfUsers>> GetAllUserOfRoles()
        {
            List<RolesOfUsers> roles = new List<RolesOfUsers>();
            List<AppUser> Users = _userManager.Users.ToList();
            foreach (AppUser item in Users)
            {
                var role = await _userManager.GetRolesAsync(item);
                roles.Add(new RolesOfUsers
                {
                    RoleName = role.ToList(),
                    UserName = item.UserName
                });
            }
            return roles;
        }
    }
}
