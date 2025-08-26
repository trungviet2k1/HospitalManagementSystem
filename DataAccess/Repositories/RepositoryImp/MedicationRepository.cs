using BusinessObject.Models;
using DataAccess.DAO;
using DataAccess.Repositories.IRepository;

namespace DataAccess.Repositories.RepositoryImp
{
    public class MedicationRepository(MedicationDAO dao) : IMedicationRepository
    {
        public async Task<IEnumerable<Medication>> GetAllMedicationsAsync() => await dao.GetAllMedicationsAsync();
        public async Task<Medication?> GetMedicationByIdAsync(int id) => await dao.GetMedicationByIdAsync(id);
        public async Task AddMedicationAsync(Medication medication) => await dao.AddMedicationAsync(medication);
        public async Task UpdateMedicationAsync(Medication medication) => await dao.UpdateMedicationAsync(medication);
        public async Task DeleteMedicationAsync(int id) => await dao.DeleteMedicationAsync(id); // đã soft delete
    }
}