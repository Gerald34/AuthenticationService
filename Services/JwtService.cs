using AuthenticationService.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationService.Config;
using Microsoft.Extensions.Options;

namespace AuthenticationService.Services
{
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;
        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        /// <summary>
        /// JWT security token generator
        /// </summary>
        /// <param name="userEntity"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public string GenerateJwtToken(UserEntity userEntity)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, userEntity.id.ToString()),
                    new Claim("id", userEntity.id.ToString()),
                    new Claim("first name", userEntity.firstName),
                    new Claim("last name", userEntity.lastName),
                    new Claim(ClaimTypes.Email, userEntity.username),
                    new Claim(ClaimTypes.Gender, userEntity.gender.ToString()),
                    new Claim(ClaimTypes.Role, userEntity.role.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                // Issuer = "https://localhost:7091"
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Validate JWT security token
        /// </summary>
        /// <param name="token"></param>
        /// <returns>userID</returns>
        public dynamic ValidateToken(string token)
        {
            if (token == null)
                throw new Exception("valid token not provided");

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
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

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                Guid userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                return new { error = false, userID = userId };
            }
            catch (SecurityTokenValidationException e)
            {
                return new { error = true, message = e.Message };
            }
        }
    }
}