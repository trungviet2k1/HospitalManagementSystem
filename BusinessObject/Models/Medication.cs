namespace BusinessObject.Models;

public partial class Medication
{
    public int MedicationId { get; set; }

    public string MedicationName { get; set; } = null!;

    public int QuantityInStock { get; set; }

    public string? Unit { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<PrescriptionDetail> PrescriptionDetails { get; set; } = new List<PrescriptionDetail>();
}