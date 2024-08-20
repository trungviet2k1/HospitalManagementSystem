using BusinessObject.Models;

namespace DataAccess.Repositories.IRepository
{
    public interface IUserRepository
    {
        User GetUserByUsername(string username);
        IEnumerable<User> GetAllUsers();
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int userId);
    }
}