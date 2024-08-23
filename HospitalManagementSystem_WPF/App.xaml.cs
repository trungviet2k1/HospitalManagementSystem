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
        public static IServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Cấu hình ServiceProvider
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            // Khởi tạo LoginWindow và hiển thị
            var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
            loginWindow.ShowDialog();

            // Khởi tạo MainWindow và hiển thị sau khi đăng nhập thành công
            var mainWindow = new MainWindow
            {
                DataContext = ServiceProvider.GetRequiredService<MainViewModel>()
            };
            mainWindow.Show();

            base.OnStartup(e);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Đăng ký ViewModel
            services.AddSingleton<MainViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<StaffViewModel>();
            services.AddTransient<DoctorViewModel>();
            services.AddTransient<NurseViewModel>();
            services.AddTransient<ReceptionistViewModel>();
            services.AddTransient<DepartmentViewModel>();
            services.AddTransient<RoomViewModel>();
            services.AddTransient<PatientViewModel>();
            services.AddTransient<MedicationViewModel>();
            services.AddTransient<InvoiceViewModel>();

            // Đăng ký View
            services.AddSingleton<MainWindow>();
            services.AddTransient<LoginWindow>();

            // Đăng ký các DAO
            services.AddTransient<AppointmentDAO>();
            services.AddTransient<DepartmentDAO>();
            services.AddTransient<PatientDAO>();
            services.AddTransient<RoomDAO>();
            services.AddTransient<UserDAO>();

            // Đăng ký các Repository
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Đăng ký DbContext
            services.AddDbContext<HospitalManagementDbContext>(options =>
                options.UseSqlServer("Server=(local); uid=sa; pwd=123; database=HospitalManagementDB; TrustServerCertificate=True;"));
        }
    }
}