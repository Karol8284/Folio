using Folio.CORE.Entities;
using Folio.CORE.Responses;

namespace Folio.CORE.Interfaces.Repositories
{
    public interface IChapterRepository
    {
        Task<Result<Chapter>> GetByIdAsync(Guid id);
        Task<Result<List<Chapter>>> GetByBookIdAsync(Guid bookId);
        Task<Result<PagedResponse<Chapter>>> GetPagedByBookIdAsync(Guid bookId, int pageNumber, int pageSize);
        Task<Result<List<Chapter>>> GetAllAsync();
        Task<Result<Chapter>> CreateAsync(Chapter chapter);
        Task<Result<Chapter>> UpdateAsync(Chapter chapter);
        Task<Result<bool>> DeleteAsync(Guid id);
        Task<Result<bool>> ExistsAsync(Guid id);
    }
}
