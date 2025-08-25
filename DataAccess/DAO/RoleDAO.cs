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

        public async Task<List<Permission>> GetAllPermissionsAsync()
        {
            using var context = CreateContext();
            return await context.Permissions.ToListAsync();
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

            // Lấy role từ db cùng RolePermissions
            var existingRole = await context.Roles
                                            .Include(r => r.RolePermissions)
                                            .FirstOrDefaultAsync(r => r.RoleId == role.RoleId);

            if (existingRole == null)
                throw new Exception("Role not found.");

            // Cập nhật RoleName (nếu cần)
            existingRole.RoleName = role.RoleName;

            // Cập nhật RolePermissions
            // Xóa các RolePermission không còn tick
            var toRemove = existingRole.RolePermissions
                                       .Where(rp => !role.RolePermissions.Any(x => x.PermissionId == rp.PermissionId))
                                       .ToList();
            context.RemoveRange(toRemove);

            // Thêm các RolePermission mới
            var toAdd = role.RolePermissions
                            .Where(rp => !existingRole.RolePermissions.Any(x => x.PermissionId == rp.PermissionId))
                            .ToList();
            foreach (var rp in toAdd)
            {
                existingRole.RolePermissions.Add(new RolePermission
                {
                    RoleId = existingRole.RoleId,
                    PermissionId = rp.PermissionId
                });
            }

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