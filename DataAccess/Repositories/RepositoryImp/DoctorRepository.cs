using BusinessObject.Models;
using DataAccess.DAO;
using DataAccess.Repositories.IRepository;

namespace DataAccess.Repositories.RepositoryImp
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