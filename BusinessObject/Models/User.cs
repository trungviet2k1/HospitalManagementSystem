namespace BusinessObject.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public int RoleId { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

    // Methods
    public string GetUserDetails() => $"{FullName} ({Username})";

    public bool VerifyPassword(string password) => PasswordHash == HashPassword(password);

    private string HashPassword(string password)
    {
        return password;
    }
}