namespace HospitalManagementSystem.BusinessObject.Models;

public partial class Medication
{
    public int MedicationId { get; set; }
    public string MedicationName { get; set; } = null!;
    public string? Description { get; set; }
    public int StockQuantity { get; set; }
    public decimal Price { get; set; }

    public virtual ICollection<PrescriptionMedication> PrescriptionMedications { get; set; } = new List<PrescriptionMedication>();

    //Method
    public bool IsInStock() => StockQuantity > 0;
    public void UpdateStock(int quantity) => StockQuantity -= quantity;
}