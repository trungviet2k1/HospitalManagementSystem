using HospitalManagementSystem.BusinessObject.Models;
using HospitalManagementSystem.DataAccess.DAO;
using HospitalManagementSystem.DataAccess.Repositories.IRepository;

namespace HospitalManagementSystem.DataAccess.Repositories.RepositoryImp
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DepartmentDAO _departmentDAO;

        public DepartmentRepository(DepartmentDAO departmentDAO) => _departmentDAO = departmentDAO;

        public Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return _departmentDAO.GetAllDepartmentsAsync();
        }

        public Task<Department> GetDepartmentByIdAsync(int departmentId)
        {
            return _departmentDAO.GetDepartmentByIdAsync(departmentId);
        }

        public Task AddDepartmentAsync(Department department)
        {
            return _departmentDAO.AddDepartmentAsync(department);
        }

        public Task UpdateDepartmentAsync(Department department)
        {
            return _departmentDAO.UpdateDepartmentAsync(department);
        }

        public Task DeleteDepartmentAsync(int departmentId)
        {
            return _departmentDAO.DeleteDepartmentAsync(departmentId);
        }
    }
}