using Folio.CORE.Entities;
using Folio.CORE.Interfaces.Repositories;
using Folio.CORE.Interfaces.Services;
using Folio.CORE.Responses;

namespace Folio.Infrastructure.Services
{
    /// <summary>
    /// UserService - implements business logic for user management
    /// Delegates data access to IUserRepository and maps entities to DTOs for API responses
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Constructor - initializes service with user repository dependency
        /// </summary>
        /// <param name="userRepository">The user repository for data access</param>
        public UserService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Retrieves a single user by ID and maps to UserDto
        /// </summary>
        /// <param name="id">The user's unique identifier</param>
        /// <returns>Result containing user DTO if found, otherwise failure message</returns>
        public async Task<Result<User>> GetUserByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return Result<User>.Failure("User ID cannot be empty");

                var result = await _userRepository.GetByIdAsync(id);
                if (!result.IsSuccess || result.Value == null)
                    return Result<User>.Failure(result.Error ?? "User not found");

                return Result<User>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Error retrieving user: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a user by email address (case-insensitive)
        /// </summary>
        /// <param name="email">The user's email address</param>
        /// <returns>Result containing user DTO if found, otherwise failure message</returns>
        public async Task<Result<User>> GetUserByEmailAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return Result<User>.Failure("Email cannot be empty");

                var result = await _userRepository.GetByEmailAsync(email);
                if (!result.IsSuccess || result.Value == null)
                    return Result<User>.Failure(result.Error ?? "User not found");

                return Result<User>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Error retrieving user: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all users (warning: use paged version for large datasets)
        /// </summary>
        /// <returns>Result containing list of all users</returns>
        public async Task<Result<List<User>>> GetAllUsersAsync()
        {
            try
            {
                var result = await _userRepository.GetAllAsync();
                if (!result.IsSuccess)
                    return Result<List<User>>.Failure(result.Error ?? "Error retrieving users");

                return Result<List<User>>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<List<User>>.Failure($"Error retrieving users: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves users with pagination for improved performance
        /// </summary>
        /// <param name="pageNumber">The page number (must be >= 1)</param>
        /// <param name="pageSize">The number of records per page (must be >= 1)</param>
        /// <returns>Result containing paginated user data with metadata</returns>
        public async Task<Result<PagedResponse<User>>> GetUsersPagedAsync(int pageNumber, int pageSize)
        {
            try
            {
                if (pageNumber < 1)
                    return Result<PagedResponse<User>>.Failure("Page number must be >= 1");
                if (pageSize < 1)
                    return Result<PagedResponse<User>>.Failure("Page size must be >= 1");

                var result = await _userRepository.GetPagedAsync(pageNumber, pageSize);
                if (!result.IsSuccess)
                    return Result<PagedResponse<User>>.Failure(result.Error ?? "Error retrieving users");

                return Result<PagedResponse<User>>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<PagedResponse<User>>.Failure($"Error retrieving paged users: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing user with new information
        /// Validates user exists before update
        /// </summary>
        /// <param name="user">The user entity with updated values</param>
        /// <returns>Result containing updated user or failure message</returns>
        public async Task<Result<User>> UpdateUserAsync(User user)
        {
            try
            {
                if (user == null)
                    return Result<User>.Failure("User cannot be null");

                if (user.Id == Guid.Empty)
                    return Result<User>.Failure("User ID cannot be empty");

                // Verify user exists before updating
                var existingUser = await _userRepository.GetByIdAsync(user.Id);
                if (!existingUser.IsSuccess || existingUser.Value == null)
                    return Result<User>.Failure("User not found");

                // Validate email is not taken by another user
                if (!existingUser.Value.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var emailExists = await _userRepository.EmailExistsAsync(user.Email);
                    if (emailExists.IsSuccess && emailExists.Value)
                        return Result<User>.Failure($"Email {user.Email} is already in use");
                }

                var result = await _userRepository.UpdateAsync(user);
                if (!result.IsSuccess)
                    return Result<User>.Failure(result.Error ?? "Error updating user");

                return Result<User>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Error updating user: {ex.Message}");
            }
        }

        /// <summary>
        /// Deactivates a user account (soft delete - preserves data)
        /// </summary>
        /// <param name="id">The user's unique identifier</param>
        /// <returns>Result indicating success or failure of deactivation</returns>
        public async Task<Result<bool>> DeactivateUserAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return Result<bool>.Failure("User ID cannot be empty");

                // Retrieve user
                var userResult = await _userRepository.GetByIdAsync(id);
                if (!userResult.IsSuccess || userResult.Value == null)
                    return Result<bool>.Failure("User not found");

                var user = userResult.Value;

                // Already deactivated
                if (!user.IsActive)
                    return Result<bool>.Success(true);

                // Set IsActive to false
                user.IsActive = false;

                // Update user
                var updateResult = await _userRepository.UpdateAsync(user);
                if (!updateResult.IsSuccess)
                    return Result<bool>.Failure(updateResult.Error ?? "Error deactivating user");

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error deactivating user: {ex.Message}");
            }
        }

        /// <summary>
        /// Permanently deletes a user and all associated data (cascade delete)
        /// </summary>
        /// <param name="id">The user's unique identifier</param>
        /// <returns>Result indicating success or failure of deletion</returns>
        public async Task<Result<bool>> DeleteUserAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return Result<bool>.Failure("User ID cannot be empty");

                var result = await _userRepository.DeleteAsync(id);
                if (!result.IsSuccess)
                    return Result<bool>.Failure(result.Error ?? "Error deleting user");

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error deleting user: {ex.Message}");
            }
        }
    }
}
