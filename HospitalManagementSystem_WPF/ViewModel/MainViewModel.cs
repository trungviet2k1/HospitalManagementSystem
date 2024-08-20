using HospitalManagementSystem.HospitalManagementSystem_WPF;
using HospitalManagementSystem_WPF.View;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            LoginWindow loginWindow = new ();
            {
                ServiceLocator.ServiceProvider.GetRequiredService<LoginWindow>();
                loginWindow.ShowDialog();
            }
        }
    }
}