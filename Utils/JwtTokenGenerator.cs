using AuthenticationService.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationService.Config;
using Microsoft.Extensions.Options;

namespace AuthenticationService.Utils
{
    public static class JwtTokenGenerator
    {
        /// <summary>
        /// JWT security token generator
        /// </summary>
        /// <param name="userEntity"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string GenerateJwtToken(UserEntity userEntity, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("id", userEntity.id.ToString()),
                    new Claim("first name", userEntity.firstName),
                    new Claim("last name", userEntity.lastName),
                    new Claim("username", userEntity.username),
                    new Claim("gender", userEntity.gender.ToString()),
                    new Claim("role", userEntity.role.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                // Issuer = "https://localhost:7091"
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}