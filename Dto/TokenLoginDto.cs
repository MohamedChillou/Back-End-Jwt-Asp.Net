using Back_Auth_Jwt.Users;

namespace Back_Auth_Jwt.Dto
{
    public class TokenLoginDto
    {
        public string Token { get; set; }
        public AppUser User { get; set; }
        public bool IsAuthenticated { get; set; } = false;
        public string Message { get; set; }
        public double expires { get; set; } 
        public IList<string> roles { get; set; }
    }
}
