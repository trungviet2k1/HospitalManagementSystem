using BusinessObject.Models;
using DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class PermissionDAO
    {
        private static PermissionDAO? instance;
        private static readonly object lockObj = new object();

        private PermissionDAO() { }

        public static PermissionDAO Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (instance == null)
                        instance = new PermissionDAO();
                    return instance;
                }
            }
        }

        public async Task<List<Permission>> GetAllPermissionsAsync(HospitalManagementDbContext context)
        {
            return await context.Permissions.ToListAsync();
        }

        public async Task<Permission?> GetPermissionByIdAsync(HospitalManagementDbContext context, int permissionId)
        {
            return await context.Permissions.FindAsync(permissionId);
        }

        public async Task AddPermissionAsync(HospitalManagementDbContext context, Permission permission)
        {
            context.Permissions.Add(permission);
            await context.SaveChangesAsync();
        }

        public async Task UpdatePermissionAsync(HospitalManagementDbContext context, Permission permission)
        {
            context.Permissions.Update(permission);
            await context.SaveChangesAsync();
        }

        public async Task DeletePermissionAsync(HospitalManagementDbContext context, int permissionId)
        {
            var permission = await context.Permissions
                                  .Include(p => p.RolePermissions)
                                  .FirstOrDefaultAsync(p => p.PermissionId == permissionId);

            if (permission == null) return;

            // Check có bao nhiêu role đang sử dụng permission này
            int roleCount = permission.RolePermissions.Count;

            if (roleCount > 0)
            {
                // Báo lỗi cho UI xử lý
                throw new InvalidOperationException(
                    $"Permission '{permission.PermissionName}' đang được {roleCount} role(s) sử dụng, không thể xóa.");
            }

            context.Permissions.Remove(permission);
            await context.SaveChangesAsync();
        }
    }
}