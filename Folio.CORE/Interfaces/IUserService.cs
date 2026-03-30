using Folio.CORE.Entities;
using Folio.CORE.Responses;

namespace Folio.CORE.Interfaces
{
    public interface IUserService
    {
        Task<Result<User>> GetUserByIdAsync(Guid id);
        Task<Result<User>> GetUserByEmailAsync(string email);
        Task<Result<List<User>>> GetAllUsersAsync();
        Task<Result<PagedResponse<User>>> GetUsersPagedAsync(int pageNumber, int pageSize);
        Task<Result<User>> UpdateUserAsync(User user);
        Task<Result<bool>> DeleteUserAsync(Guid id);
        Task<Result<bool>> DeactivateUserAsync(Guid id);
    }
}
