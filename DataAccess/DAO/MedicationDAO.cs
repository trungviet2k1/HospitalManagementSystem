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
            return await _context.Medications
                .Where(m => !m.IsDeleted) // chỉ lấy những thuốc chưa bị xóa
                .Include(m => m.Inventories)
                .ToListAsync();
        }

        public async Task<Medication?> GetMedicationByIdAsync(int id)
        {
            return await _context.Medications
                .Where(m => !m.IsDeleted) // Thêm điều kiện này
                .Include(m => m.Inventories)
                .FirstOrDefaultAsync(m => m.MedicationId == id);
        }

        public async Task AddMedicationAsync(Medication medication)
        {
            // Kiểm tra thuốc đã tồn tại trong DB chưa (bao gồm thuốc bị soft delete)
            var medName = medication.MedicationName.Trim().ToLower();
            var unit = medication.Unit?.Trim().ToLower() ?? "";

            // Tìm thuốc đã tồn tại (bao gồm cả đã soft delete)
            var existingMedication = await _context.Medications
                .AsNoTracking() // thêm dòng này
                .Include(m => m.Inventories)
                .FirstOrDefaultAsync(m => m.MedicationName.ToLower().Trim() == medName && (m.Unit ?? "").ToLower().Trim() == unit);

            if (existingMedication != null)
            {
                if (existingMedication.IsDeleted)
                {
                    // Thuốc đã soft delete -> khôi phục và CẬP NHẬT HOÀN TOÀN
                    existingMedication.IsDeleted = false;
                    existingMedication.QuantityInStock = medication.QuantityInStock; // Cập nhật số lượng mới
                    existingMedication.Unit = medication.Unit;

                    // Xóa tất cả inventory cũ và thêm mới
                    _context.Inventories.RemoveRange(existingMedication.Inventories);

                    // Thêm inventory mới
                    if (medication.Inventories.Any())
                    {
                        var newInv = medication.Inventories.First();
                        existingMedication.Inventories.Add(new Inventory
                        {
                            MedicationId = existingMedication.MedicationId,
                            Quantity = newInv.Quantity,
                            Location = newInv.Location,
                            LastUpdated = DateTime.Now
                        });
                    }

                    _context.Medications.Update(existingMedication);
                    _context.Entry(existingMedication).State = EntityState.Modified;
                }
                else
                {
                    // Thuốc đang active -> cộng thêm số lượng
                    existingMedication.QuantityInStock += medication.QuantityInStock;

                    if (medication.Inventories.Any())
                    {
                        var newInv = medication.Inventories.First();
                        var existingInv = existingMedication.Inventories.FirstOrDefault();

                        if (existingInv != null)
                        {
                            existingInv.Quantity += newInv.Quantity;
                            existingInv.LastUpdated = DateTime.Now;
                        }
                        else
                        {
                            existingMedication.Inventories.Add(new Inventory
                            {
                                MedicationId = existingMedication.MedicationId,
                                Quantity = newInv.Quantity,
                                Location = newInv.Location,
                                LastUpdated = DateTime.Now
                            });
                        }
                    }

                    _context.Medications.Update(existingMedication);
                }
            }
            else
            {
                // Nếu hoàn toàn mới thì insert
                medication.IsDeleted = false;
                _context.Medications.Add(medication);
            }

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
                med.IsDeleted = true; // soft delete
                _context.Medications.Update(med);
                await _context.SaveChangesAsync();
            }
        }
    }
}