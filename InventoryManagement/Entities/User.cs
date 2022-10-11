namespace InventoryManagement.Entities
{
	public class User : BaseEntity
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Username { get; set; }
		public byte[]? PasswordHash { get; set; }
		public byte[]? PasswordSalt { get; set; }
		public string? EmailAddress { get; set; }
		public Guid? RoleId { get; set; }

		public string FullName
		{
			get { return $"{FirstName} {LastName}"; }
		}
	}
}
