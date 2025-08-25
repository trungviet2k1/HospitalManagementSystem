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

            if (user != null && user.VerifyPassword(password))
            {
                return user; // user có đầy đủ Role, RolePermissions
            }
            return null;
        }
    }
}