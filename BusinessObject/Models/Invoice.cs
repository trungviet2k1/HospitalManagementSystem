namespace HospitalManagementSystem.BusinessObject.Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }
    public int PatientId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime DateIssued { get; set; }
    public string? PaymentStatus { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    //Method
    public bool IsPaid() => PaymentStatus == "Paid";
    public bool IsUnpaid() => PaymentStatus == "Unpaid";
}