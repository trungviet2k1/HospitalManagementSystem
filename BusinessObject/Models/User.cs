using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BusinessObject.Models
{
    public partial class User : INotifyPropertyChanged
    {
        private int _userId;
        private string _username = null!;
        private string _passwordHash = null!;
        private string _fullName = null!;
        private string? _email;
        private string? _phoneNumber;
        private int _roleId;
        private bool _status;
        private Role? _role;

        public int UserId { get => _userId; set { _userId = value; OnPropertyChanged(); } }
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        public string PasswordHash { get => _passwordHash; set { _passwordHash = value; OnPropertyChanged(); } }
        public string FullName { get => _fullName; set { _fullName = value; OnPropertyChanged(); } }
        public string? Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        public string? PhoneNumber { get => _phoneNumber; set { _phoneNumber = value; OnPropertyChanged(); } }
        public int RoleId { get => _roleId; set { _roleId = value; OnPropertyChanged(); } }
        public bool Status { get => _status; set { _status = value; OnPropertyChanged(); } }
        public virtual Role? Role { get => _role; set { _role = value; OnPropertyChanged(); } }

        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
        public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
        public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
        public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // Methods
        public string GetUserDetails() => $"{FullName} ({Username})";
        public string DepartmentName => Staff.FirstOrDefault()?.Department?.DepartmentName ?? "N/A";
        public bool VerifyPassword(string password) => PasswordHash == HashPassword(password);
        private string HashPassword(string password) => password;
    }
}