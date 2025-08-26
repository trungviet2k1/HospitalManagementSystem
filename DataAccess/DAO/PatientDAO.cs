using BusinessObject.Models;
using DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class PatientDAO(DbContextOptions<HospitalManagementDbContext> options)
    {
        private readonly DbContextOptions<HospitalManagementDbContext> _options = options;

        private HospitalManagementDbContext CreateContext()
        {
            return new HospitalManagementDbContext(_options);
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            using var context = CreateContext();
            return await context.Patients
                .Include(p => p.Doctor) // Include Doctor information với đầy đủ thông tin
                .ToListAsync();
        }

        public async Task<Patient> GetPatientByIdAsync(int patientId)
        {
            using var context = CreateContext();
            var patient = await context.Patients
                .Include(p => p.Doctor) // Include Doctor information với đầy đủ thông tin
                .FirstOrDefaultAsync(p => p.PatientId == patientId);
            return patient ?? new Patient();
        }

        public async Task AddPatientAsync(Patient patient)
        {
            using var context = CreateContext();
            context.Patients.Add(patient);
            await context.SaveChangesAsync();
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            using var context = CreateContext();
            context.Patients.Update(patient);
            await context.SaveChangesAsync();
        }

        public async Task DeletePatientAsync(int patientId)
        {
            using var context = CreateContext();
            var patient = await context.Patients.FindAsync(patientId);
            if (patient != null)
            {
                context.Patients.Remove(patient);
                await context.SaveChangesAsync();
            }
        }
    }
}