using BusinessObject.Models;
using DataAccess.Repositories.IRepository;
using HospitalManagementSystem_WPF.ViewModel;
using System.Windows;

namespace HospitalManagementSystem_WPF.View
{
    public partial class PermissionDialogWindow : Window
    {
        public PermissionViewModel ViewModel { get; private set; }

        public PermissionDialogWindow(Role role, IRoleRepository roleRepository)
        {
            InitializeComponent();
            ViewModel = new PermissionViewModel(role, roleRepository);
            DataContext = ViewModel;
        }
    }
}