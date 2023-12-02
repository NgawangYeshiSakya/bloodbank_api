using BBank.Data;
using BBank.Model;
using BBank.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BBank.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ValidateUserKeyAsync(string userKey)
        {
            return await _context.Users.AnyAsync(u => u.APIKey == userKey);
        }

        public async Task<User> Register(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
            return user;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> Update(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser == null)
            {
                return false; // User not found
            }
            existingUser.Username = user.Username;
            existingUser.Password = user.Password;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.APIKey = user.APIKey;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return false; // User not found
            }
            _context.Users.Remove(user);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
