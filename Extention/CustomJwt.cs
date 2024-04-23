using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Back_Auth_Jwt.Extention
{
    public static class CustomJwt
    {
        public static void AddCustomJwt(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddAuthentication(
                o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }
           ).AddJwtBearer(
                    o =>
                    {
                        o.RequireHttpsMetadata = false;
                        o.SaveToken = true;
                        o.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateAudience = false,
                            ValidateIssuer = true,
                            ValidIssuer = configuration["JWT:Issuer"],
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                        };
                    }
                
                );

        }
    }
}
