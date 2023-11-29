using BBank.Model;
using BBank.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BBank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            user.APIKey = Guid.NewGuid().ToString();
            var registeredUser = await _userRepository.Register(user);
            if (registeredUser == null)
            {
                return BadRequest("User could not be registered.");
            }
            return Ok(registeredUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userRepository.Login(username, password);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }
            return Ok(user);
        }
    }
}
