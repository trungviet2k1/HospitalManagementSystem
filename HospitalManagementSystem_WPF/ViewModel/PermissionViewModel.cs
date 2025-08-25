using System.Collections.ObjectModel;
using BusinessObject.Models;
using DataAccess.Repositories.IRepository;
using HospitalManagementSystem_WPF.View;
using System.Windows.Input;
using System.Windows;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class PermissionViewModel : BaseViewModel
    {
        private readonly IRoleRepository _roleRepository;
        private readonly Role _role;

        public ObservableCollection<PermissionItem> Permissions { get; set; }

        public ICommand SaveCommand { get; }

        public PermissionViewModel(Role role, IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
            _role = role;

            // Khởi tạo RolePermissions nếu null
            _role.RolePermissions ??= [];

            Permissions = [];

            SaveCommand = new RelayCommand(async () => await SaveAsync());

            LoadPermissionsAsync();
        }

        private async void LoadPermissionsAsync()
        {
            var allPermissions = await _roleRepository.GetAllPermissionsAsync();

            Permissions.Clear();

            foreach (var perm in allPermissions)
            {
                var isAssigned = _role.RolePermissions.Any(rp => rp.PermissionId == perm.PermissionId);
                Permissions.Add(new PermissionItem
                {
                    Permission = perm,
                    IsAssigned = isAssigned
                });
            }
        }

        private async Task SaveAsync()
        {
            _role.RolePermissions ??= [];

            foreach (var item in Permissions)
            {
                var existing = _role.RolePermissions.FirstOrDefault(rp => rp.PermissionId == item.Permission.PermissionId);
                if (item.IsAssigned && existing == null)
                {
                    // Thêm mới
                    _role.RolePermissions.Add(new RolePermission
                    {
                        RoleId = _role.RoleId,
                        PermissionId = item.Permission.PermissionId
                    });
                }
                else if (!item.IsAssigned && existing != null)
                {
                    // Xóa permission
                    _role.RolePermissions.Remove(existing);
                }
            }

            await _roleRepository.UpdateRoleAsync(_role);

            MessageBox.Show("Permissions updated successfully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            // Đóng cửa sổ PermissionDialogWindow hiện tại
            var window = Application.Current?.Windows
                .OfType<PermissionDialogWindow>()
                .FirstOrDefault();
            window?.Close();
        }
    }
}