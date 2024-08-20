using HospitalManagementSystem.BusinessObject.Models;
using HospitalManagementSystem.DataAccess.DAO;
using HospitalManagementSystem.DataAccess.Repositories.IRepository;

namespace HospitalManagementSystem.DataAccess.Repositories.RepositoryImp
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DoctorDAO _doctorDAO;

        public DoctorRepository(DoctorDAO doctorDAO) => _doctorDAO = doctorDAO;

        public Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
        {
            return _doctorDAO.GetAllDoctorsAsync();
        }

        public Task<Doctor> GetDoctorByIdAsync(int doctorId)
        {
            return _doctorDAO.GetDoctorByIdAsync(doctorId);
        }

        public Task AddDoctorAsync(Doctor doctor)
        {
            return _doctorDAO.AddDoctorAsync(doctor);
        }

        public Task UpdateDoctorAsync(Doctor doctor)
        {
            return _doctorDAO.UpdateDoctorAsync(doctor);
        }

        public Task DeleteDoctorAsync(int doctorId)
        {
            return _doctorDAO.DeleteDoctorAsync(doctorId);
        }
    }
}