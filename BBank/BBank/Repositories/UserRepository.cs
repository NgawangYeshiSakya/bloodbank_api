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
    }
}
