using BusinessObject.Models;
using HospitalManagementSystem_WPF.ViewModel;
using System.Windows;

namespace HospitalManagementSystem_WPF.View
{
    public partial class LoginWindow : Window
    {
        public User? LoggedInUser { get; private set; }

        private readonly LoginViewModel _loginModel;
        private readonly MainViewModel _mainViewModel;

        public LoginWindow(LoginViewModel viewModel, MainViewModel mainViewModel)
        {
            InitializeComponent();
            _loginModel = viewModel;
            _mainViewModel = mainViewModel;
            DataContext = _loginModel;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = await _loginModel.LoginAsync(username, password);

            if (user == null)
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!user.Status)
            {
                MessageBox.Show("Your account is inactive. Please contact admin.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Đăng nhập thành công
            LoggedInUser = user;
            _mainViewModel.SetRole(user.Role);
            DialogResult = true;
            Close();
        }
    }
}