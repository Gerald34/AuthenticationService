using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AuthenticationService.Entities
{
    public class UserEntity
    {
        [Key]
        public Guid id { get; set; }
        [Required]
        public RoleIdentifiers role { get; set; }
        [Required]
        public string firstName { get; set; } = string.Empty;
        [Required]
        public string lastName { get; set; } = string.Empty;
        [Required]
        public string username { get; set; } = string.Empty;
        [Required]
        [StringLength(100, MinimumLength = 3)]
        [JsonIgnore]
        public string password { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Text)]
        public string RSAIdNumber { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Date)]
        public DateTime dob { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime createdAt { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime updatedAt { get; set; }
        [Required]
        public int gender { get; set; }
        [Required]
        public StatusIdentifiers active { get; set; }
        [Required]
        public bool verified { get; set; }
    }
}

