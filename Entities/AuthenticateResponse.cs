
namespace AuthenticationService.Entities
{
    public class AuthenticateResponse
    {
        public int id { get; set; }
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public DateTime dob { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int gender { get; set; }
        public int active { get; set; }
        public bool verified { get; set; }
        public string token { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;


        public AuthenticateResponse(UserEntity userEntity, string autheToken)
        {
            id = userEntity.id;
            firstName = userEntity.firstName;
            lastName = userEntity.lastName;
            username = userEntity.username;
            active = userEntity.active;
            verified = userEntity.verified;
            dob = userEntity.dob;
            gender = userEntity.gender;
            token = autheToken;
            type = "Bearer";
        }
    }
}



