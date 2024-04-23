namespace Back_Auth_Jwt.Dto
{
    public class RegistreDto
    {
        public string UserName {  get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
    }
}
