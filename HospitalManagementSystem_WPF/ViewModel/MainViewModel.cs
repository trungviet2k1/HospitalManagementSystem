using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private IServiceProvider _serviceProvider;
        private BaseViewModel _currentViewModel;
        private int _selectedTabIndex;

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                if (_selectedTabIndex == value) return;
                _selectedTabIndex = value;
                OnPropertyChanged(nameof(SelectedTabIndex));
                UpdateCurrentViewModelBasedOnTab();
            }
        }

        public ICommand ShowRoomsCommand { get; }
        public ICommand ShowDepartmentsCommand { get; }
        public ICommand ShowDoctorsCommand { get; }
        public ICommand ShowMedicationsCommand { get; }
        public ICommand ShowPatientsCommand { get; }
        public ICommand ShowInvoicesCommand { get; }
        public ICommand ShowStaffCommand { get; }
        public ICommand ShowStaffListCommand { get; }
        public ICommand SelectTabCommand { get; }

        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            ShowRoomsCommand = new RelayCommand(ExecuteShowRooms);
            ShowDepartmentsCommand = new RelayCommand(ExecuteShowDepartments);
            ShowDoctorsCommand = new RelayCommand(ExecuteShowDoctors);
            ShowMedicationsCommand = new RelayCommand(ExecuteShowMedications);
            ShowPatientsCommand = new RelayCommand(ExecuteShowPatients);
            ShowInvoicesCommand = new RelayCommand(ExecuteShowInvoices);
            ShowStaffCommand = new RelayCommand(ExecuteShowStaff);
            ShowStaffListCommand = new RelayCommand<object>(ExecuteShowStaffList);
            SelectTabCommand = new RelayCommand<int>(ExecuteSelectTab);
        }

        private void UpdateCurrentViewModelBasedOnTab()
        {
            switch (SelectedTabIndex)
            {
                case 0: // Staff tab
                    CurrentViewModel = _serviceProvider.GetRequiredService<StaffViewModel>();
                    break;
                case 1: // Departments tab
                    CurrentViewModel = _serviceProvider.GetRequiredService<DepartmentViewModel>();
                    break;
                case 2: // Rooms tab
                    CurrentViewModel = _serviceProvider.GetRequiredService<RoomViewModel>();
                    break;
                case 3: // Patients tab
                    CurrentViewModel = _serviceProvider.GetRequiredService<PatientViewModel>();
                    break;
                case 4: // Medications tab
                    CurrentViewModel = _serviceProvider.GetRequiredService<MedicationViewModel>();
                    break;
                case 5: // Invoices tab
                    CurrentViewModel = _serviceProvider.GetRequiredService<InvoiceViewModel>();
                    break;
                default:
                    break;
            }
        }

        private void ExecuteShowRooms() => SelectedTabIndex = 2;
        private void ExecuteShowDepartments() => SelectedTabIndex = 1;
        private void ExecuteShowDoctors() => SelectedTabIndex = 0;
        private void ExecuteShowMedications() => SelectedTabIndex = 4;
        private void ExecuteShowPatients() => SelectedTabIndex = 3;
        private void ExecuteShowInvoices() => SelectedTabIndex = 5;
        private void ExecuteShowStaff() => SelectedTabIndex = 0;
        private void ExecuteShowStaffList(object parameter)
        {
            if (parameter is string category)
            {
                switch (category)
                {
                    case "Doctors":
                        CurrentViewModel = _serviceProvider.GetRequiredService<DoctorViewModel>();
                        break;
                    case "Nurses":
                        CurrentViewModel = _serviceProvider.GetRequiredService<NurseViewModel>();
                        break;
                    case "Receptionists":
                        CurrentViewModel = _serviceProvider.GetRequiredService<ReceptionistViewModel>();
                        break;
                    default:
                        break;
                }
            }
        }
        private void ExecuteSelectTab(int tabIndex) => SelectedTabIndex = tabIndex;
    }
}