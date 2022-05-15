using AuthenticationService.Requests;
using AuthenticationService.Entities;

namespace AuthenticationService.Repositories
{
    public interface IUserRepository
    {
        public dynamic Authenticate(AuthenticateRequest authenticateRequest);
        public IEnumerable<UserEntity> GetAll();
        public UserEntity GetById(Guid id);
        public dynamic CreateAccount(UserEntity userEntity);
        public dynamic ActivateAccount(Guid userID, string username);
    }
}

