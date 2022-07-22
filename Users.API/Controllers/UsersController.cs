using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Users.API.Entities;
using Users.API.Repositories;
using Users.API.SwaggerExamples;

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


        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>Returns all registered users.</returns>
        [HttpGet(Name = "GetUsers")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _repository.GetUsers();
            return Ok(users);
        }


        /// <summary>
        /// Get all active users.
        /// </summary>
        /// <returns>Returns all registered users that are actives.</returns>
        [HttpGet("/api/v1/GetActiveUsers", Name = "GetActiveUsers")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetActiveUsers()
        {
            var users = await _repository.GetActiveUsers();
            return Ok(users);
        }


        /// <summary>
        /// Get user by Id.
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     GET /api/v1/Users/602d2149e773f2a3990b47f1
        ///     
        /// </remarks>
        /// <param name="id">User Id</param>
        /// <returns>Returns the user.</returns>
        [HttpGet("{id}", Name = "GetUser")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            var user = await _repository.GetUser(id);

            if (user is null) return NotFound();

            return Ok(user);
        }



        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// /// <remarks>
        /// Example:
        /// 
        ///     POST /api/v1/Users/CreateUser
        ///     {
        ///         "name": "Albert Einstein",
        ///         "birthDate": "1879-03-14"
        ///     }
        ///     
        ///     * birthDate must be in the format yyyy-MM-dd.
        /// </remarks>
        /// <param name="user">User object</param>
        /// <returns>Returns the user that was created.</returns>
        [HttpPost("CreateUser", Name = "CreateUser")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            if (user is null) return BadRequest("Invalid user");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            string pattern = "^[0-9]{4}-[0-1][0-9]-[0-3][0-9]$"; //yyyy-MM-dd
            if (!Regex.IsMatch(user.BirthDate.ToString(), pattern)) return BadRequest("Date is in the wrong format. Please use the format: yyyy-MM-dd");

            await _repository.CreateUser(user);

            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }



        /// <summary>
        /// Updates user state.
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     PUT /api/v1/Users/UpdateUserState/602d2149e773f2a3990b47f1
        ///     
        ///     * If the user Active state is true, it will change to false, and vice versa
        ///     
        /// </remarks>
        /// <param name="id">User Id</param>
        /// <returns>Returns the action result.</returns>
        [HttpPut("UpdateUserState/{id}", Name = "UpdateUserState")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserState(string id)
        {
            bool wasUpdated = await _repository.UpdateUserState(id);

            if (!wasUpdated) return NotFound();

            var user = await _repository.GetUser(id);

            return Ok(user);
        }



        /// <summary>
        /// Deletes a user by Id.
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     DELETE /api/v1/Users/602d2149e773f2a3990b47f1
        ///     
        /// </remarks>
        /// <param name="id">User Id</param>
        /// <returns>Returns the action result.</returns>
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
