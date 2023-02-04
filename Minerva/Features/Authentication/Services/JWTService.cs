using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Minerva.Config;
using Minerva.Features.Authentication.Documents;

namespace Minerva.Features.Authentication.Services;

public class JWTService
{
    private readonly JwtConfig Config;
  
     public JWTService(MinervaConfig config)
     {
         Config = config.Jwt;
     }
     
     public string GenerateToken(UserDocument user)
     {
         var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.Key));
         var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

         var claims = new[] 
         {
             new Claim(JwtRegisteredClaimNames.Sub, $"{user.FirstName} {user.LastName}"),
             new Claim(JwtRegisteredClaimNames.Email, user.Email.Address),
             new Claim("roles", user.Role.ToString()),
             new Claim("Date", DateTime.Now.ToString(CultureInfo.InvariantCulture)),
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
         };

         var token = new JwtSecurityToken(Config.Issuer, Config.Issuer, claims, expires: DateTime.Now.AddMinutes(120), signingCredentials: credentials);

         var tokenData = new JwtSecurityTokenHandler().WriteToken(token);
         
         return tokenData;
     }
}