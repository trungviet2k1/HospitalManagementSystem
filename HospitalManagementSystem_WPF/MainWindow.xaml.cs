using HospitalManagementSystem.WPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace HospitalManagementSystem.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = ServiceLocator.ServiceProvider.GetRequiredService<MainViewModel>();
        }
    }
}