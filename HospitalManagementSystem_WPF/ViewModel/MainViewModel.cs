using System.Windows.Input;
using BusinessObject.Models;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private BaseViewModel? _currentViewModel;

        public BaseViewModel? CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public ICommand ShowRoomsCommand { get; }
        public ICommand ShowDepartmentsCommand { get; }
        public ICommand ShowDoctorsCommand { get; }
        public ICommand ShowMedicationsCommand { get; }
        public ICommand ShowPatientsCommand { get; }
        public ICommand ShowInvoicesCommand { get; }

        public MainViewModel(IServiceProvider serviceProvider, User user)
        {
            _serviceProvider = serviceProvider;

            ShowRoomsCommand = new RelayCommand(ExecuteShowRooms);
            ShowDepartmentsCommand = new RelayCommand(ExecuteShowDepartments);
            ShowDoctorsCommand = new RelayCommand(ExecuteShowDoctors);
            ShowMedicationsCommand = new RelayCommand(ExecuteShowMedications);
            ShowPatientsCommand = new RelayCommand(ExecuteShowPatients);
            ShowInvoicesCommand = new RelayCommand(ExecuteShowInvoices);
        }

        private void ExecuteShowRooms()
        {
            CurrentViewModel = _serviceProvider.GetRequiredService<RoomViewModel>();
        }

        private void ExecuteShowDepartments()
        {
            CurrentViewModel = _serviceProvider.GetRequiredService<DepartmentViewModel>();
        }

        private void ExecuteShowDoctors()
        {
            CurrentViewModel = _serviceProvider.GetRequiredService<DoctorViewModel>();
        }

        private void ExecuteShowMedications()
        {
            CurrentViewModel = _serviceProvider.GetRequiredService<MedicationViewModel>();
        }

        private void ExecuteShowPatients()
        {
            CurrentViewModel = _serviceProvider.GetRequiredService<PatientViewModel>();
        }

        private void ExecuteShowInvoices()
        {
            CurrentViewModel = _serviceProvider.GetRequiredService<InvoiceViewModel>();
        }
    }
}