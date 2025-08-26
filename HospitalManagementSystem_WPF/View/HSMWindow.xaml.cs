using HospitalManagementSystem.HospitalManagementSystem_WPF;
using HospitalManagementSystem_WPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace HospitalManagementSystem_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (App.ServiceProvider == null)
            {
                throw new InvalidOperationException("ServiceProvider is not initialized.");
            }

            // Lấy MainViewModel từ ServiceProvider
            var mainViewModel = App.ServiceProvider.GetRequiredService<MainViewModel>();
            DataContext = mainViewModel;
        }
    }
}