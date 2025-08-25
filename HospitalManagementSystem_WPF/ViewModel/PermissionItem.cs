using BusinessObject.Models;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class PermissionItem : BaseViewModel
    {
        public required Permission Permission { get; set; }

        private bool _isAssigned;
        public bool IsAssigned
        {
            get => _isAssigned;
            set => SetProperty(ref _isAssigned, value);
        }

        public string PermissionName => Permission.PermissionName;
    }
}