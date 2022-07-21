using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.API.Entities;
using Users.API.Repositories;


namespace Users.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;
        public UsersController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _repository.GetUsers();
            return Ok(users);
        }

        [HttpGet("/api/v1/GetActiveUsers")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetActiveUsers()
        {
            var users = await _repository.GetActiveUsers();
            return Ok(users);
        }

        [HttpGet("{id}", Name = "GetUser")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            var user = await _repository.GetUser(id);

            if (user is null) return NotFound();

            return Ok(user);
        }

        //TODO: Colocar para sempre criar com o Active como true
        //TODO: Tratar a formatação da BirthDate
        [HttpPost]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            if (user is null) return BadRequest("Invalid user");

            await _repository.CreateUser(user);

            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserState(string id)
        {
            bool wasUpdated = await _repository.UpdateUserState(id);

            if (!wasUpdated) return NotFound();

            return Ok("User state updated");
        }


        [HttpDelete("{id}", Name = "DeleteUser")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            bool wasDeleted = await _repository.DeleteUser(id);

            if (!wasDeleted) return NotFound();

            return Ok($"User of id {id} deleted");
        }
    }
}
