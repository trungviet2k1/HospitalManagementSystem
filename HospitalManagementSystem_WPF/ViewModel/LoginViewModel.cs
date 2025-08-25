using BusinessObject.Models;
using DataAccess.Repositories.IRepository;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class LoginViewModel(IUserRepository userRepository) : BaseViewModel
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetUserWithRoleAsync(username);

            if (user == null) return null; // username không tồn tại
            if (!user.Status) return user;  // inactive
            if (!user.VerifyPassword(password)) return null; // sai password

            return user; // đăng nhập thành công
        }
    }
}