namespace BusinessObject.Models;

public partial class Billing
{
    public int BillingId { get; set; }

    public int PatientId { get; set; }

    public decimal TotalAmount { get; set; }

    public DateOnly DateIssued { get; set; }

    public virtual Patient Patient { get; set; } = null!;
}