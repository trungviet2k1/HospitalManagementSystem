using BusinessObject.Models;
using DataAccess.Repositories.IRepository;
using HospitalManagementSystem_WPF.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

public class NewPermissionViewModel : BaseViewModel
{
    private readonly IPermissionRepository _permissionRepository;

    private string _permissionName = string.Empty;

    public string PermissionName
    {
        get => _permissionName;
        set => SetProperty(ref _permissionName, value);
    }

    private Permission? _selectedPermission;
    public Permission? SelectedPermission
    {
        get => _selectedPermission;
        set => SetProperty(ref _selectedPermission, value);
    }

    public ObservableCollection<Permission> PermissionList { get; set; } = [];

    public ICommand AddPermissionCommand { get; }
    public ICommand EditPermissionCommand { get; }
    public ICommand DeletePermissionCommand { get; }

    public NewPermissionViewModel(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;

        AddPermissionCommand = new RelayCommand(
            execute: (parameter) => AddPermission(),
            canExecute: (parameter) => true
        );

        EditPermissionCommand = new RelayCommand<Permission>(
            execute: (permission) => EditPermission(permission),
            canExecute: (permission) => permission != null
        );

        DeletePermissionCommand = new RelayCommand<Permission>(
            execute: (permission) => DeletePermission(permission),
            canExecute: (permission) => permission != null
        );

        LoadPermissions();
    }

    private async void LoadPermissions()
    {
        var permissions = await _permissionRepository.GetAllPermissionsAsync();
        PermissionList.Clear();
        foreach (var p in permissions)
            PermissionList.Add(p);
    }

    private async void AddPermission()
    {
        if (string.IsNullOrWhiteSpace(PermissionName))
        {
            MessageBox.Show("Permission Name cannot be empty!");
            return;
        }

        var newPerm = new Permission { PermissionName = PermissionName };
        await _permissionRepository.AddPermissionAsync(newPerm);

        PermissionList.Add(newPerm);
        PermissionName = string.Empty;
    }

    private async void EditPermission(Permission permission)
    {
        if (permission == null) return;

        // ví dụ popup sửa tên
        var input = Microsoft.VisualBasic.Interaction.InputBox("Edit Permission Name:", "Edit Permission", permission.PermissionName);
        if (!string.IsNullOrWhiteSpace(input))
        {
            permission.PermissionName = input;
            await _permissionRepository.UpdatePermissionAsync(permission);
            LoadPermissions();
        }
    }

    private async void DeletePermission(Permission permission)
    {
        if (permission == null) return;

        var result = MessageBox.Show($"Do you want to delete '{permission.PermissionName}'?", "Confirm", MessageBoxButton.YesNo);
        if (result == MessageBoxResult.Yes)
        {
            await _permissionRepository.DeletePermissionAsync(permission.PermissionId);
            PermissionList.Remove(permission);
        }
    }
}