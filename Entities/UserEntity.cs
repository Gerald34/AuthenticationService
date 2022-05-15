using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Entities
{
    public class UserEntity
    {
        [Key]
        public Guid id { get; set; }
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public DateTime dob { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int gender { get; set; }
        public int active { get; set; }
        public bool verified { get; set; }
    }
}

