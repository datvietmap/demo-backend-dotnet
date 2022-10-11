using InventoryManagement.Dtos;
using InventoryManagement.Entities;

namespace InventoryManagement.Services
{
    public interface IUserService : IBaseService<User>
    {
        IEnumerable<User>? GetAllByRole(Guid? id);
        Task<User> Authenticate(string username, string password);
        Task<bool> Create(CreateUserRequest user);
        Task<bool> UpdateUser(UpdateUserRequest user);
    }
}
