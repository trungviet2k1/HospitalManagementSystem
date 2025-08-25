using BusinessObject.Models;
using DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class UserDAO(HospitalManagementDbContext context)
    {
        private readonly HospitalManagementDbContext _context = context;

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                    .ThenInclude(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersWithDepartmentAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Staff)
                    .ThenInclude(s => s.Department)
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.Include(u => u.Role)
                    .ThenInclude(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                    .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.Include(u => u.Role)
                    .ThenInclude(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                    .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetUserWithRoleAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Role)
                    .ThenInclude(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Username == username); // trả về null nếu không tìm thấy
        }

        public async Task AddUserAsync(User user)
        {
            var role = await _context.Roles.FindAsync(user.RoleId);
            user.Role = role;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            var role = await _context.Roles.FindAsync(user.RoleId);
            user.Role = role; // EF sẽ track đúng instance
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}