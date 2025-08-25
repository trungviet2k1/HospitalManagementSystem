using BusinessObject.Models;
using DataAccess.Repositories.IRepository;
using System.Collections.ObjectModel;
using System.Windows;
using HospitalManagementSystem_WPF.View;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class StaffViewModel : BaseViewModel, ICrudOperations
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private ObservableCollection<User>? _staffList;
        private ObservableCollection<Role>? _roles;
        private ObservableCollection<Department>? _departments;
        private User? _selectedStaff;
        private Role? _role;

        public User? CurrentUser { get; set; }

        public ObservableCollection<User>? StaffList
        {
            get => _staffList;
            set => SetProperty(ref _staffList, value);
        }

        public ObservableCollection<Role>? Roles
        {
            get => _roles;
            set => SetProperty(ref _roles, value);
        }

        public ObservableCollection<Department>? Departments
        {
            get => _departments;
            set => SetProperty(ref _departments, value);
        }

        public User? SelectedStaff
        {
            get => _selectedStaff;
            set => SetProperty(ref _selectedStaff, value);
        }

        public Role? Role
        {
            get => _role;
            set => SetProperty(ref _role, value);
        }

        // Constructor nhận CurrentUser từ MainViewModel (hoặc gán sau)
        public StaffViewModel(IUserRepository userRepository, IRoleRepository roleRepository, IDepartmentRepository departmentRepository, User? currentUser = null)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _departmentRepository = departmentRepository;
            CurrentUser = currentUser;

            LoadStaffsAsync();
            _ = LoadRolesAsync();
        }

        public async void LoadStaffsAsync()
        {
            var staffs = await _userRepository.GetAllUsersWithDepartmentAsync();
            StaffList = [.. staffs];
        }

        public async Task LoadRolesAsync()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            Roles = [.. roles];
        }

        public async Task LoadDepartmentsAsync()
        {
            var depts = await _departmentRepository.GetAllDepartmentsAsync();
            Departments = [.. depts];
        }

        public async void Add()
        {
            // 1. Load Roles & Departments trước khi mở dialog
            await LoadRolesAsync();
            await LoadDepartmentsAsync();

            // 2. Tạo User mới với Role mặc định
            var user = new User
            {
                RoleId = Roles?.FirstOrDefault()?.RoleId ?? 0
            };

            // 3. Tạo Staff mặc định liên kết Department
            var staff = new Staff
            {
                DepartmentId = Departments?.FirstOrDefault()?.DepartmentId ?? 0,
                Department = Departments?.FirstOrDefault() ?? null!,
                User = user,
                UserId = user.UserId
            };
            user.Staff.Add(staff);

            // 4. Mở dialog
            var dialog = new StaffDialogWindow(
                user,
                Roles ?? [],
                Departments ?? []
            )
            {
                Owner = Application.Current.MainWindow
            };

            if (dialog.ShowDialog() == true)
            {
                // 5. Thêm User vào database và cập nhật StaffList
                await _userRepository.AddUserAsync(user);
                StaffList?.Add(user);
                SelectedStaff = user;
            }
        }

        public async void Edit()
        {
            if (SelectedStaff == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên để sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 1. Load Roles & Departments trước khi mở dialog
            await LoadRolesAsync();
            await LoadDepartmentsAsync();

            // 2. Cập nhật RoleId nếu Role != null
            if (SelectedStaff.Role != null)
                SelectedStaff.RoleId = SelectedStaff.Role.RoleId;

            // 3. Mở dialog
            var dialog = new StaffDialogWindow(
                SelectedStaff,
                Roles ?? [],
                Departments ?? []
            )
            {
                Owner = Application.Current.MainWindow
            };

            if (dialog.ShowDialog() == true)
            {
                // 4. Cập nhật User trong database
                await _userRepository.UpdateUserAsync(SelectedStaff);

                // 5. Cập nhật ObservableCollection
                var index = StaffList?.IndexOf(SelectedStaff) ?? -1;
                if (index >= 0 && StaffList != null)
                    StaffList[index] = SelectedStaff;

                // 6. Cập nhật MainViewModel nếu CurrentUser bị sửa
                if (Application.Current.MainWindow?.DataContext is MainViewModel mainVM)
                {
                    mainVM.UpdateCurrentUserIfEdited(SelectedStaff);

                    if (mainVM.CurrentViewModel is StaffViewModel staffVM)
                    {
                        var idx = staffVM.StaffList?.IndexOf(SelectedStaff) ?? -1;
                        if (idx >= 0)
                            staffVM.StaffList[idx] = SelectedStaff;
                    }
                }
            }
        }

        public async void Delete()
        {
            if (SelectedStaff == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên {SelectedStaff.FullName}?",
                                         "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                await _userRepository.DeleteUserAsync(SelectedStaff.UserId);
                StaffList?.Remove(SelectedStaff);
                SelectedStaff = null;
            }
        }
    }
}