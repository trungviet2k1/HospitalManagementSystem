using HospitalManagementSystem.BusinessObject.Models;
using HospitalManagementSystem.DataAccess.DAO;
using HospitalManagementSystem.DataAccess.Repositories.IRepository;
using HospitalManagementSystem.DataAccess.Repositories.RepositoryImp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace HospitalManagementSystem.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            // Đăng ký các DAO
            services.AddTransient<AppointmentDAO>();
            services.AddTransient<DepartmentDAO>();
            services.AddTransient<DoctorDAO>();
            services.AddTransient<PatientDAO>();

            // Đăng ký các Repository
            services.AddTransient<IAppointmentRepository, AppointmentRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<IDoctorRepository, DoctorRepository>();
            services.AddTransient<IPatientRepository, PatientRepository>();

            // Register DbContext
            services.AddDbContext<HospitalManagementSystemContext>(options =>
                options.UseSqlServer("DBContext"));

            // Cấu hình ServiceProvider
            ServiceLocator.ConfigureServices(services);

            // Khởi tạo MainWindow và hiển thị
            var mainWindow = ServiceLocator.ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}