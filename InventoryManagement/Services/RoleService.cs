using InventoryManagement.Dtos;
using InventoryManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Services
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public RoleService(DataContext dataContext, IServiceScopeFactory scopeFactory) : base(dataContext)
        {
            _scopeFactory = scopeFactory;
        }

        /// <summary>
        /// create new role
        /// </summary>
        /// <param name="roleRequest"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> Create(CreateRoleRequest roleRequest)
        {
            if (roleRequest == null)
            {
                throw new ArgumentNullException(nameof(roleRequest));
            }

            var role = new Role
            {
                Name = roleRequest.Name,
                Remarks = roleRequest.Remarks
            };

            await base.Add(role);

            return await _context.SaveChangesAsync() > 0;
            
        }

        /// <summary>
        /// update role
        /// </summary>
        /// <param name="roleRequest"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<bool> UpdateRole(UpdateRoleRequest roleRequest)
        {
            var selectedRole = await _entities.FirstOrDefaultAsync(x => x.Id == roleRequest.Id && x.IsDeleted);

            if (selectedRole == null)
                throw new InvalidOperationException("Role not found.");

            selectedRole.Name = roleRequest.Name;
            selectedRole.Remarks = roleRequest.Remarks;

            base.Update(selectedRole);

            return await _context.SaveChangesAsync() > 0;

        }
    }
}
