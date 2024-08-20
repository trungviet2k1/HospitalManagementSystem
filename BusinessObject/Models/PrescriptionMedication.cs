namespace HospitalManagementSystem.BusinessObject.Models;

public partial class PrescriptionMedication
{
    public int PrescriptionMedicationId { get; set; }
    public int PrescriptionId { get; set; }
    public int MedicationId { get; set; }
    public int Quantity { get; set; }

    public virtual Prescription Prescription { get; set; } = null!;
    public virtual Medication Medication { get; set; } = null!;

    // Methods
    public decimal GetTotalPrice() => Quantity * Medication.Price;
}