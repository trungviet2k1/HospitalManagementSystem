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

        public User GetUserByUsername(string username)
        {
            return _userDAO.GetUserByUsername(username);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userDAO.GetAllUsers();
        }

        public void AddUser(User user)
        {
            _userDAO.AddUser(user);
        }

        public void UpdateUser(User user)
        {
            _userDAO.UpdateUser(user);
        }

        public void DeleteUser(int userId)
        {
            _userDAO.DeleteUser(userId);
        }
    }
}