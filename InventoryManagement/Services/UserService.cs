using InventoryManagement.Dtos;
using InventoryManagement.Entities;
using InventoryManagement.Helpers;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public UserService(DataContext dataContext, IServiceScopeFactory scopeFactory) : base(dataContext)
        {
            _scopeFactory = scopeFactory;
        }

        public async override Task<User?> Get(Guid id)
        {
            var selectedUser = await base.Get(id);
            return (selectedUser != null && selectedUser.IsDeleted) ? null : selectedUser;
        }

        public IEnumerable<User>? GetAllByRole(Guid? id)
        {
            IEnumerable<User>? result = null;

            using (var scope = _scopeFactory.CreateScope())
            {
                var roleContext = scope.ServiceProvider.GetService<IRoleService>();
                var roles = roleContext?.Find(x => x.IsDeleted == false);
                
                if (roles != null)
                {
                    var users = (from a in base.Find(x => x.IsDeleted == false)
                                 join b in roles on a.RoleId equals b.Id
                                 select a).ToList();

                    if (id != null || id != default(Guid))
                    {
                        result = users.Where(x => x.RoleId == id);
                    }
                    else
                    {
                        result = users;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// authenticate user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username.");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password.");
            }

            var selectedUser = await _entities.FirstOrDefaultAsync(x => x.Username == username);
            if (selectedUser == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            if (passwordHash == selectedUser.PasswordHash)
            {
                return selectedUser;
            }
            
            return null;
        }

        /// <summary>
        /// create user async
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> Create(CreateUserRequest request)
        {
            if(request == null)
            {
                throw new ArgumentNullException("Request");
            }

            if (request.Password == null)
            {
                throw new ArgumentNullException("Password");
            }

            var isUsernameDuplicate = _entities.Any(x => x.Username.ToLower() == request.Username.ToLower() && !x.IsDeleted);
            if (isUsernameDuplicate)
            {
                throw new ArgumentException("Username is duplicated");
            }
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                EmailAddress = request.EmailAddress,
                RoleId = request.RoleId
            };

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await base.Add(user);

            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> UpdateUser(UpdateUserRequest request)
        {
            if (request == null) throw new ArgumentNullException("user");

            var selectedUser = await Get(request.Id);

            if (selectedUser == null) throw new InvalidOperationException("User not found.");

            if (!string.IsNullOrWhiteSpace(request.FirstName) && selectedUser.FirstName != request.FirstName)
            {
                selectedUser.FirstName = request.FirstName;
            }

            if (!string.IsNullOrWhiteSpace(request.LastName) && selectedUser.LastName != request.LastName)
            {
                selectedUser.LastName = request.LastName;
            }

            if (selectedUser.EmailAddress != request.EmailAddress)
            {
                selectedUser.EmailAddress = request.EmailAddress;
            }

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);

                selectedUser.PasswordHash = passwordHash;
                selectedUser.PasswordSalt = passwordSalt;
            }

            if (request.RoleId != default(Guid) && selectedUser.RoleId != request.RoleId)
            {
                selectedUser.RoleId = request.RoleId;
            }

            base.Update(selectedUser);

            return await _context.SaveChangesAsync() > 0;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException(Constants.ErrorMessage.IsNullOrWhiteSpace, "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
