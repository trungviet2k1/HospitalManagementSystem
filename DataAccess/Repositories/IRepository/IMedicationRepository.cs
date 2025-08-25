using BusinessObject.Models;

namespace DataAccess.Repositories.IRepository
{
    public interface IMedicationRepository
    {
        Task<IEnumerable<Medication>> GetAllMedicationsAsync();
        Task<Medication?> GetMedicationByIdAsync(int id);
        Task AddMedicationAsync(Medication medication);
        Task UpdateMedicationAsync(Medication medication);
        Task DeleteMedicationAsync(int id);
    }
}