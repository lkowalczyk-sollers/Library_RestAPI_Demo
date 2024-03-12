using AutoMapper;
using Library_demo.Dtos;
using Library_demo.Interfaces;
using Library_demo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library_demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserRepository userRepository, IMapper mapper) : Controller
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(404)]
        public IActionResult GetUsers()
        {
            var users = mapper.Map<List<UserDto>>(userRepository.GetUsers());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(users);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        public IActionResult GetUser(int userId)
        {

            if(!userRepository.UserExists(userId))
            {
                return NotFound();
            }
            var user = mapper.Map<UserDto>(userRepository.GetUser(userId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] UserDto userObject)
        {

            if(userObject == null)
            {
                return BadRequest();
            }

            if(userRepository.UserExists(userObject.Id))
            {
                ModelState.AddModelError("DuplicateError", "User with that id already exists");
                return StatusCode(422, ModelState);
            }

            var mappedUser = mapper.Map<User>(userObject);

            mappedUser.SecurityCode = "112233445566";

            if (!userRepository.CreateUser(mappedUser))
            {
                ModelState.AddModelError("SavingError", "User could not be saved");
                return StatusCode(500, ModelState);
            }

            return Ok("User added successfully");
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateUser(int userId, [FromBody] UserDto userObject)
        {
            if(userObject == null)
            {
                return BadRequest(ModelState);
            }

            if(userObject.Id != userId) 
            {
                return BadRequest(ModelState);
            }

            if (!userRepository.UserExists(userId)) 
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var mappedUser = mapper.Map<User>(userObject);
            if(!userRepository.UpdateUser(mappedUser))
            {
                ModelState.AddModelError("SavingError", "User could not be saved");
                return StatusCode(500, ModelState);
            }

            return Ok("User updated successfully");
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int userId) 
        {

            if (!userRepository.UserExists(userId))
            {
                return NotFound();
            }
            var user = userRepository.GetUser(userId);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!userRepository.DeleteUser(user))
            {
                ModelState.AddModelError("SavingError", "User could not be deleted");
                return StatusCode(500, ModelState);
            }

            return Ok("User deleted successfully");
        }
    }
}
