using BusinessObject.Models;
using DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class AppointmentDAO(DbContextOptions<HospitalManagementDbContext> options)
    {
        private readonly DbContextOptions<HospitalManagementDbContext> _options = options;

        private HospitalManagementDbContext CreateContext()
        {
            return new HospitalManagementDbContext(_options);
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            using var context = CreateContext();
            return await context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToListAsync();
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(int appointmentId)
        {
            using var context = CreateContext();
            return await context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAsync(int doctorId)
        {
            using var context = CreateContext();
            return await context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientAsync(int patientId)
        {
            using var context = CreateContext();
            return await context.Appointments
                .Include(a => a.Patient) 
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(DateTime startDate, DateTime endDate)
        {
            using var context = CreateContext();
            return await context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.AppointmentDate >= startDate && a.AppointmentDate <= endDate)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task AddAppointmentAsync(Appointment appointment)
        {
            using var context = CreateContext();
            context.Appointments.Add(appointment);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            using var context = CreateContext();
            context.Appointments.Update(appointment);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAppointmentAsync(int appointmentId)
        {
            using var context = CreateContext();
            var appointment = await context.Appointments.FindAsync(appointmentId);
            if (appointment != null)
            {
                context.Appointments.Remove(appointment);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsTimeSlotAvailableAsync(int doctorId, DateTime appointmentDate, int excludeAppointmentId = 0)
        {
            using var context = CreateContext();

            // Tính toán thời gian chính xác để tránh client evaluation
            var bufferMinutes = 30;
            var startTime = appointmentDate.AddMinutes(-bufferMinutes);
            var endTime = appointmentDate.AddMinutes(bufferMinutes);

            var hasConflict = await context.Appointments
                .AnyAsync(a => a.DoctorId == doctorId &&
                              a.AppointmentId != excludeAppointmentId &&
                              a.AppointmentDate >= startTime &&
                              a.AppointmentDate <= endTime);

            return !hasConflict;
        }
    }
}