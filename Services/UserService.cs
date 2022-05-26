using AuthenticationService.DbContexts;
using AuthenticationService.Config;
using Microsoft.Extensions.Options;
using AuthenticationService.Requests;
using AuthenticationService.Utils;
using AuthenticationService.Repositories;
using AuthenticationService.Entities;
using AuthenticationService.DTO;

namespace AuthenticationService.Services
{
    /// <summary>
    /// performs all user related tasks
    /// </summary>
    public class UserService : IUserRepository
    {
        private UserDbContext _userDbContext;
        private readonly AppSettings _appSettings;
        private readonly JwtSettings _jwtSettings;
        private readonly RSAIDNumberService _rSAIDNumberService;
        private JwtService _jwtService;

        public UserService(
        IOptions<AppSettings> appSettings,
        IOptions<JwtSettings> jwtSettings,
        UserDbContext usersDbContext,
        RSAIDNumberService rSAIDNumberService,
        JwtService jwtService)
        {
            _jwtService = jwtService;
            _appSettings = appSettings.Value;
            _userDbContext = usersDbContext;
            _rSAIDNumberService = rSAIDNumberService;
            _jwtSettings = jwtSettings.Value;
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
                return new { error = true, message = "Username or password is incorrect." };
            }

            string token = _jwtService.GenerateJwtToken(user);
            return new
            {
                error = false,
                message = "User found",
                data = new AuthenticateDTO(user, "Bearer", token)
            };

        }

        public IEnumerable<AuthenticateDTO> GetAll()
        {
            List<UserEntity> users = _userDbContext.Users.ToList<UserEntity>();
            List<AuthenticateDTO> userCollection = new List<AuthenticateDTO>();
            foreach (UserEntity user in users)
                userCollection.Add(new AuthenticateDTO(user));

            return userCollection;
        }

        public AuthenticateDTO GetById(Guid id)
        {
            UserEntity userEntity = _userDbContext.Users.FirstOrDefault
                <UserEntity>(user => user.id == id);

            return (userEntity == null) ? new AuthenticateDTO(userEntity) : null;
        }

        public dynamic CreateAccount(UserEntity userEntity)
        {
            if (userEntity == null)
                throw new ArgumentNullException(nameof(userEntity));

            bool userExists = _FindUserByUsername(userEntity.username);
            if (userExists)
                return new { error = true, message = "User with username: " + userEntity.username + " already exists, try a different valid email address." };

            if (!_rSAIDNumberService.ValidRSAID(userEntity.RSAIdNumber))
                return new { error = true, message = "Provided ID content is not a valid RSA ID number." };

            UserEntity newUser = new UserEntity
            {
                id = new Guid(),
                role = userEntity.role,
                firstName = userEntity.firstName,
                lastName = userEntity.lastName,
                username = userEntity.username,
                password = PasswordEncryptor.EncryptString(userEntity.password, 1000),
                RSAIdNumber = userEntity.RSAIdNumber,
                dob = userEntity.dob,
                createdAt = DateTime.Now,
                updatedAt = DateTime.Now,
                gender = userEntity.gender,
                active = 0,
                verified = false
            };

            _userDbContext.Users.Add(newUser);
            _userDbContext.SaveChanges();
            return new { error = false, message = "User account created", data = new AuthenticateDTO(newUser) };
        }

        public dynamic ActivateAccount(Guid userID, string username)
        {
            UserEntity userEntity = _userDbContext.Users.FirstOrDefault
            <UserEntity>(user => user.id == userID && user.username == username);

            if (userEntity == null)
                return new { error = true, message = "User not found" };

            userEntity.active = StatusIdentifiers.DEACTIVATED;
            userEntity.verified = true;
            userEntity.updatedAt = DateTime.Now;
            _userDbContext.SaveChanges();

            return new { error = false, message = "User account is activated" };
        }

        public dynamic Deactivate(Guid userID, string reason)
        {
            UserEntity user = _userDbContext.Users.FirstOrDefault<UserEntity>(data => data.id == userID);
            if (user == null) return new { error = true, message = "User not found" };

            user.active = StatusIdentifiers.DEACTIVATED;
            user.role = RoleIdentifiers.DEACTIVATED;
            _userDbContext.SaveChanges();
            return new { error = false, message = "User with GUID: " + userID + " has been deactivated" };
        }

        public dynamic Suspend(Guid userID, string reason)
        {
            UserEntity user = _userDbContext.Users.FirstOrDefault<UserEntity>(data => data.id == userID);
            if (user == null) return new { error = true, message = "User not found" };

            user.active = StatusIdentifiers.SUSPENDED;
            _userDbContext.SaveChanges();
            return new { error = false, message = "User with GUID: " + userID + " has been suspended" };
        }

        private bool _FindUserByUsername(string username)
        {
            return (_userDbContext.Users.FirstOrDefault
                <UserEntity>(data => data.username == username) != null) ? true : false;
        }
    }
}

