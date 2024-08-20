using HospitalManagementSystem.BusinessObject.Models;

namespace HospitalManagementSystem.DataAccess.Repositories.IRepository
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task<Department> GetDepartmentByIdAsync(int departmentId);
        Task AddDepartmentAsync(Department department);
        Task UpdateDepartmentAsync(Department department);
        Task DeleteDepartmentAsync(int departmentId);
    }
}