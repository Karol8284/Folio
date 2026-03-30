using Folio.CORE.Entities;
using Folio.CORE.Responses;

namespace Folio.CORE.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<Result<User>> GetByIdAsync(Guid id);
        Task<Result<User>> GetByEmailAsync(string email);
        Task<Result<List<User>>> GetAllAsync();
        Task<Result<PagedResponse<User>>> GetPagedAsync(int pageNumber, int pageSize);
        Task<Result<User>> CreateAsync(User user);
        Task<Result<User>> UpdateAsync(User user);
        Task<Result<bool>> DeleteAsync(Guid id);
        Task<Result<bool>> ExistsAsync(Guid id);
        Task<Result<bool>> EmailExistsAsync(string email);
    }
}
