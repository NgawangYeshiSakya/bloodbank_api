using BBank.Model;

namespace BBank.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<bool> ValidateUserKeyAsync(string userKey);
        Task<User> Register(User user);
        Task<User> Login(string username, string password);
    }
}
