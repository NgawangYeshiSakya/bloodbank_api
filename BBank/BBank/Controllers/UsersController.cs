using BBank.Model;
using BBank.Repositories.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BBank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json", "application/json-patch+json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            return user;
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
        public async Task<IActionResult> Login(User user)
        {
            var user_data = await _userRepository.Login(user.Username, user.Password);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }
            return Ok(user_data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return BadRequest("User ID mismatch.");
            }

            var userToUpdate = await _userRepository.GetByIdAsync(id);

            if (userToUpdate == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            userToUpdate.Username = updatedUser.Username;
            userToUpdate.Password = updatedUser.Password;
            userToUpdate.FirstName = updatedUser.FirstName;
            userToUpdate.LastName = updatedUser.LastName;
            userToUpdate.APIKey = updatedUser.APIKey;

            var updated = await _userRepository.Update(userToUpdate);

            if (!updated)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateUser(int id, [FromBody] JsonPatchDocument<User> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Patch document is null.");
            }

            var userFromRepo = await _userRepository.GetByIdAsync(id);

            if (userFromRepo == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            patchDoc.ApplyTo(userFromRepo, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);

            if (!TryValidateModel(userFromRepo))
            {
                return ValidationProblem(ModelState);
            }
            var updated = await _userRepository.Update(userFromRepo);

            if (!updated)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            var success = await _userRepository.Delete(id);

            if (!success)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }
    }
}
