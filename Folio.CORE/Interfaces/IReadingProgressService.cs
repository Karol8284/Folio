using Folio.CORE.Entities;
using Folio.CORE.Responses;

namespace Folio.CORE.Interfaces
{
    public interface IReadingProgressService
    {
        Task<Result<ReadingProgress>> GetReadingProgressByIdAsync(Guid id);
        Task<Result<List<ReadingProgress>>> GetUserReadingProgressAsync(Guid userId);
        Task<Result<ReadingProgress>> GetOrCreateReadingProgressAsync(Guid userId, Guid chapterId);
        Task<Result<ReadingProgress>> UpdateReadingProgressAsync(ReadingProgress progress);
        Task<Result<bool>> DeleteReadingProgressAsync(Guid id);
        Task<Result<int>> GetTotalWordsReadByUserAsync(Guid userId);
    }
}
