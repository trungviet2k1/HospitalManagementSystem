using BusinessObject.Models;
using DataAccess.DBContext;
using DataAccess.DAO;
using DataAccess.Repositories.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.RepositoryImp
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly HospitalManagementDbContext _context;

        public PermissionRepository(HospitalManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            return await PermissionDAO.Instance.GetAllPermissionsAsync(_context);
        }

        public async Task<Permission?> GetPermissionByIdAsync(int permissionId)
        {
            return await PermissionDAO.Instance.GetPermissionByIdAsync(_context, permissionId);
        }

        public async Task AddPermissionAsync(Permission permission)
        {
            await PermissionDAO.Instance.AddPermissionAsync(_context, permission);
        }

        public async Task UpdatePermissionAsync(Permission permission)
        {
            await PermissionDAO.Instance.UpdatePermissionAsync(_context, permission);
        }

        public async Task DeletePermissionAsync(int permissionId)
        {
            await PermissionDAO.Instance.DeletePermissionAsync(_context, permissionId);
        }
    }
}