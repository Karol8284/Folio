using Folio.CORE.Entities;
using Folio.CORE.Responses;

namespace Folio.CORE.Interfaces.Services
{
    /// <summary>
    /// IUserService - defines business logic operations for user management
    /// Provides methods for user retrieval, updates, and account management with validation and error handling
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves a single user by their unique identifier
        /// </summary>
        /// <param name="id">The user's unique identifier</param>
        /// <returns>Result containing the user if found, otherwise failure message</returns>
        Task<Result<User>> GetUserByIdAsync(Guid id);

        /// <summary>
        /// Retrieves a user by their email address
        /// Email lookup is case-insensitive for flexible searching
        /// </summary>
        /// <param name="email">The user's email address</param>
        /// <returns>Result containing the user if found, otherwise failure message</returns>
        Task<Result<User>> GetUserByEmailAsync(string email);

        /// <summary>
        /// Retrieves all users without pagination
        /// Warning: Use GetUsersPagedAsync for large user bases to avoid performance issues
        /// </summary>
        /// <returns>Result containing list of all users</returns>
        Task<Result<List<User>>> GetAllUsersAsync();

        /// <summary>
        /// Retrieves users with pagination support for improved performance
        /// </summary>
        /// <param name="pageNumber">The page number (must be >= 1)</param>
        /// <param name="pageSize">The number of records per page (must be >= 1)</param>
        /// <returns>Result containing paginated user data with metadata</returns>
        Task<Result<PagedResponse<User>>> GetUsersPagedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Updates an existing user's profile information
        /// Validates user exists before applying changes
        /// </summary>
        /// <param name="user">The user entity with updated values</param>
        /// <returns>Result containing the updated user or failure message if not found</returns>
        Task<Result<User>> UpdateUserAsync(User user);

        /// <summary>
        /// Permanently deletes a user account and all associated data
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete</param>
        /// <returns>Result indicating success or failure of delete operation</returns>
        Task<Result<bool>> DeleteUserAsync(Guid id);

        /// <summary>
        /// Deactivates a user account without deleting data
        /// Soft-delete approach allowing data recovery or account reactivation
        /// </summary>
        /// <param name="id">The unique identifier of the user to deactivate</param>
        /// <returns>Result indicating success or failure of deactivation</returns>
        Task<Result<bool>> DeactivateUserAsync(Guid id);
    }
}
