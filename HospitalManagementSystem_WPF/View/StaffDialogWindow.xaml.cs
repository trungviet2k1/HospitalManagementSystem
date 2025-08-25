using System.Collections.ObjectModel;
using System.Windows;
using BusinessObject.Models;

namespace HospitalManagementSystem_WPF.View
{
    public partial class StaffDialogWindow : Window
    {
        public User User { get; private set; }
        public ObservableCollection<Role> Roles { get; set; } = [];
        public Role? SelectedRole { get; set; }
        public ObservableCollection<Department> Departments { get; set; } = [];
        public Department? SelectedDepartment { get; set; }

        public string EmailPrefix
        {
            get => User?.Email != null && User.Email.EndsWith("@hsm.com")
                   ? User.Email.Replace("@hsm.com", "")
                   : User?.Email ?? "";
            set
            {
                if (User != null)
                {
                    User.Email = value + "@hsm.com";
                }
            }
        }   

        public StaffDialogWindow(User user, ObservableCollection<Role> roles, ObservableCollection<Department> departments)
        {
            InitializeComponent();
            User = user;
            Roles = roles ?? [];
            Departments = departments ?? [];

            SelectedRole = Roles.FirstOrDefault(r => r.RoleId == user.RoleId);

            // Lấy Department từ User.Staff nếu có
            if (user.Staff.Count > 0)
                SelectedDepartment = user.Staff.First().Department;
            else
                SelectedDepartment = Departments.FirstOrDefault(); // fallback

            DataContext = this;
            PasswordBox.Password = user.PasswordHash;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            User.PasswordHash = PasswordBox.Password;

            if (SelectedRole != null)
            {
                User.Role = SelectedRole;
                User.RoleId = SelectedRole.RoleId;
            }

            if (SelectedDepartment != null)
            {
                Staff staff;
                if (User.Staff.Count == 0)
                {
                    staff = new Staff { User = User, UserId = User.UserId };
                    User.Staff.Add(staff);
                }
                else
                {
                    staff = User.Staff.First();
                }

                staff.Department = SelectedDepartment;
                staff.DepartmentId = SelectedDepartment.DepartmentId;
            }

            DialogResult = true;
        }
    }
}