namespace BusinessObject.Models;

public partial class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? Email { get; set; }
    public bool? IsActive { get; set; }

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();

    public virtual ICollection<UserLog> UserLogs { get; set; } = new List<UserLog>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    // Methods
    public string GetUserDetails() => $"{FullName} ({Username})";
    public bool VerifyPassword(string password) => PasswordHash == HashPassword(password);
    private string HashPassword(string password)
    {
        return password;
    }
}