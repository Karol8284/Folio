using Folio.CORE.Entities;
using Folio.CORE.Interfaces.Services;
using Folio.CORE.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Folio.API.Controllers
{
    /// <summary>
    /// UsersController - REST API endpoints for user management
    /// Handles CRUD operations and user-related queries
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor - initializes controller with user service dependency
        /// </summary>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// GET /api/users/{id} - Retrieves a single user by ID
        /// </summary>
        /// <param name="id">The user's unique identifier</param>
        /// <returns>User details if found</returns>
        /// <response code="200">User found and returned</response>
        /// <response code="404">User not found</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<User>>> GetUserById(Guid id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// GET /api/users/by-email/{email} - Retrieves a user by email address
        /// </summary>
        /// <param name="email">The user's email address</param>
        /// <returns>User details if found</returns>
        /// <response code="200">User found and returned</response>
        /// <response code="404">User not found</response>
        [HttpGet("by-email/{email}")]
        public async Task<ActionResult<Result<User>>> GetUserByEmail(string email)
        {
            var result = await _userService.GetUserByEmailAsync(email);
            if (!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// GET /api/users - Retrieves all users (warning: use pagination for large datasets)
        /// </summary>
        /// <returns>List of all users</returns>
        /// <response code="200">Users retrieved successfully</response>
        /// <response code="400">Error retrieving users</response>
        [HttpGet]
        public async Task<ActionResult<Result<List<User>>>> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// GET /api/users/paged - Retrieves users with pagination
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Records per page (default: 10)</param>
        /// <returns>Paginated list of users</returns>
        /// <response code="200">Users retrieved successfully</response>
        /// <response code="400">Invalid pagination parameters</response>
        [HttpGet("paged")]
        public async Task<ActionResult<Result<PagedResponse<User>>>> GetUsersPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _userService.GetUsersPagedAsync(pageNumber, pageSize);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// POST /api/users - Creates a new user
        /// </summary>
        /// <param name="user">The user entity to create</param>
        /// <returns>The created user with generated ID</returns>
        /// <response code="201">User created successfully</response>
        /// <response code="400">Invalid user data</response>
        [HttpPost]
        public async Task<ActionResult<Result<User>>> CreateUser([FromBody] User user)
        {
            var result = await _userService.UpdateUserAsync(user);
            if (!result.IsSuccess)
                return BadRequest(result);
            return CreatedAtAction(nameof(GetUserById), new { id = result.Value?.Id }, result);
        }

        /// <summary>
        /// PUT /api/users/{id} - Updates an existing user
        /// </summary>
        /// <param name="id">The user's unique identifier</param>
        /// <param name="user">The updated user data</param>
        /// <returns>The updated user</returns>
        /// <response code="200">User updated successfully</response>
        /// <response code="400">Invalid user data or ID mismatch</response>
        /// <response code="404">User not found</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<Result<User>>> UpdateUser(Guid id, [FromBody] User user)
        {
            if (id != user.Id)
                return BadRequest(Result<User>.Failure("ID mismatch: URL ID does not match user ID"));

            var result = await _userService.UpdateUserAsync(user);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// DELETE /api/users/{id} - Permanently deletes a user account
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete</param>
        /// <returns>Deletion result</returns>
        /// <response code="200">User deleted successfully</response>
        /// <response code="400">Error deleting user</response>
        /// <response code="404">User not found</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<bool>>> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// POST /api/users/{id}/deactivate - Deactivates a user account (soft delete)
        /// </summary>
        /// <param name="id">The unique identifier of the user to deactivate</param>
        /// <returns>Deactivation result</returns>
        /// <response code="200">User deactivated successfully</response>
        /// <response code="400">Error deactivating user</response>
        /// <response code="404">User not found</response>
        [HttpPost("{id}/deactivate")]
        public async Task<ActionResult<Result<bool>>> DeactivateUser(Guid id)
        {
            var result = await _userService.DeactivateUserAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
