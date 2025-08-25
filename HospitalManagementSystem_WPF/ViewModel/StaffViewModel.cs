using BusinessObject.Models;
using DataAccess.Repositories.IRepository;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using HospitalManagementSystem_WPF.View;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class StaffViewModel : BaseViewModel, ICrudOperations
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private ObservableCollection<User>? _staffList;
        private ObservableCollection<Role>? _roles;
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
        public StaffViewModel(IUserRepository userRepository, IRoleRepository roleRepository, User? currentUser = null)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            CurrentUser = currentUser;

            LoadStaffsAsync();
            LoadRolesAsync();
        }

        public async void LoadStaffsAsync()
        {
            var staffs = await _userRepository.GetAllUsersAsync();
            StaffList = new ObservableCollection<User>(staffs);
        }

        public async void LoadRolesAsync()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            Roles = new ObservableCollection<Role>(roles);
        }

        public async void Add()
        {
            var staff = new User();
            staff.RoleId = Roles?.FirstOrDefault()?.RoleId ?? 0;

            var dialog = new StaffDialogWindow(staff, Roles) { Owner = Application.Current.MainWindow };
            if (dialog.ShowDialog() == true)
            {
                await _userRepository.AddUserAsync(staff);
                StaffList?.Add(staff);
                SelectedStaff = staff;
            }
        }

        public async void Edit()
        {
            if (SelectedStaff == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên để sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedStaff.Role != null)
                SelectedStaff.RoleId = SelectedStaff.Role.RoleId;

            var dialog = new StaffDialogWindow(SelectedStaff, Roles) { Owner = Application.Current.MainWindow };
            if (dialog.ShowDialog() == true)
            {
                await _userRepository.UpdateUserAsync(SelectedStaff);

                // Cập nhật ObservableCollection
                var index = StaffList?.IndexOf(SelectedStaff) ?? -1;
                if (index >= 0 && StaffList != null)
                    StaffList[index] = SelectedStaff;

                // Cập nhật MainViewModel
                if (Application.Current.MainWindow?.DataContext is MainViewModel mainVM)
                {
                    // Update role nếu edit chính CurrentUser
                    mainVM.UpdateCurrentUserIfEdited(SelectedStaff);

                    // Luôn refresh UI StaffList nếu admin edit user khác
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