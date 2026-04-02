using Folio.CORE.Entities;
using Folio.CORE.Responses;

namespace Folio.CORE.Interfaces.Repositories
{
    public interface IChapterRepository
    {
        public Task<Result<Chapter>> GetByIdAsync(Guid id);
        public Task<Result<List<Chapter>>> GetByBookIdAsync(Guid bookId);
        public Task<Result<PagedResponse<Chapter>>> GetPagedByBookIdAsync(Guid bookId, int pageNumber, int pageSize);
        public Task<Result<List<Chapter>>> GetAllAsync();
        public Task<Result<Chapter>> CreateAsync(Chapter chapter);
        public Task<Result<Chapter>> UpdateAsync(Chapter chapter);
        public Task<Result<bool>> DeleteAsync(Guid id);
        public Task<Result<bool>> ExistsAsync(Guid id);
    }
}
