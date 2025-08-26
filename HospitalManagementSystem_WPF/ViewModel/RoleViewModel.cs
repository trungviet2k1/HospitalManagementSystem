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

            var result = MessageBox.Show($"Do you want to delete role '{SelectedRole.RoleName}'?",
                                         "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;

            await _roleRepository.DeleteRoleAsync(SelectedRole.RoleId);
            RoleList?.Remove(SelectedRole);
            SelectedRole = null;
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