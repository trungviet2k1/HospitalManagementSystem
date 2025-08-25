using BusinessObject.Models;
using DataAccess.DAO;
using DataAccess.Repositories.IRepository;

namespace DataAccess.Repositories.RepositoryImp
{
    public class PatientRepository(PatientDAO patientDAO) : IPatientRepository
    {
        public Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return patientDAO.GetAllPatientsAsync();
        }

        public Task<Patient> GetPatientByIdAsync(int patientId)
        {
            return patientDAO.GetPatientByIdAsync(patientId);
        }

        public Task AddPatientAsync(Patient patient)
        {
            return patientDAO.AddPatientAsync(patient);
        }

        public Task UpdatePatientAsync(Patient patient)
        {
            return patientDAO.UpdatePatientAsync(patient);
        }

        public Task DeletePatientAsync(int patientId)
        {
            return patientDAO.DeletePatientAsync(patientId);
        }
    }
}