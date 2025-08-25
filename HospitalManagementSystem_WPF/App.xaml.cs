using DataAccess.DAO;
using DataAccess.Repositories.IRepository;
using DataAccess.Repositories.RepositoryImp;
using HospitalManagementSystem_WPF.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using DataAccess.DBContext;
using HospitalManagementSystem_WPF.View;
using HospitalManagementSystem_WPF;
using Microsoft.Extensions.Configuration;
using System.IO;
using DataAccess.Repositories;

namespace HospitalManagementSystem.HospitalManagementSystem_WPF
{
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }
        public static IConfiguration? Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            // 1) Load config
            var builder = new ConfigurationManager();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder;

            // 2) Build DI
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            // 3) RẤT QUAN TRỌNG: chặn WPF auto-shutdown khi login window đóng
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // 4) Show login dialog
            var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
            var loginResult = loginWindow.ShowDialog();

            if (loginResult == true && loginWindow.LoggedInUser != null)
            {
                // 5) Tạo và mở MainWindow
                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();

                // Set role vào MainViewModel (đã được set làm DataContext trong MainWindow)
                if (mainWindow.DataContext is MainViewModel mainVM)
                {
                    mainVM.SetRole(loginWindow.LoggedInUser.Role);
                }

                // Đặt MainWindow CHÍNH THỨC rồi mới Show
                MainWindow = mainWindow;
                MainWindow.Show();

                // 6) Cho phép WPF tự tắt khi MainWindow đóng
                ShutdownMode = ShutdownMode.OnMainWindowClose;
            }
            else
            {
                // Login fail/cancel
                Shutdown();
            }

            base.OnStartup(e);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // ViewModels
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

            // Views
            services.AddSingleton<MainWindow>();
            services.AddTransient<LoginWindow>();
            services.AddTransient<StaffDialogWindow>();

            // DAOs
            services.AddTransient<AppointmentDAO>();
            services.AddTransient<DepartmentDAO>();
            services.AddTransient<PatientDAO>();
            services.AddTransient<RoomDAO>();
            services.AddTransient<UserDAO>();
            services.AddTransient<RoleDAO>();

            // Repos
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            // DbContext
            var connectionString = Configuration!.GetConnectionString("DBContext");
            services.AddDbContext<HospitalManagementDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}