using HospitalManagementSystem.BusinessObject.Models;

namespace HospitalManagementSystem.DataAccess.Repositories.IRepository
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<Patient> GetPatientByIdAsync(int patientId);
        Task AddPatientAsync(Patient patient);
        Task UpdatePatientAsync(Patient patient);
        Task DeletePatientAsync(int patientId);
    }
}