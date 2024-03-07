using BusinessLogic.Interfaces;
using DataAccess.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        string[] Role = { "Admin", "User", "Physician" };
        public string GetJwtToken(Aspnetuser aspnetuser)
        {
            var role = aspnetuser.Aspnetuserroles.FirstOrDefault(x => x.Userid == aspnetuser.Id);
            int roleid = role.Roleid;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, aspnetuser.Email),
                new Claim(ClaimTypes.Role,Role[roleid-1]),
                 //aspnetuser.Aspnetuserroles.ToString()
                new Claim("aspNetUserId",aspnetuser.Id)
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(20);

            var token = new JwtSecurityToken(

                _configuration["Jwt: Issuer"], _configuration["Jwt: Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string token, out JwtSecurityToken jwtSecurityToken)
        {
            jwtSecurityToken = null;

            if (token == null)
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            try
            {

                tokenHandler.ValidateToken(token, new TokenValidationParameters

                {

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero

                }, out SecurityToken validatedToken);

                // Corrected access to the validatedToken

                jwtSecurityToken = (JwtSecurityToken)validatedToken;

                if (jwtSecurityToken != null)
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}