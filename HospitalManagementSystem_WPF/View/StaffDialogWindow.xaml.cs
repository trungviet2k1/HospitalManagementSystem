using System.Collections.ObjectModel;
using System.Windows;
using BusinessObject.Models;

namespace HospitalManagementSystem_WPF.View
{
    public partial class StaffDialogWindow : Window
    {
        public User Staff { get; private set; }

        public ObservableCollection<Role> Roles { get; set; } = [];

        // SelectedRole dùng cho ComboBox binding
        public Role? SelectedRole { get; set; }

        public StaffDialogWindow(User staff, ObservableCollection<Role> roles)
        {
            InitializeComponent();
            Staff = staff;
            Roles = roles;

            // Gán SelectedRole từ collection Roles để ComboBox hiển thị đúng
            SelectedRole = Roles?.FirstOrDefault(r => r.RoleId == staff.RoleId);

            DataContext = this;
            PasswordBox.Password = staff.PasswordHash;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Cập nhật mật khẩu
            Staff.PasswordHash = PasswordBox.Password;

            // Cập nhật Role nếu chọn
            if (SelectedRole != null)
            {
                Staff.Role = SelectedRole;
                Staff.RoleId = SelectedRole.RoleId;
            }

            DialogResult = true;
        }
    }
}