using AuthenticationService.DbContexts;
using AuthenticationService.Config;
using Microsoft.Extensions.Options;
using AuthenticationService.Requests;
using AuthenticationService.Utils;

namespace AuthenticationService.Services
{
    public class UserService
    {
        private UserDbContext _userDbContext;
        private readonly AppSettings _appSettings;
        private dynamic _response = null;

        public UserService(IOptions<AppSettings> appSettings, UserDbContext usersDbContext)
        {
            _appSettings = appSettings.Value;
            _userDbContext = usersDbContext;
        }

        /// <summary>
        /// Authenticate user
        /// Generate Json Web Token
        /// </summary>
        /// <param name="authenticateRequest"></param>
        /// <returns>Dynamic object including user record and token</returns>
        public dynamic Authenticate(AuthenticateRequest authenticateRequest)
        {
            var user = _userDbContext.Users.FirstOrDefault(
                user => user.username == authenticateRequest.Username
            );

            if (user == null) return new { error = true, message = "User not found." };

            if (!PasswordEncryptor.decryptString(authenticateRequest!.Password, user.password))
            {
                _response = new { error = true, message = "Username or password is incorrect." };
                return _response;
            }

            var token = JwtTokenGenerator.GenerateJwtToken(user, _appSettings.Secret);
            _response = new
            {
                error = false,
                message = "User found",
                data = new
                {

                }
            };

            return _response;
        }
    }
}

