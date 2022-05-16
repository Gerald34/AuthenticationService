using AuthenticationService.Entities;

namespace AuthenticationService.Responses
{
    public class AuthenticateResponse
    {
        public Guid id { get; set; }
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public DateTime dob { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int gender { get; set; }
        public int active { get; set; }
        public bool verified { get; set; }
        public dynamic authentication { get; set; }

        public AuthenticateResponse(UserEntity userEntity, string authType, string authToken)
        {
            id = userEntity.id;
            firstName = userEntity.firstName;
            lastName = userEntity.lastName;
            username = userEntity.username;
            active = userEntity.active;
            verified = userEntity.verified;
            dob = userEntity.dob;
            gender = userEntity.gender;
            authentication = new { type = authType, token = authToken };
        }
    }
}



