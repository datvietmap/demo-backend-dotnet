using InventoryManagement.Helpers;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InventoryManagement.Entities
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration _configuration;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public DataContext(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Role>? Roles { get; set; }
        public DbSet<User>? Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SampleDatabase"));
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            var identity = (_httpContextAccessor.HttpContext != null) ? _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity : null;
            Guid userId;
            if (identity == null || identity.FindFirst(Constants.CustomClaimTypes.ID) == null)
            {
                userId = default(Guid);
            }
            else
            {
                userId = Guid.Parse(identity.FindFirst(Constants.CustomClaimTypes.ID).Value);
            }

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    if (((BaseEntity)entity.Entity).Id == default(Guid))
                    {
                        ((BaseEntity)entity.Entity).Id = Guid.NewGuid();
                    }
                    ((BaseEntity)entity.Entity).CreatedBy = userId;
                    ((BaseEntity)entity.Entity).CreatedDate = DateTime.Now;
                }

                if (entity.State == EntityState.Modified)
                {
                    ((BaseEntity)entity.Entity).ModifiedBy = userId;
                    ((BaseEntity)entity.Entity).ModifiedDate = DateTime.Now;
                }
            }
        }
    }
}
