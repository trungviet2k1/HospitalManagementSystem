namespace HospitalManagementSystem.BusinessObject.Models;

public partial class Patient
{
    public int PatientId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = null!;
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? InsuranceDetails { get; set; }
    public string? EmergencyContact { get; set; }
    public int? CreatedByUserId { get; set; }

    public virtual User? CreatedByUser { get; set; }
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    //Method
    public string GetFullName() => $"{FirstName} {LastName}";
    public int Age => DateTime.Now.Year - DateOfBirth.Year - (DateTime.Now.Date < DateOfBirth.Date ? 1 : 0);
}