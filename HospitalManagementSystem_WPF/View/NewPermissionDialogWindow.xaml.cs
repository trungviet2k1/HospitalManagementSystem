using DataAccess.Repositories.IRepository;
using HospitalManagementSystem_WPF.ViewModel;
using System.Windows;

namespace HospitalManagementSystem_WPF.View
{
    public partial class NewPermissionWindow : Window
    {
        public NewPermissionWindow(IPermissionRepository permissionRepository)
        {
            InitializeComponent();
            DataContext = new NewPermissionViewModel(permissionRepository);
        }
    }
}