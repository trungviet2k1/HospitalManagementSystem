using BusinessObject.Models;
using DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class RoleDAO
    {
        private readonly DbContextOptions<HospitalManagementDbContext> _options;

        public RoleDAO(DbContextOptions<HospitalManagementDbContext> options)
        {
            _options = options;
        }

        private HospitalManagementDbContext CreateContext()
        {
            return new HospitalManagementDbContext(_options);
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            using var context = CreateContext();
            return await context.Roles
                                .Include(r => r.RolePermissions)
                                .ToListAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(int roleId)
        {
            using var context = CreateContext();
            return await context.Roles
                                .Include(r => r.RolePermissions)
                                .FirstOrDefaultAsync(r => r.RoleId == roleId);
        }

        public async Task AddRoleAsync(Role role)
        {
            using var context = CreateContext();
            context.Roles.Add(role);
            await context.SaveChangesAsync();
        }

        public async Task UpdateRoleAsync(Role role)
        {
            using var context = CreateContext();
            context.Roles.Update(role);
            await context.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            using var context = CreateContext();
            var role = await context.Roles.FindAsync(roleId);
            if (role != null)
            {
                context.Roles.Remove(role);
                await context.SaveChangesAsync();
            }
        }
    }
}