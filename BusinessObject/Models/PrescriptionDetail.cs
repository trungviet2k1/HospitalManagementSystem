namespace BusinessObject.Models;

public partial class PrescriptionDetail
{
    public int PrescriptionDetailId { get; set; }

    public int PrescriptionId { get; set; }

    public int MedicationId { get; set; }

    public string? Dosage { get; set; }

    public string? Duration { get; set; }

    public virtual Medication Medication { get; set; } = null!;

    public virtual Prescription Prescription { get; set; } = null!;
}