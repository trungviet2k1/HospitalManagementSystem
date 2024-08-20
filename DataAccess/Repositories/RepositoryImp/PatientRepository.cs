using BusinessObject.Models;
using DataAccess.DAO;
using DataAccess.Repositories.IRepository;

namespace DataAccess.Repositories.RepositoryImp
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientDAO _patientDAO;

        public PatientRepository(PatientDAO patientDAO) => _patientDAO = patientDAO;

        public Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return _patientDAO.GetAllPatientsAsync();
        }

        public Task<Patient> GetPatientByIdAsync(int patientId)
        {
            return _patientDAO.GetPatientByIdAsync(patientId);
        }

        public Task AddPatientAsync(Patient patient)
        {
            return _patientDAO.AddPatientAsync(patient);
        }

        public Task UpdatePatientAsync(Patient patient)
        {
            return _patientDAO.UpdatePatientAsync(patient);
        }

        public Task DeletePatientAsync(int patientId)
        {
            return _patientDAO.DeletePatientAsync(patientId);
        }
    }
}