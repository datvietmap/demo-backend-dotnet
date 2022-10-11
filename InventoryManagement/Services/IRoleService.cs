using InventoryManagement.Dtos;
using InventoryManagement.Entities;

namespace InventoryManagement.Services
{
    public interface IRoleService : IBaseService<Role>
    {
        Task<bool> Create(CreateRoleRequest role);
        Task<bool> UpdateRole(UpdateRoleRequest role);
    }
}
