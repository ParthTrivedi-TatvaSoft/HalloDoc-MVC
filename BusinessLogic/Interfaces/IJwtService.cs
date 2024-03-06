using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IJwtService
    {
        string GetJwtToken(Aspnetuser aspnetuser);

        bool ValidateToken(string token, out JwtSecurityToken jwtSecurityToken);
    }
}
