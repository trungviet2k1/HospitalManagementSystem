using HospitalManagementSystem.HospitalManagementSystem_WPF;
using HospitalManagementSystem_WPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace HospitalManagementSystem_WPF.View
{
    public partial class LoginWindow : Window
    {
        private readonly LoginViewModel _viewModel;

        public LoginWindow()
        {
            InitializeComponent();
            _viewModel = ServiceLocator.ServiceProvider.GetRequiredService<LoginViewModel>();
            DataContext = _viewModel;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            bool loginSuccessful = await _viewModel.LoginAsync(username, password);

            if (loginSuccessful)
            {
                var mainWindow = ServiceLocator.ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}