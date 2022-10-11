using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Dtos
{
    public class UpdateUserRequest
    {
        [Required]
        public Guid Id { get; set; }
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
