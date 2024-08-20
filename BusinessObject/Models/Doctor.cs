namespace HospitalManagementSystem.BusinessObject.Models;

public partial class Doctor
{
    public int DoctorId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Specialization { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public int? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    //Method
    public string GetFullName() => $"{FirstName} {LastName}";
}