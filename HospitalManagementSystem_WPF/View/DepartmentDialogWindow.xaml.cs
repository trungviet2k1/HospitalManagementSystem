using BusinessObject.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace HospitalManagementSystem_WPF.View
{
    public partial class DepartmentDialogWindow : Window
    {
        public Department Department { get; private set; }
        public ObservableCollection<User> Users { get; private set; }

        public DepartmentDialogWindow(Department department, ObservableCollection<User>? allUsers = null)
        {
            InitializeComponent();

            Department = department;
            Users = allUsers ?? [];

            DataContext = this;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra tên phòng ban
            if (string.IsNullOrWhiteSpace(Department.DepartmentName))
            {
                MessageBox.Show("Tên phòng ban không được để trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }
    }
}