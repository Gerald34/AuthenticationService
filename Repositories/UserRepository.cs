using AuthenticationService.Requests;
using AuthenticationService.Entities;
using AuthenticationService.DTO;

namespace AuthenticationService.Repositories
{
    public interface IUserRepository
    {
        public dynamic Authenticate(AuthenticateRequest authenticateRequest);
        public IEnumerable<AuthenticateDTO> GetAll();
        public AuthenticateDTO GetById(Guid id);
        public dynamic CreateAccount(UserEntity userEntity);
        public dynamic ActivateAccount(Guid userID, string username);
    }
}

