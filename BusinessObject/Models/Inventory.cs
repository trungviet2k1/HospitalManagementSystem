namespace BusinessObject.Models;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public int MedicationId { get; set; }

    public int Quantity { get; set; }

    public string? Location { get; set; }

    public virtual Medication Medication { get; set; } = null!;
}