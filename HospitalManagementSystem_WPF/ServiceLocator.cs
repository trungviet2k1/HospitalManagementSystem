using Microsoft.Extensions.DependencyInjection;

namespace HospitalManagementSystem.HospitalManagementSystem_WPF
{
    public static class ServiceLocator
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        public static void ConfigureServices(IServiceCollection services)
        {
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}