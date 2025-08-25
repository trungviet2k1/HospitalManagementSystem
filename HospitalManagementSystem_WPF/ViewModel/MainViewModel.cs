using System.Windows;
using System.Windows.Input;
using BusinessObject.Models;
using DataAccess.Repositories.IRepository;
using HospitalManagementSystem.HospitalManagementSystem_WPF;
using HospitalManagementSystem_WPF.View;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private IServiceProvider _serviceProvider;
        private BaseViewModel? _currentViewModel = null;
        private int _selectedTabIndex;

        public User? CurrentUser { get; private set; }

        // Properties for tab visibility
        public Visibility StaffTabVisibility { get; private set; } = Visibility.Visible;
        public Visibility DepartmentTabVisibility { get; private set; } = Visibility.Visible;
        public Visibility RoomTabVisibility { get; private set; } = Visibility.Visible;
        public Visibility PatientTabVisibility { get; private set; } = Visibility.Visible;
        public Visibility MedicationTabVisibility { get; private set; } = Visibility.Visible;
        public Visibility InvoiceTabVisibility { get; private set; } = Visibility.Visible;

        // Properties for button visibility
        public Visibility AddButtonVisibility { get; private set; } = Visibility.Visible;
        public Visibility EditButtonVisibility { get; private set; } = Visibility.Visible;
        public Visibility DeleteButtonVisibility { get; private set; } = Visibility.Visible;

        public BaseViewModel? CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                if (SetProperty(ref _selectedTabIndex, value))
                {
                    UpdateCurrentViewModelBasedOnTab();
                }
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
        public ICommand LogoutCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

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
            LogoutCommand = new RelayCommand(Logout);
            AddCommand = new RelayCommand(ExecuteAdd);
            EditCommand = new RelayCommand(ExecuteEdit);
            DeleteCommand = new RelayCommand(ExecuteDelete);
        }

        private void UpdateCurrentViewModelBasedOnTab()
        {
            switch (SelectedTabIndex)
            {
                case 0: // Staff tab
                    var staffVM = _serviceProvider.GetRequiredService<StaffViewModel>();
                    staffVM.CurrentUser = CurrentUser; // <-- gán CurrentUser
                    CurrentViewModel = staffVM;
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
        private void ExecuteAdd()
        {
            if (CurrentViewModel is ICrudOperations crudVm)
            {
                crudVm.Add();
            }
        }

        private void ExecuteEdit()
        {
            if (CurrentViewModel is ICrudOperations crudVm)
            {
                crudVm.Edit();
            }
        }

        private void ExecuteDelete()
        {
            if (CurrentViewModel is ICrudOperations crudVm)
            {
                crudVm.Delete();
            }
        }

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

        public void SetRole(Role role, User? user = null)
        {
            CurrentUser = user;

            StaffTabVisibility = Visibility.Collapsed;
            DepartmentTabVisibility = Visibility.Collapsed;
            RoomTabVisibility = Visibility.Collapsed;
            PatientTabVisibility = Visibility.Collapsed;
            MedicationTabVisibility = Visibility.Collapsed;
            InvoiceTabVisibility = Visibility.Collapsed;

            AddButtonVisibility = Visibility.Collapsed;
            EditButtonVisibility = Visibility.Collapsed;
            DeleteButtonVisibility = Visibility.Collapsed;

            // luôn cho xem Patient
            PatientTabVisibility = Visibility.Visible;

            bool defaultTabSet = false;

            if (role?.RolePermissions != null)
            {
                foreach (var rp in role.RolePermissions)
                {
                    switch (rp.Permission.PermissionName)
                    {
                        case "ViewStaff":
                            StaffTabVisibility = Visibility.Visible;
                            if (!defaultTabSet)
                            {
                                SelectedTabIndex = 0;
                                CurrentViewModel = _serviceProvider.GetRequiredService<StaffViewModel>();
                                defaultTabSet = true;
                            }
                            break;

                        case "ViewDepartment":
                            DepartmentTabVisibility = Visibility.Visible;
                            if (!defaultTabSet)
                            {
                                SelectedTabIndex = 1;
                                CurrentViewModel = _serviceProvider.GetRequiredService<DepartmentViewModel>();
                                defaultTabSet = true;
                            }
                            break;

                        case "ViewRoom":
                            RoomTabVisibility = Visibility.Visible;
                            if (!defaultTabSet)
                            {
                                SelectedTabIndex = 2;
                                CurrentViewModel = _serviceProvider.GetRequiredService<RoomViewModel>();
                                defaultTabSet = true;
                            }
                            break;

                        case "ViewMedication":
                            MedicationTabVisibility = Visibility.Visible;
                            if (!defaultTabSet)
                            {
                                SelectedTabIndex = 4;
                                CurrentViewModel = _serviceProvider.GetRequiredService<MedicationViewModel>();
                                defaultTabSet = true;
                            }
                            break;

                        case "ViewInvoice":
                            InvoiceTabVisibility = Visibility.Visible;
                            if (!defaultTabSet)
                            {
                                SelectedTabIndex = 5;
                                CurrentViewModel = _serviceProvider.GetRequiredService<InvoiceViewModel>();
                                defaultTabSet = true;
                            }
                            break;
                    }

                    switch (rp.Permission.PermissionName)
                    {
                        case "Add": AddButtonVisibility = Visibility.Visible; break;
                        case "Edit": EditButtonVisibility = Visibility.Visible; break;
                        case "Delete": DeleteButtonVisibility = Visibility.Visible; break;
                    }
                }
            }

            // Nếu vẫn chưa chọn tab nào (role không có gì ngoài Patient)
            if (!defaultTabSet)
            {
                SelectedTabIndex = 3;
                CurrentViewModel = _serviceProvider.GetRequiredService<PatientViewModel>();
            }

            // notify UI
            OnPropertyChanged(nameof(StaffTabVisibility));
            OnPropertyChanged(nameof(DepartmentTabVisibility));
            OnPropertyChanged(nameof(RoomTabVisibility));
            OnPropertyChanged(nameof(PatientTabVisibility));
            OnPropertyChanged(nameof(MedicationTabVisibility));
            OnPropertyChanged(nameof(InvoiceTabVisibility));
            OnPropertyChanged(nameof(AddButtonVisibility));
            OnPropertyChanged(nameof(EditButtonVisibility));
            OnPropertyChanged(nameof(DeleteButtonVisibility));
        }

        public void UpdateCurrentUserIfEdited(User editedUser)
        {
            if (CurrentUser != null && editedUser.UserId == CurrentUser.UserId)
            {
                // Update role ngay lập tức
                SetRole(editedUser.Role, editedUser);
            }
        }

        private void Logout()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                mainWindow?.Hide();

                var loginWindow = App.ServiceProvider.GetRequiredService<LoginWindow>();
                var result = loginWindow.ShowDialog();

                if (result == true && loginWindow.LoggedInUser != null)
                {
                    // Reset role cho MainWindow cũ
                    var mainVM = (MainViewModel)mainWindow.DataContext;
                    mainVM.SetRole(loginWindow.LoggedInUser.Role);

                    mainWindow.Show();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            });
        }
    }
}