using BusinessObject.Models;
using DataAccess.Repositories.IRepository;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HospitalManagementSystem_WPF.View
{
    public partial class AppointmentDialogWindow : Window, INotifyPropertyChanged
    {
        private Appointment _appointment;
        public Appointment Appointment
        {
            get => _appointment;
            set
            {
                _appointment = value;
                OnPropertyChanged(nameof(Appointment));
                UpdateDateTimeDisplay();
            }
        }

        public List<User> AvailableDoctors { get; set; }
        public List<Patient> AvailablePatients { get; set; }
        public List<int> AvailableHours { get; set; }
        public List<int> AvailableMinutes { get; set; }

        private int _selectedHour;
        public int SelectedHour
        {
            get => _selectedHour;
            set
            {
                _selectedHour = value;
                OnPropertyChanged(nameof(SelectedHour));
                UpdateDateTimeDisplay();
            }
        }

        private int _selectedMinute;
        public int SelectedMinute
        {
            get => _selectedMinute;
            set
            {
                _selectedMinute = value;
                OnPropertyChanged(nameof(SelectedMinute));
                UpdateDateTimeDisplay();
            }
        }

        public string DisplayDateTime { get; set; }
        public string StatusMessage { get; set; }
        public bool HasStatusMessage => !string.IsNullOrEmpty(StatusMessage);
        public bool IsEditMode => Appointment?.AppointmentId > 0;

        private readonly IUserRepository _userRepository;
        private readonly IPatientRepository _patientRepository;

        public ICommand OkCommand { get; }

        public AppointmentDialogWindow(Appointment appointment, IUserRepository userRepository, IPatientRepository patientRepository)
        {
            InitializeComponent();
            _userRepository = userRepository;
            _patientRepository = patientRepository;

            Appointment = appointment ?? new Appointment();
            AvailableDoctors = new List<User>();
            AvailablePatients = new List<Patient>();
            AvailableHours = Enumerable.Range(8, 10).ToList(); // 8AM to 5PM
            AvailableMinutes = new List<int> { 0, 15, 30, 45 };

            // Set default time if not set
            if (Appointment.AppointmentDate == default)
            {
                Appointment.AppointmentDate = DateTime.Now.AddHours(1);
                SelectedHour = Appointment.AppointmentDate.Hour;
                SelectedMinute = Appointment.AppointmentDate.Minute;
            }
            else
            {
                SelectedHour = Appointment.AppointmentDate.Hour;
                SelectedMinute = Appointment.AppointmentDate.Minute;
            }

            OkCommand = new RelayCommand(OkExecute, CanOkExecute);
            DataContext = this;

            Loaded += async (s, e) => await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                // Load doctors (filter for doctors only)
                var allUsers = await _userRepository.GetAllUsersAsync();
                AvailableDoctors = allUsers
                    .Where(u => u.Role?.RoleName?.ToLower().Contains("doctor") == true ||
                               u.Role?.RoleName?.ToLower().Contains("dr") == true)
                    .OrderBy(d => d.FullName)
                    .ToList();

                // Load patients
                var patients = await _patientRepository.GetAllPatientsAsync();
                AvailablePatients = patients.OrderBy(p => p.FullName).ToList();

                OnPropertyChanged(nameof(AvailableDoctors));
                OnPropertyChanged(nameof(AvailablePatients));
                OnPropertyChanged(nameof(IsEditMode));
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading data: {ex.Message}";
                OnPropertyChanged(nameof(StatusMessage));
                OnPropertyChanged(nameof(HasStatusMessage));
            }
        }

        private void UpdateDateTimeDisplay()
        {
            if (Appointment?.AppointmentDate != null)
            {
                try
                {
                    var date = Appointment.AppointmentDate.Date;
                    var time = new TimeSpan(SelectedHour, SelectedMinute, 0);
                    var dateTime = date + time;

                    DisplayDateTime = dateTime.ToString("dd/MM/yyyy HH:mm");
                }
                catch
                {
                    DisplayDateTime = "Invalid date/time";
                }
            }
            else
            {
                DisplayDateTime = "Not set";
            }
            OnPropertyChanged(nameof(DisplayDateTime));
        }

        private void OkExecute(object? obj)
        {
            try
            {
                // Combine date and time
                if (Appointment.AppointmentDate == default)
                {
                    StatusMessage = "Please select a date.";
                    OnPropertyChanged(nameof(StatusMessage));
                    OnPropertyChanged(nameof(HasStatusMessage));
                    return;
                }

                var appointmentDate = Appointment.AppointmentDate.Date;
                var combinedDateTime = new DateTime(
                    appointmentDate.Year,
                    appointmentDate.Month,
                    appointmentDate.Day,
                    SelectedHour,
                    SelectedMinute,
                    0
                );

                Appointment.AppointmentDate = combinedDateTime;

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
                OnPropertyChanged(nameof(StatusMessage));
                OnPropertyChanged(nameof(HasStatusMessage));
            }
        }

        private bool CanOkExecute(object? obj)
        {
            StatusMessage = string.Empty;

            if (Appointment.PatientId <= 0)
            {
                StatusMessage = "Please select a patient.";
            }
            else if (Appointment.DoctorId <= 0)
            {
                StatusMessage = "Please select a doctor.";
            }
            else if (Appointment.AppointmentDate < DateTime.Now.Date)
            {
                StatusMessage = "Appointment date cannot be in the past.";
            }
            else if (string.IsNullOrWhiteSpace(Appointment.Reason))
            {
                StatusMessage = "Please enter a reason for the appointment.";
            }

            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(HasStatusMessage));

            return string.IsNullOrEmpty(StatusMessage);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}