using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Entities
{
    public class AccountStatusEntity
    {
        [Key]
        public Guid userID { get; set; }
        [Required]
        public RoleIdentifiers status { get; set; }
        [Required]
        public string reason { get; set; } = string.Empty;
        [Required]
        public DateTime updatedAt { get; set; }
    }
}