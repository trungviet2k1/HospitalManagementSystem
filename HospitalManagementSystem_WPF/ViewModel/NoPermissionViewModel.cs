using System.Windows;
using System.Windows.Input;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class NoPermissionViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainViewModel;

        public string Message { get; } = "Tài khoản của bạn không có quyền truy cập vào hệ thống. Vui lòng liên hệ quản trị viên.";

        public ICommand ContactAdminCommand { get; }
        public ICommand LogoutCommand { get; }

        // Thêm constructor nhận MainViewModel
        public NoPermissionViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            ContactAdminCommand = new RelayCommand((param) => ContactAdmin());
            LogoutCommand = new RelayCommand(async (param) => await Logout());
        }

        private static void ContactAdmin()
        {
            MessageBox.Show("Liên hệ quản trị viên:\n- Email: admin@hms.com\n- SĐT: 0900.000.001",
                "Thông tin liên hệ", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async Task Logout()
        {
           await _mainViewModel.Logout();
        }
    }
}