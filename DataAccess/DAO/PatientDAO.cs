using HospitalManagementSystem.BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.DataAccess.DAO
{
    public class PatientDAO
    {
        private readonly HospitalManagementSystemContext _context;

        public PatientDAO(HospitalManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await _context.Patients.ToListAsync();
        }

        public async Task<Patient> GetPatientByIdAsync(int patientId)
        {
            var patient = await _context.Patients.FindAsync(patientId);
            return patient ?? new Patient();
        }

        public async Task AddPatientAsync(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePatientAsync(int patientId)
        {
            var patient = await _context.Patients.FindAsync(patientId);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
        }
    }
}