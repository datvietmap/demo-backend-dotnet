namespace InventoryManagement.Dtos
{
    public class UpdateRoleRequest
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Remarks { get; set; }
    }
}
