using System.Text.Json.Serialization;
using AuthenticationService.Entities;

namespace AuthenticationService.DTO
{
    public class AuthenticateDTO
    {
        public Guid id { get; set; }
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public DateTime dob { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int gender { get; set; }
        public StatusIdentifiers active { get; set; }
        public bool verified { get; set; }
        public dynamic authentication { get; set; }

        public AuthenticateDTO(UserEntity userEntity, string authType, string authToken)
        {
            id = userEntity.id;
            firstName = userEntity.firstName;
            lastName = userEntity.lastName;
            username = userEntity.username;
            active = userEntity.active;
            verified = userEntity.verified;
            dob = userEntity.dob;
            gender = userEntity.gender;
            authentication = (authToken != null && authType != null) ? new { type = authType, token = authToken } : null;
        }

        public AuthenticateDTO(UserEntity userEntity)
        {
            id = userEntity.id;
            firstName = userEntity.firstName;
            lastName = userEntity.lastName;
            username = userEntity.username;
            active = userEntity.active;
            verified = userEntity.verified;
            dob = userEntity.dob;
            gender = userEntity.gender;
        }
    }
}



