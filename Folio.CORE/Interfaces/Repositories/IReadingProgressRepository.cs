using Folio.CORE.Entities;
using Folio.CORE.Responses;

namespace Folio.CORE.Interfaces.Repositories
{
    public interface IReadingProgressRepository
    {
        Task<Result<ReadingProgress>> GetByIdAsync(Guid id);
        Task<Result<List<ReadingProgress>>> GetByUserIdAsync(Guid userId);
        Task<Result<List<ReadingProgress>>> GetByChapterIdAsync(Guid chapterId);
        Task<Result<ReadingProgress>> GetByUserAndChapterAsync(Guid userId, Guid chapterId);
        Task<Result<List<ReadingProgress>>> GetAllAsync();
        Task<Result<ReadingProgress>> CreateAsync(ReadingProgress progress);
        Task<Result<ReadingProgress>> UpdateAsync(ReadingProgress progress);
        Task<Result<bool>> DeleteAsync(Guid id);
        Task<Result<bool>> ExistsAsync(Guid id);
    }
}
