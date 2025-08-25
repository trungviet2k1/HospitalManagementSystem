using HospitalManagementSystem_WPF.ViewModel;
using System.Windows;

namespace HospitalManagementSystem_WPF.View
{
    public partial class RoleDialogWindow : Window
    {
        public RoleViewModel ViewModel { get; private set; }

        public RoleDialogWindow(RoleViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;
        }
    }
}