using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Entities
{
    public class UserEntity
    {
        [Key]
        public Guid id { get; set; }
        [Required]
        public string firstName { get; set; } = string.Empty;
        [Required]
        public string lastName { get; set; } = string.Empty;
        [Required]
        public string username { get; set; } = string.Empty;
        [Required]
        public string password { get; set; } = string.Empty;
        [Required]
        public DateTime dob { get; set; }
        [Required]
        public DateTime createdAt { get; set; }
        [Required]
        public DateTime updatedAt { get; set; }
        [Required]
        public int gender { get; set; }
        [Required]
        public int active { get; set; }
        [Required]
        public bool verified { get; set; }
    }
}

