using BusinessObject.Models;
using DataAccess.DAO;
using DataAccess.Repositories.IRepository;

namespace DataAccess.Repositories.RepositoryImp
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDAO _userDAO;

        public UserRepository(UserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userDAO.GetAllUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userDAO.GetUserByIdAsync(userId);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userDAO.GetUserByUsernameAsync(username);
        }

        public async Task AddUserAsync(User user)
        {
            await _userDAO.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userDAO.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            await _userDAO.DeleteUserAsync(userId);
        }
    }
}