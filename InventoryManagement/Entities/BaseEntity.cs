using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Entities
{
	public class BaseEntity
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		public Guid? CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public Guid? ModifiedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public bool IsDeleted { get; set; }
	}
}
