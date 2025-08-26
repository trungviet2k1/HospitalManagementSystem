using BusinessObject.Models;
using DataAccess.Repositories.IRepository;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class LoginViewModel(IUserRepository userRepository, ILogRepository logRepository) : BaseViewModel
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ILogRepository _logRepository = logRepository;

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetUserWithRoleAsync(username);

            if (user == null)
            {
                // Log: username không tồn tại
                var logNotExist = new Log
                {
                    UserId = null,
                    Action = $"Failed login attempt: username '{username}' does not exist.",
                    Timestamp = DateTime.Now
                };
                await _logRepository.AddLogAsync(logNotExist);
                return null;
            }

            if (!user.Status)
            {
                // Log: tài khoản inactive
                var logInactive = new Log
                {
                    UserId = user.UserId,
                    Action = $"Failed login attempt: username '{username}' is inactive.",
                    Timestamp = DateTime.Now
                };
                await _logRepository.AddLogAsync(logInactive);
                return user; // vẫn return user để thông báo inactive
            }

            if (!user.VerifyPassword(password))
            {
                // Log: sai password
                var logWrongPassword = new Log
                {
                    UserId = user.UserId,
                    Action = $"Failed login attempt: wrong password for username '{username}'.",
                    Timestamp = DateTime.Now
                };
                await _logRepository.AddLogAsync(logWrongPassword);
                return null;
            }

            // Log: đăng nhập thành công
            var logSuccess = new Log
            {
                UserId = user.UserId,
                Action = $"User '{username}' logged in successfully.",
                Timestamp = DateTime.Now
            };
            await _logRepository.AddLogAsync(logSuccess);

            return user;
        }
    }
}