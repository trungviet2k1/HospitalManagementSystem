using BusinessObject.Models;

namespace DataAccess.Repositories.IRepository
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllRolesAsync();
        Task<List<Permission>> GetAllPermissionsAsync();
        Task<Role?> GetRoleByIdAsync(int roleId);
        Task<int> CountUsersByRoleAsync(int roleId);
        Task AddRoleAsync(Role role);
        Task UpdateRoleAsync(Role role);
        Task DeleteRoleAsync(int roleId);
    }
}