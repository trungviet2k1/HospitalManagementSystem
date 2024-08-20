namespace HospitalManagementSystem.BusinessObject.Models;

public partial class Prescription
{
    public int PrescriptionId { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public string MedicationDetails { get; set; } = null!;
    public DateTime DatePrescribed { get; set; }

    public virtual Patient Patient { get; set; } = null!;
    public virtual Doctor Doctor { get; set; } = null!;
    public virtual ICollection<PrescriptionMedication> PrescriptionMedications { get; set; } = new List<PrescriptionMedication>();

    // Methods
    public string GetMedicationSummary() => MedicationDetails;
}