using DataAccess.Repositories.IRepository;
using BusinessObject.Models;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IUserRepository _userRepository;

        public LoginViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user != null && user.VerifyPassword(password))
            {
                return true;
            }
            return false;
        }
    }
}