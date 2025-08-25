using BusinessObject.Models;
using DataAccess.DAO;
using DataAccess.Repositories.IRepository;

namespace DataAccess.Repositories
{
    public class RoleRepository(RoleDAO roleDAO) : IRoleRepository
    {
        private readonly RoleDAO _roleDAO = roleDAO;

        public Task<List<Role>> GetAllRolesAsync()
        {
            return _roleDAO.GetAllRolesAsync();
        }

        public Task<List<Permission>> GetAllPermissionsAsync()
        {
            return _roleDAO.GetAllPermissionsAsync();
        }

        public Task<Role?> GetRoleByIdAsync(int roleId)
        {
            return _roleDAO.GetRoleByIdAsync(roleId);
        }

        public Task AddRoleAsync(Role role)
        {
            return _roleDAO.AddRoleAsync(role);
        }

        public Task UpdateRoleAsync(Role role)
        {
            return _roleDAO.UpdateRoleAsync(role);
        }

        public Task DeleteRoleAsync(int roleId)
        {
            return _roleDAO.DeleteRoleAsync(roleId);
        }
    }
}