using BusinessObject.Models;
using DataAccess.DAO;
using DataAccess.Repositories.IRepository;

namespace DataAccess.Repositories.RepositoryImp
{
    public class AppointmentRepository(AppointmentDAO appointmentDAO) : IAppointmentRepository
    {
        private readonly AppointmentDAO _appointmentDAO = appointmentDAO;

        public Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
            => _appointmentDAO.GetAllAppointmentsAsync();

        public Task<Appointment?> GetAppointmentByIdAsync(int appointmentId)
            => _appointmentDAO.GetAppointmentByIdAsync(appointmentId);

        public async Task<Appointment?> GetAppointmentWithDetailsAsync(int appointmentId)
        {
            return await _appointmentDAO.GetAppointmentByIdAsync(appointmentId);
        }

        public Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(int doctorId)
            => _appointmentDAO.GetAppointmentsByDoctorAsync(doctorId);

        public Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId)
            => _appointmentDAO.GetAppointmentsByPatientAsync(patientId);

        public Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(DateTime startDate, DateTime endDate)
            => _appointmentDAO.GetUpcomingAppointmentsAsync(startDate, endDate);

        public Task AddAppointmentAsync(Appointment appointment)
            => _appointmentDAO.AddAppointmentAsync(appointment);

        public Task UpdateAppointmentAsync(Appointment appointment)
            => _appointmentDAO.UpdateAppointmentAsync(appointment);

        public Task DeleteAppointmentAsync(int appointmentId)
            => _appointmentDAO.DeleteAppointmentAsync(appointmentId);

        public Task<bool> IsTimeSlotAvailableAsync(int doctorId, DateTime appointmentDate, int excludeAppointmentId = 0)
            => _appointmentDAO.IsTimeSlotAvailableAsync(doctorId, appointmentDate, excludeAppointmentId);
    }
}