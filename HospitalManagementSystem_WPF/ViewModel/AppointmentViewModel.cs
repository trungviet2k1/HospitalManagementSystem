using BusinessObject.Models;
using DataAccess.Repositories.IRepository;
using HospitalManagementSystem_WPF.View;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class AppointmentViewModel : BaseViewModel, ICrudOperations
    {
        public ICommand ViewPatientDetailsCommand { get; }

        public ICommand LoadAppointmentsCommand { get; }

        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPatientRepository _patientRepository;

        private ObservableCollection<Appointment> _appointmentList = new();
        private Appointment? _selectedAppointment;

        public ObservableCollection<Appointment> AppointmentList
        {
            get => _appointmentList;
            set => SetProperty(ref _appointmentList, value);
        }

        public Appointment? SelectedAppointment
        {
            get => _selectedAppointment;
            set => SetProperty(ref _selectedAppointment, value);
        }

        public AppointmentViewModel(IAppointmentRepository appointmentRepository,
                                    IUserRepository userRepository,
                                    IPatientRepository patientRepository)
        {
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
            _patientRepository = patientRepository;

            // Lambda expressions
            ViewPatientDetailsCommand = new RelayCommand(
                execute: (param) => ViewPatientDetails(param),
                canExecute: (param) => CanViewPatientDetails(param)
            );

            // sử dụng AsyncRelayCommand cho ASYNC METHODS
            LoadAppointmentsCommand = new AsyncRelayCommand(
                execute: async (param) => await LoadAppointmentsAsync(),
                canExecute: (param) => true
            );

            _ = LoadAppointmentsAsync();
        }

        private bool CanViewPatientDetails(object? parameter)
        {
            return SelectedAppointment?.Patient != null;
        }

        private async void ViewPatientDetails(object? parameter)
        {
            if (SelectedAppointment?.Patient == null) return;

            try
            {
                if (Application.Current.MainWindow?.DataContext is MainViewModel mainVM)
                {
                    // Chuyển sang tab Patients
                    mainVM.SelectedTabIndex = 3;
                    await Task.Delay(100);

                    // Select patient tương ứng
                    if (mainVM.CurrentViewModel is PatientViewModel patientVM)
                    {
                        await patientVM.LoadPatientsAsync();
                        var patient = patientVM.PatientList?
                            .FirstOrDefault(p => p.PatientId == SelectedAppointment.PatientId);

                        if (patient != null)
                        {
                            patientVM.SelectedPatient = patient;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xem thông tin bệnh nhân: {ex.Message}",
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task LoadAppointmentsAsync()
        {
            try
            {
                var appointments = await _appointmentRepository.GetAllAppointmentsAsync();
                AppointmentList = new ObservableCollection<Appointment>(appointments);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải lịch hẹn: {ex.Message}",
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void Add()
        {
            var appointment = new Appointment
            {
                AppointmentDate = DateTime.Now.AddHours(1)
            };

            var dialog = new AppointmentDialogWindow(appointment, _userRepository, _patientRepository)
            {
                Owner = Application.Current.MainWindow
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    // Check if time slot is available
                    var isAvailable = await _appointmentRepository.IsTimeSlotAvailableAsync(
                        appointment.DoctorId, appointment.AppointmentDate);

                    if (!isAvailable)
                    {
                        MessageBox.Show("Bác sĩ đã có lịch hẹn trong khoảng thời gian này. Vui lòng chọn thời gian khác.",
                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    await _appointmentRepository.AddAppointmentAsync(appointment);

                    // Load lại appointment với đầy đủ navigation properties
                    var fullAppointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(appointment.AppointmentId);

                    if (fullAppointment != null)
                    {
                        AppointmentList.Add(fullAppointment);
                        SelectedAppointment = fullAppointment;
                    }
                    else
                    {
                        // Fallback: thêm appointment cũ nếu không load được
                        AppointmentList.Add(appointment);
                        SelectedAppointment = appointment;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm lịch hẹn: {ex.Message}",
                        "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public async void Edit()
        {
            if (SelectedAppointment == null) return;

            var dialog = new AppointmentDialogWindow(SelectedAppointment, _userRepository, _patientRepository)
            {
                Owner = Application.Current.MainWindow
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    // Check if time slot is available (excluding current appointment)
                    var isAvailable = await _appointmentRepository.IsTimeSlotAvailableAsync(
                        SelectedAppointment.DoctorId, SelectedAppointment.AppointmentDate,
                        SelectedAppointment.AppointmentId);

                    if (!isAvailable)
                    {
                        MessageBox.Show("Bác sĩ đã có lịch hẹn trong khoảng thời gian này. Vui lòng chọn thời gian khác.",
                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    await _appointmentRepository.UpdateAppointmentAsync(SelectedAppointment);

                    // Load lại appointment với đầy đủ navigation properties
                    var updatedAppointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(SelectedAppointment.AppointmentId);

                    if (updatedAppointment != null)
                    {
                        var index = AppointmentList.IndexOf(SelectedAppointment);
                        if (index >= 0)
                        {
                            AppointmentList[index] = updatedAppointment;
                            SelectedAppointment = updatedAppointment;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi cập nhật lịch hẹn: {ex.Message}",
                        "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public async void Delete()
        {
            if (SelectedAppointment == null)
            {
                MessageBox.Show("Vui lòng chọn lịch hẹn để xóa.", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa lịch hẹn của {SelectedAppointment.Patient?.FullName}?",
                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                await _appointmentRepository.DeleteAppointmentAsync(SelectedAppointment.AppointmentId);
                AppointmentList.Remove(SelectedAppointment);
                SelectedAppointment = null;
            }
        }

        public async Task LoadAppointmentsByDoctorAsync(int doctorId)
        {
            var appointments = await _appointmentRepository.GetAppointmentsByDoctorAsync(doctorId);
            AppointmentList = new ObservableCollection<Appointment>(appointments);
        }

        public async Task LoadAppointmentsByPatientAsync(int patientId)
        {
            try
            {
                var appointments = await _appointmentRepository.GetAppointmentsByPatientAsync(patientId);
                AppointmentList = new ObservableCollection<Appointment>(appointments);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải lịch hẹn: {ex.Message}",
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task LoadUpcomingAppointmentsAsync(int daysAhead = 7)
        {
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(daysAhead);
            var appointments = await _appointmentRepository.GetUpcomingAppointmentsAsync(startDate, endDate);
            AppointmentList = new ObservableCollection<Appointment>(appointments);
        }

        public async void AddWithSelectedPatient(int patientId)
        {
            var appointment = new Appointment
            {
                AppointmentDate = DateTime.Now.AddHours(1),
                PatientId = patientId  // ⭐ Set patientId từ patient được chọn
            };

            var dialog = new AppointmentDialogWindow(appointment, _userRepository, _patientRepository)
            {
                Owner = Application.Current.MainWindow
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var isAvailable = await _appointmentRepository.IsTimeSlotAvailableAsync(
                        appointment.DoctorId, appointment.AppointmentDate);

                    if (!isAvailable)
                    {
                        MessageBox.Show("Bác sĩ đã có lịch hẹn trong khoảng thời gian này. Vui lòng chọn thời gian khác.",
                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    await _appointmentRepository.AddAppointmentAsync(appointment);

                    // Load lại với đầy đủ thông tin
                    var fullAppointment = await _appointmentRepository.GetAppointmentByIdAsync(appointment.AppointmentId);

                    if (fullAppointment != null)
                    {
                        AppointmentList.Add(fullAppointment);
                        SelectedAppointment = fullAppointment;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm lịch hẹn: {ex.Message}",
                        "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}