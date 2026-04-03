using Folio.CORE.Entities;
using Folio.CORE.Responses;

namespace Folio.CORE.Interfaces.Repositories
{
    /// <summary>
    /// IReadingProgressRepository - contract for reading progress data access operations
    /// Manages user reading progress tracking across chapters
    /// </summary>
    public interface IReadingProgressRepository
    {
        /// <summary>Retrieves a single reading progress record by ID</summary>
        Task<Result<ReadingProgress>> GetByIdAsync(Guid id);

        /// <summary>Retrieves all reading progress for a specific user</summary>
        Task<Result<List<ReadingProgress>>> GetByUserIdAsync(Guid userId);

        /// <summary>Retrieves all reading progress for a specific chapter</summary>
        Task<Result<List<ReadingProgress>>> GetByChapterIdAsync(Guid chapterId);

        /// <summary>Retrieves reading progress for a specific user-chapter combination</summary>
        Task<Result<ReadingProgress>> GetByUserAndChapterAsync(Guid userId, Guid chapterId);

        /// <summary>Retrieves all reading progress records (warning: use pagination for large datasets)</summary>
        Task<Result<List<ReadingProgress>>> GetAllAsync();

        /// <summary>Creates a new reading progress record</summary>
        Task<Result<ReadingProgress>> CreateAsync(ReadingProgress progress);

        /// <summary>Updates an existing reading progress record</summary>
        Task<Result<ReadingProgress>> UpdateAsync(ReadingProgress progress);

        /// <summary>Deletes a reading progress record</summary>
        Task<Result<bool>> DeleteAsync(Guid id);

        /// <summary>Checks if a reading progress record exists</summary>
        Task<Result<bool>> ExistsAsync(Guid id);

        /// <summary>Calculates total words read by a user across all chapters</summary>
        Task<Result<int>> GetTotalWordsReadByUserAsync(Guid userId);
    }
}
