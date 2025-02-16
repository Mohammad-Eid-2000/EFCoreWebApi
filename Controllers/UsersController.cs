using EFCoreWebApi.Models;
using EFCoreWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;

        public UsersController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await _userRepository.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            await _userRepository.AddAsync(user);
            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            await _userRepository.UpdateAsync(user);
            return NoContent();
        }
        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


            // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool isDeleted = await _userRepository.DeleteAsync(id);

            if (isDeleted)
            {
                return Ok(new { success = true, message = "User deleted successfully." });
            }
            else
            {
                return NotFound(new { success = false, message = "User not found or could not be deleted." });
            }
        }
    }
    }