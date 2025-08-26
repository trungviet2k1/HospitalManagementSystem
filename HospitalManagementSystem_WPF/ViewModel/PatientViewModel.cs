using BusinessObject.Models;
using DataAccess.Repositories.IRepository;
using HospitalManagementSystem_WPF.View;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class PatientViewModel : BaseViewModel, ICrudOperations
    {
        public ICommand ViewPatientAppointmentsCommand { get; }
        public ICommand ScheduleAppointmentCommand { get; }
        public ICommand LoadPatientsCommand { get; }

        private readonly IPatientRepository _patientRepository;
        private readonly IUserRepository _userRepository;
        private ObservableCollection<Patient> _patientList = new();
        private Patient? _selectedPatient;

        public ObservableCollection<Patient> PatientList
        {
            get => _patientList;
            set => SetProperty(ref _patientList, value);
        }

        public Patient? SelectedPatient
        {
            get => _selectedPatient;
            set => SetProperty(ref _selectedPatient, value);
        }

        public PatientViewModel(IPatientRepository patientRepository, IUserRepository userRepository)
        {
            _patientRepository = patientRepository;
            _userRepository = userRepository;

            // Sử dụng lambda expressions với tham số
            ViewPatientAppointmentsCommand = new RelayCommand(
                execute: (parameter) => ViewPatientAppointments(parameter),
                canExecute: (parameter) => CanViewPatientAppointments(parameter)
            );

            ScheduleAppointmentCommand = new RelayCommand(
                execute: (parameter) => ScheduleAppointment(parameter),
                canExecute: (parameter) => CanScheduleAppointment(parameter)
            );

            LoadPatientsCommand = new RelayCommand(
                execute: async (parameter) => await LoadPatientsAsync(),
                canExecute: (parameter) => true
            );

            _ = LoadPatientsAsync();
        }

        private bool CanViewPatientAppointments(object? parameter)
        {
            return SelectedPatient != null;
        }

        private bool CanScheduleAppointment(object? parameter)
        {
            return SelectedPatient != null;
        }

        public async Task LoadPatientsAsync()
        {
            var patients = await _patientRepository.GetAllPatientsAsync();
            PatientList = [.. patients];
        }

        // ICrudOperations implementation
        public async void Add()
        {
            var patient = new Patient();

            var dialog = new PatientDialogWindow(patient, _userRepository)
            {
                Owner = Application.Current.MainWindow
            };

            if (dialog.ShowDialog() == true)
            {
                await _patientRepository.AddPatientAsync(patient);
                PatientList?.Add(patient);
                SelectedPatient = patient;
            }
        }

        public async void Edit()
        {
            if (SelectedPatient == null) return;

            var dialog = new PatientDialogWindow(SelectedPatient, _userRepository)
            {
                Owner = Application.Current.MainWindow
            };

            if (dialog.ShowDialog() == true)
            {
                await _patientRepository.UpdatePatientAsync(SelectedPatient);
                var index = PatientList?.IndexOf(SelectedPatient) ?? -1;
                if (index >= 0 && PatientList != null)
                    PatientList[index] = SelectedPatient;
            }
        }

        public async void Delete()
        {
            if (SelectedPatient == null)
            {
                MessageBox.Show("Vui lòng chọn bệnh nhân để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa bệnh nhân {SelectedPatient.FullName}?",
                                         "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                await _patientRepository.DeletePatientAsync(SelectedPatient.PatientId);
                PatientList?.Remove(SelectedPatient);
                SelectedPatient = null;
            }
        }

        private async void ViewPatientAppointments(object? parameter)
        {
            if (SelectedPatient == null) return;

            try
            {
                if (Application.Current.MainWindow?.DataContext is MainViewModel mainVM)
                {
                    mainVM.SelectedTabIndex = 4; // Appointments tab
                    await Task.Delay(100);

                    if (mainVM.CurrentViewModel is AppointmentViewModel appointmentVM)
                    {
                        await appointmentVM.LoadAppointmentsByPatientAsync(SelectedPatient.PatientId);
                        MessageBox.Show($"Đang hiển thị {appointmentVM.AppointmentList.Count} lịch hẹn của {SelectedPatient.FullName}",
                            "Thông tin", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xem lịch hẹn: {ex.Message}",
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ScheduleAppointment(object? parameter)
        {
            if (SelectedPatient == null) return;

            try
            {
                if (Application.Current.MainWindow?.DataContext is MainViewModel mainVM)
                {
                    mainVM.SelectedTabIndex = 4; // Appointments tab

                    if (mainVM.CurrentViewModel is AppointmentViewModel appointmentVM)
                    {
                        appointmentVM.AddWithSelectedPatient(SelectedPatient.PatientId);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đặt lịch hẹn mới: {ex.Message}",
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}