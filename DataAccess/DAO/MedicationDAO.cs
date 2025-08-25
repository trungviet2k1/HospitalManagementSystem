using BusinessObject.Models;
using DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class MedicationDAO(HospitalManagementDbContext context)
    {
        private readonly HospitalManagementDbContext _context = context;

        public async Task<IEnumerable<Medication>> GetAllMedicationsAsync()
        {
            return await _context.Medications.ToListAsync();
        }

        public async Task<Medication?> GetMedicationByIdAsync(int id)
        {
            return await _context.Medications.FindAsync(id);
        }

        public async Task AddMedicationAsync(Medication medication)
        {
            _context.Medications.Add(medication);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMedicationAsync(Medication medication)
        {
            _context.Medications.Update(medication);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMedicationAsync(int id)
        {
            var med = await _context.Medications.FindAsync(id);
            if (med != null)
            {
                _context.Medications.Remove(med);
                await _context.SaveChangesAsync();
            }
        }
    }
}