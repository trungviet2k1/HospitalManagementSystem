using HospitalManagementSystem.HospitalManagementSystem_WPF;
using HospitalManagementSystem_WPF.ViewModel;
using System.Windows;

namespace HospitalManagementSystem_WPF.View
{
    public partial class LoginWindow : Window
    {
        private LoginViewModel _loginModel;
        private MainWindow _mainWindow;

        public LoginWindow(LoginViewModel viewModel, MainWindow mainWindow)
        {
            InitializeComponent();
            _loginModel = viewModel;
            _mainWindow = mainWindow;
            DataContext = _loginModel;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            bool loginSuccessful = await _loginModel.LoginAsync(username, password);

            if (loginSuccessful)
            {
                _mainWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}