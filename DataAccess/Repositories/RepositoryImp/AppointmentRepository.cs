using BusinessObject.Models;
using DataAccess.DAO;
using DataAccess.Repositories.IRepository;

namespace DataAccess.Repositories.RepositoryImp
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppointmentDAO _appointmentDAO;

        public AppointmentRepository(AppointmentDAO appointmentDAO) => _appointmentDAO = appointmentDAO;

        public Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return _appointmentDAO.GetAllAppointmentsAsync();
        }

        public Task<Appointment> GetAppointmentByIdAsync(int appointmentId)
        {
            return _appointmentDAO.GetAppointmentByIdAsync(appointmentId);
        }

        public Task AddAppointmentAsync(Appointment appointment)
        {
            return _appointmentDAO.AddAppointmentAsync(appointment);
        }

        public Task UpdateAppointmentAsync(Appointment appointment)
        {
            return _appointmentDAO.UpdateAppointmentAsync(appointment);
        }

        public Task DeleteAppointmentAsync(int appointmentId)
        {
            return _appointmentDAO.DeleteAppointmentAsync(appointmentId);
        }
    }
}