using BusinessObject.Models;
using DataAccess.Repositories.IRepository;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace HospitalManagementSystem_WPF.View
{
    public partial class PatientDialogWindow : Window, INotifyPropertyChanged
    {
        public Patient Patient { get; set; }
        public List<User> AvailableDoctors { get; set; }
        private readonly IUserRepository _userRepository;

        public ICommand OkCommand { get; }

        public PatientDialogWindow(Patient patient, IUserRepository userRepository)
        {
            InitializeComponent();
            _userRepository = userRepository;
            Patient = patient ?? new Patient();
            AvailableDoctors = new List<User>();

            OkCommand = new RelayCommand(OkExecute, CanOkExecute);
            DataContext = this;

            LoadDoctorsAsync();
        }

        private async void LoadDoctorsAsync()
        {
            try
            {
                var allUsers = await _userRepository.GetAllUsersAsync();
                // Filter for doctors (you might need to adjust this logic based on your role system)
                AvailableDoctors = allUsers
                    .Where(u => u.Role?.RoleName?.ToLower().Contains("doctor") == true ||
                               u.Role?.RoleName?.ToLower().Contains("dr") == true)
                    .ToList();

                OnPropertyChanged(nameof(AvailableDoctors));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading doctors: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OkExecute(object? obj)
        {
            DialogResult = true;
            Close();
        }

        private bool CanOkExecute(object? obj)
        {
            // Kiểm tra đầy đủ dữ liệu bắt buộc
            return !string.IsNullOrWhiteSpace(Patient.FullName);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // RelayCommand đơn giản
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter) => _execute(parameter);

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}