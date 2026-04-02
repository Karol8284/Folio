using Folio.CORE.Entities;
using Folio.CORE.Responses;

namespace Folio.CORE.Interfaces.Services
{
    /// <summary>
    /// IReadingProgressService - defines business logic for tracking user reading progress
    /// Manages user reading statistics, chapter progress, and progress history with validation
    /// </summary>
    public interface IReadingProgressService
    {
        /// <summary>
        /// Retrieves a single reading progress record by its unique identifier
        /// </summary>
        /// <param name="id">The reading progress unique identifier</param>
        /// <returns>Result containing the reading progress record if found, otherwise failure message</returns>
        Task<Result<ReadingProgress>> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all reading progress records for a specific user
        /// Shows all chapters the user has started reading
        /// </summary>
        /// <param name="userId">The user's unique identifier</param>
        /// <returns>Result containing list of reading progress records for the user</returns>
        Task<Result<List<ReadingProgress>>> GetUserProgressAsync(Guid userId);

        /// <summary>
        /// Retrieves existing reading progress for a user-chapter combination, or creates a new one if not found
        /// Automatically initializes progress record on first read
        /// </summary>
        /// <param name="userId">The user's unique identifier</param>
        /// <param name="chapterId">The chapter's unique identifier</param>
        /// <returns>Result containing the reading progress record (existing or newly created)</returns>
        Task<Result<ReadingProgress>> GetOrCreateReadingProgressAsync(Guid userId, Guid chapterId);

        /// <summary>
        /// Updates reading progress with new words read count and timestamp
        /// Validates the progress record exists before updating
        /// </summary>
        /// <param name="progress">The reading progress entity with updated values</param>
        /// <returns>Result containing the updated progress record or failure message</returns>
        Task<Result<ReadingProgress>> UpdateAsync(ReadingProgress progress);

        /// <summary>
        /// Deletes a reading progress record
        /// </summary>
        /// <param name="id">The unique identifier of the reading progress to delete</param>
        /// <returns>Result indicating success or failure of delete operation</returns>
        Task<Result<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Calculates total words read by a user across all chapters
        /// Useful for user statistics, achievements, and reading milestones
        /// </summary>
        /// <param name="userId">The user's unique identifier</param>
        /// <returns>Result containing the total word count for the user</returns>
        Task<Result<int>> GetTotalWordsReadByUserAsync(Guid userId);
    }
}
