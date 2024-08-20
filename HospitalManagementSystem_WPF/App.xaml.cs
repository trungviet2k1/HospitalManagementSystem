using DataAccess.DAO;
using DataAccess.Repositories.IRepository;
using DataAccess.Repositories.RepositoryImp;
using HospitalManagementSystem_WPF.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using DataAccess.DBContext;
using HospitalManagementSystem_WPF.View;

namespace HospitalManagementSystem.HospitalManagementSystem_WPF
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
            services.AddTransient<UserDAO>();

            // Đăng ký các Repository
            services.AddTransient<IAppointmentRepository, AppointmentRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<IDoctorRepository, DoctorRepository>();
            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            // Đăng ký ViewModel
            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainViewModel>();

            // Đăng ký View
            services.AddTransient<LoginWindow>();

            // Đăng ký DbContext
            services.AddDbContext<HospitalManagementSystemContext>(options =>
                options.UseSqlServer("DBContext"));

            // Cấu hình ServiceProvider
            ServiceLocator.ConfigureServices(services);

            // Khởi tạo LoginWindow và hiển thị
            var loginWindow = ServiceLocator.ServiceProvider.GetRequiredService<LoginWindow>();
            loginWindow.ShowDialog();

            // Nếu người dùng đăng nhập thành công, khởi tạo MainWindow và hiển thị
            if (loginWindow.DialogResult == true)
            {
                var mainWindow = ServiceLocator.ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
    }
}