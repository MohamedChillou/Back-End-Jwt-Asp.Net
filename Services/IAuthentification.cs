using Back_Auth_Jwt.Dto;
using Back_Auth_Jwt.Users;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace Back_Auth_Jwt.Services
{
    public interface IAuthentification
    {
        public Task<JwtSecurityToken> GetJwtSecurityToken(AppUser user);
        public Task <bool> CreateNewUser(RegistreDto user);
        public Task<bool> CreateRole(string role);
        public Task<List<IdentityRole>> GetRoles() ;
        public Task<List<AppUser>> GetUsers();
        public Task<bool> CheckedRole(List<string> roles);
        public Task<TokenLoginDto> LoginDto(LoginDto login);
        public Task<List<RolesOfUsers>> GetAllUserOfRoles();
    }
}
