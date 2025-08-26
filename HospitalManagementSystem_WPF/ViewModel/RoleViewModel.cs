using BusinessObject.Models;
using DataAccess.Repositories.IRepository;
using HospitalManagementSystem_WPF.View;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class RoleViewModel : BaseViewModel, ICrudOperations
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private ObservableCollection<Role>? _roleList;
        private Role? _selectedRole;

        public ObservableCollection<Role>? RoleList
        {
            get => _roleList;
            set => SetProperty(ref _roleList, value);
        }

        public Role? SelectedRole
        {
            get => _selectedRole;
            set => SetProperty(ref _selectedRole, value);
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ViewPermissionCommand { get; }
        public ICommand NewPermissionCommand { get; }

        public RoleViewModel(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;

            AddCommand = new RelayCommand(execute: (parameter) => Add(), canExecute: (parameter) => true);
            EditCommand = new RelayCommand(execute: (parameter) => Edit(), canExecute: (parameter) => true);
            DeleteCommand = new RelayCommand(execute: (parameter) => Delete(), canExecute: (parameter) => true);
            ViewPermissionCommand = new RelayCommand(execute: (parameter) => ViewPermission(), canExecute: (parameter) => true);
            NewPermissionCommand = new RelayCommand(execute: (parameter) => NewPermission(), canExecute: (parameter) => true);

            LoadRolesAsync();
        }

        public async void LoadRolesAsync()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            RoleList = [.. roles];
        }

        public async void Add()
        {
            var newRole = new Role { RoleName = "New Role" };
            await _roleRepository.AddRoleAsync(newRole);
            RoleList?.Add(newRole);
            SelectedRole = newRole;

            MessageBox.Show("Role added successfully!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public async void Edit()
        {
            if (SelectedRole == null) return;
            await _roleRepository.UpdateRoleAsync(SelectedRole);

            MessageBox.Show("Role updated successfully!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public async void Delete()
        {
            if (SelectedRole == null) return;

            // Check nếu là Admin thì không cho xóa
            if (SelectedRole.RoleId == 1)
            {
                MessageBox.Show("Không thể xóa Role 'Admin'!",
                                "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kiểm tra số lượng user đang dùng Role
            int userCount = await _roleRepository.CountUsersByRoleAsync(SelectedRole.RoleId);
            if (userCount > 0)
            {
                MessageBox.Show($"Không thể xóa Role '{SelectedRole.RoleName}' vì hiện có {userCount} tài khoản đang sử dụng Role này.",
                                "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc muốn xóa Role '{SelectedRole.RoleName}'?",
                                         "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;

            await _roleRepository.DeleteRoleAsync(SelectedRole.RoleId);
            RoleList?.Remove(SelectedRole);
            SelectedRole = null;

            MessageBox.Show("Role đã được xóa thành công!",
                            "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ViewPermission()
        {
            if (SelectedRole == null) return;
            var permissionWindow = new PermissionDialogWindow(SelectedRole, _roleRepository)
            {
                Owner = Application.Current.MainWindow
            };
            permissionWindow.ShowDialog();
        }

        private void NewPermission()
        {
            var permissionWindow = new NewPermissionWindow(_permissionRepository)
            {
                Owner = Application.Current.MainWindow
            };
            permissionWindow.ShowDialog();
        }
    }
}