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
        private LogWindow _logWindow;
        private int _selectedTabIndex;
        private bool _hasPermission;

        // add new role button visibility
        private Visibility _newRoleButtonVisibility = Visibility.Collapsed;
        public Visibility AppointmentTabVisibility { get; private set; } = Visibility.Collapsed;

        // view logs button visibility
        private Visibility _viewLogsButtonVisibility = Visibility.Collapsed;
        public Visibility ViewLogsButtonVisibility
        {
            get => _viewLogsButtonVisibility;
            set => SetProperty(ref _viewLogsButtonVisibility, value);
        }

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
        public Visibility NewRoleButtonVisibility
        {
            get => _newRoleButtonVisibility;
            private set => SetProperty(ref _newRoleButtonVisibility, value);
        }

        public bool HasPermission
        {
            get => _hasPermission;
            set => SetProperty(ref _hasPermission, value);
        }

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
        public ICommand NewRoleCommand { get; }
        public ICommand ContactAdminCommand { get; }
        public ICommand OpenLogWindowCommand { get; }

        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            ShowRoomsCommand = new RelayCommand((param) => ExecuteShowRooms());
            ShowDepartmentsCommand = new RelayCommand((param) => ExecuteShowDepartments());
            ShowDoctorsCommand = new RelayCommand((param) => ExecuteShowDoctors());
            ShowMedicationsCommand = new RelayCommand((param) => ExecuteShowMedications());
            ShowPatientsCommand = new RelayCommand((param) => ExecuteShowPatients());
            ShowInvoicesCommand = new RelayCommand((param) => ExecuteShowInvoices());
            ShowStaffCommand = new RelayCommand((param) => ExecuteShowStaff());
            ShowStaffListCommand = new RelayCommand<object>((param) => ExecuteShowStaffList(param));
            SelectTabCommand = new RelayCommand<int>((param) => ExecuteSelectTab(param));
            LogoutCommand = new RelayCommand(async (param) => await Logout());
            AddCommand = new RelayCommand((param) => ExecuteAdd());
            EditCommand = new RelayCommand((param) => ExecuteEdit());
            DeleteCommand = new RelayCommand((param) => ExecuteDelete());
            NewRoleCommand = new RelayCommand((param) => OpenNewRoleWindow());
            ContactAdminCommand = new RelayCommand((param) => ContactAdmin());
            OpenLogWindowCommand = new RelayCommand(_ => OpenLogWindow());
        }

        private void UpdateCurrentViewModelBasedOnTab()
        {
            switch (SelectedTabIndex)
            {
                case 0: // Staff tab
                    var staffVM = new StaffViewModel(
                            _serviceProvider.GetRequiredService<IUserRepository>(),
                            _serviceProvider.GetRequiredService<IRoleRepository>(),
                            _serviceProvider.GetRequiredService<IDepartmentRepository>(),
                            _serviceProvider.GetRequiredService<ILogRepository>(),
                            CurrentUser // <-- truyền CurrentUser từ MainViewModel
                    );
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
                case 4: // Appointments tab
                    CurrentViewModel = _serviceProvider.GetRequiredService<AppointmentViewModel>();
                    break;
                case 5: // Medications tab
                    CurrentViewModel = _serviceProvider.GetRequiredService<MedicationViewModel>();
                    break;
                case 6: // Invoices tab
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

        public void SetRole(Role? role, User? user = null)
        {
            if (user != null)
                CurrentUser = user;

            OnPropertyChanged(nameof(CurrentUser));

            StaffTabVisibility = Visibility.Collapsed;
            DepartmentTabVisibility = Visibility.Collapsed;
            RoomTabVisibility = Visibility.Collapsed;
            PatientTabVisibility = Visibility.Collapsed;
            MedicationTabVisibility = Visibility.Collapsed;
            InvoiceTabVisibility = Visibility.Collapsed;
            AppointmentTabVisibility = Visibility.Collapsed;

            AddButtonVisibility = Visibility.Collapsed;
            EditButtonVisibility = Visibility.Collapsed;
            DeleteButtonVisibility = Visibility.Collapsed;
            NewRoleButtonVisibility = Visibility.Collapsed;

            bool defaultTabSet = false;
            bool hasAnyPermission = false;

            if (role?.RolePermissions != null)
            {
                foreach (var rp in role.RolePermissions)
                {
                    hasAnyPermission = true;

                    switch (rp.Permission.PermissionId)
                    {
                        case 1:
                            StaffTabVisibility = Visibility.Visible;
                            if (!defaultTabSet)
                            {
                                SelectedTabIndex = 0;
                                var staffVM = new StaffViewModel(
                                        _serviceProvider.GetRequiredService<IUserRepository>(),
                                        _serviceProvider.GetRequiredService<IRoleRepository>(),
                                        _serviceProvider.GetRequiredService<IDepartmentRepository>(),
                                        _serviceProvider.GetRequiredService<ILogRepository>(),
                                        CurrentUser // <-- truyền CurrentUser từ MainViewModel
                                );
                                CurrentViewModel = staffVM;
                                defaultTabSet = true;
                            }
                            break;

                        case 2:
                            DepartmentTabVisibility = Visibility.Visible;
                            if (!defaultTabSet)
                            {
                                SelectedTabIndex = 1;
                                CurrentViewModel = _serviceProvider.GetRequiredService<DepartmentViewModel>();
                                defaultTabSet = true;
                            }
                            break;

                        case 3:
                            RoomTabVisibility = Visibility.Visible;
                            if (!defaultTabSet)
                            {
                                SelectedTabIndex = 2;
                                CurrentViewModel = _serviceProvider.GetRequiredService<RoomViewModel>();
                                defaultTabSet = true;
                            }
                            break;

                        case 4:
                            PatientTabVisibility = Visibility.Visible;
                            if (!defaultTabSet)
                            {
                                SelectedTabIndex = 3;
                                CurrentViewModel = _serviceProvider.GetRequiredService<PatientViewModel>();
                                defaultTabSet = true;
                            }
                            break;

                        case 5:
                            MedicationTabVisibility = Visibility.Visible;
                            if (!defaultTabSet)
                            {
                                SelectedTabIndex = 5;
                                CurrentViewModel = _serviceProvider.GetRequiredService<MedicationViewModel>();
                                defaultTabSet = true;
                            }
                            break;

                        case 6:
                            InvoiceTabVisibility = Visibility.Visible;
                            if (!defaultTabSet)
                            {
                                SelectedTabIndex = 6;
                                CurrentViewModel = _serviceProvider.GetRequiredService<InvoiceViewModel>();
                                defaultTabSet = true;
                            }
                            break;

                        case 12:
                            AppointmentTabVisibility = Visibility.Visible;
                            if (!defaultTabSet)
                            {
                                SelectedTabIndex = 4;
                                CurrentViewModel = _serviceProvider.GetRequiredService<AppointmentViewModel>();
                                defaultTabSet = true;
                            }
                            break;
                    }

                    // Buttons permissions
                    switch (rp.Permission.PermissionId)
                    {
                        case 7: AddButtonVisibility = Visibility.Visible; break;
                        case 8: EditButtonVisibility = Visibility.Visible; break;
                        case 9: DeleteButtonVisibility = Visibility.Visible; break;
                    }
                }
            }

            // kiểm tra và thiết lập HasPermission
            CheckPermissions();

            // role không có permission nào
            if (!hasAnyPermission)
            {
                CurrentViewModel = _serviceProvider.GetRequiredService<NoPermissionViewModel>();

                // Ẩn tất cả tabs
                StaffTabVisibility = Visibility.Collapsed;
                DepartmentTabVisibility = Visibility.Collapsed;
                RoomTabVisibility = Visibility.Collapsed;
                PatientTabVisibility = Visibility.Collapsed;
                MedicationTabVisibility = Visibility.Collapsed;
                InvoiceTabVisibility = Visibility.Collapsed;
                AppointmentTabVisibility = Visibility.Collapsed;

                // Ẩn tất cả buttons
                AddButtonVisibility = Visibility.Collapsed;
                EditButtonVisibility = Visibility.Collapsed;
                DeleteButtonVisibility = Visibility.Collapsed;
                NewRoleButtonVisibility = Visibility.Collapsed;
            }
            else if (!defaultTabSet)
            {
                SelectedTabIndex = 0;
                CurrentViewModel = _serviceProvider.GetRequiredService<StaffViewModel>();
            }

            // Chỉ Admin mới có quyền tạo role mới và xem logs
            NewRoleButtonVisibility = (role?.RoleId == 1) ? Visibility.Visible : Visibility.Collapsed;
            ViewLogsButtonVisibility = (role?.RoleId == 1) ? Visibility.Visible : Visibility.Collapsed;

            // notify UI
            OnPropertyChanged(nameof(StaffTabVisibility));
            OnPropertyChanged(nameof(DepartmentTabVisibility));
            OnPropertyChanged(nameof(RoomTabVisibility));
            OnPropertyChanged(nameof(PatientTabVisibility));
            OnPropertyChanged(nameof(AppointmentTabVisibility));
            OnPropertyChanged(nameof(MedicationTabVisibility));
            OnPropertyChanged(nameof(InvoiceTabVisibility));
            OnPropertyChanged(nameof(AddButtonVisibility));
            OnPropertyChanged(nameof(EditButtonVisibility));
            OnPropertyChanged(nameof(DeleteButtonVisibility));
            OnPropertyChanged(nameof(NewRoleButtonVisibility));
            OnPropertyChanged(nameof(ViewLogsButtonVisibility));
        }

        private void CheckPermissions()
        {
            HasPermission = StaffTabVisibility == Visibility.Visible ||
                            DepartmentTabVisibility == Visibility.Visible ||
                            RoomTabVisibility == Visibility.Visible ||
                            PatientTabVisibility == Visibility.Visible ||
                            AppointmentTabVisibility == Visibility.Visible ||
                            MedicationTabVisibility == Visibility.Visible ||
                            InvoiceTabVisibility == Visibility.Visible;
        }

        public void UpdateCurrentUserIfEdited(User editedUser)
        {
            if (CurrentUser == null || editedUser == null)
                return;

            if (editedUser.UserId != CurrentUser.UserId)
                return;

            // Cập nhật thông tin user
            CurrentUser = editedUser;
            OnPropertyChanged(nameof(CurrentUser));

            try
            {
                // Chỉ gọi SetRole nếu Role hợp lệ và có permissions
                if (editedUser.Role != null && editedUser.Role.RolePermissions != null)
                {
                    SetRole(editedUser.Role, editedUser);
                }
            }
            catch (Exception ex)
            {
                // Ghi log nếu cần, tránh crash UI
                System.Diagnostics.Debug.WriteLine($"UpdateCurrentUserIfEdited warning: {ex.Message}");
            }
        }

        private void OpenNewRoleWindow()
        {
            var roleDialog = _serviceProvider.GetRequiredService<RoleDialogWindow>();
            roleDialog.Owner = Application.Current.MainWindow;
            roleDialog.ShowDialog();
        }

        private void ContactAdmin()
        {
            MessageBox.Show("Liên hệ quản trị viên:\n- Email: admin@hms.com\n- SĐT: 0900.000.001",
                "Thông tin liên hệ", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OpenLogWindow()
        {
            if (CurrentUser?.RoleId != 1) // Chỉ admin mới mở
            {
                MessageBox.Show("You do not have permission to view logs.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_logWindow == null || !_logWindow.IsLoaded)
            {
                var logVM = _serviceProvider.GetRequiredService<LogViewModel>();
                _logWindow = new LogWindow(logVM);
                _logWindow.Owner = Application.Current.MainWindow;
                _logWindow.SetCurrentUser(CurrentUser);
                _logWindow.Closed += (s, e) => _logWindow = null;
                _logWindow.Show();
            }
            else
            {
                _logWindow.Focus();
            }
        }

        public async Task Logout()
        {
            if (CurrentUser != null)
            {
                try
                {
                    // Ghi log logout
                    var log = new Log
                    {
                        UserId = CurrentUser.UserId,
                        Action = $"User '{CurrentUser.FullName}' logged out.",
                        Timestamp = DateTime.Now
                    };

                    var logRepository = _serviceProvider.GetRequiredService<ILogRepository>();
                    await logRepository.AddLogAsync(log);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Logout logging failed: {ex.Message}");
                }
            }

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
                    mainVM.SetRole(loginWindow.LoggedInUser.Role, loginWindow.LoggedInUser);

                    mainWindow.Show();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            });

            if (_logWindow != null)
            {
                _logWindow.Close();
                _logWindow = null;
            }
        }
    }
}