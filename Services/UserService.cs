using AuthenticationService.DbContexts;
using AuthenticationService.Config;
using Microsoft.Extensions.Options;
using AuthenticationService.Requests;
using AuthenticationService.Utils;
using AuthenticationService.Repositories;
using AuthenticationService.Entities;

namespace AuthenticationService.Services
{
    public class UserService : IUserRepository
    {
        private UserDbContext _userDbContext;
        private readonly AppSettings _appSettings;
        private dynamic _response = null;

        public UserService(
        IOptions<AppSettings> appSettings,
        UserDbContext usersDbContext)
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

        public IEnumerable<UserEntity> GetAll()
        {
            return _userDbContext.Users.ToList<UserEntity>();
        }

        public UserEntity GetById(Guid id)
        {
            UserEntity userEntity = _userDbContext.Users.FirstOrDefault
                <UserEntity>(user => user.id == id);

            return (userEntity == null) ? userEntity : null;
        }

        public dynamic CreateAccount(UserEntity userEntity)
        {
            if (userEntity == null)
            {
                throw new ArgumentNullException(nameof(userEntity));
            }

            bool userExists = _UserExists(userEntity.username);
            if (userExists)
            {
                return new { error = true, message = "User with username: " + userEntity.username + " already exists, try a different valid email address." };
            }
            _userDbContext.Users.Add(userEntity);
            return new { error = false, message = "User account created" };
        }

        public dynamic ActivateAccount(Guid userID, string username)
        {
            UserEntity userEntity = _userDbContext.Users.FirstOrDefault
            <UserEntity>(user => user.id == userID && user.username == username);

            if (userEntity == null)
            {
                return new { error = true, message = "User not found" };
            }

            userEntity.active = 1;
            userEntity.verified = true;
            _userDbContext.SaveChanges();

            return new { error = false, message = "User account is activated" };
        }

        private bool _UserExists(string username)
        {
            return (_userDbContext.Users.FirstOrDefault
                <UserEntity>(data => data.username == username) == null) ? true : false;
        }
    }
}

