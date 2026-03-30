using Folio.CORE.Entities;
using Folio.CORE.Responses;

namespace Folio.CORE.Interfaces
{
    public interface IChapterService
    {
        Task<Result<Chapter>> GetChapterByIdAsync(Guid id);
        Task<Result<List<Chapter>>> GetChaptersByBookIdAsync(Guid bookId);
        Task<Result<PagedResponse<Chapter>>> GetChaptersPagedByBookIdAsync(Guid bookId, int pageNumber, int pageSize);
        Task<Result<Chapter>> CreateChapterAsync(Chapter chapter);
        Task<Result<Chapter>> UpdateChapterAsync(Chapter chapter);
        Task<Result<bool>> DeleteChapterAsync(Guid id);
    }
}
