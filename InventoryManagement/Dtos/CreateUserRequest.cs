using Microsoft.Build.Framework;

namespace InventoryManagement.Dtos
{
    public class CreateUserRequest
    {
        [Required]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? EmailAddress { get; set; }
        public Guid? RoleId { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
