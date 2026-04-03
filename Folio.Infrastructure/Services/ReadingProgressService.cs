using Folio.CORE.Entities;
using Folio.CORE.Interfaces.Repositories;
using Folio.CORE.Interfaces.Services;
using Folio.CORE.Responses;

namespace Folio.Infrastructure.Services
{
    /// <summary>
    /// ReadingProgressService - implements business logic for tracking user reading progress
    /// Delegates data access to IReadingProgressRepository and manages progress-related operations
    /// </summary>
    public class ReadingProgressService : IReadingProgressService
    {
        private readonly IReadingProgressRepository _readingProgressRepository;

        /// <summary>
        /// Constructor - initializes service with reading progress repository dependency
        /// </summary>
        /// <param name="readingProgressRepository">The reading progress repository for data access</param>
        public ReadingProgressService(IReadingProgressRepository readingProgressRepository)
        {
            _readingProgressRepository = readingProgressRepository;
        }

        /// <summary>
        /// Retrieves a single reading progress record by ID
        /// </summary>
        /// <param name="id">The reading progress unique identifier</param>
        /// <returns>Result containing reading progress if found, otherwise failure message</returns>
        public async Task<Result<ReadingProgress>> GetByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return Result<ReadingProgress>.Failure("Reading progress ID cannot be empty");

                var result = await _readingProgressRepository.GetByIdAsync(id);
                if (!result.IsSuccess || result.Value == null)
                    return Result<ReadingProgress>.Failure(result.Error ?? "Reading progress not found");

                return Result<ReadingProgress>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<ReadingProgress>.Failure($"Error retrieving reading progress: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all reading progress records for a specific user
        /// Shows all chapters the user has started reading
        /// </summary>
        /// <param name="userId">The user's unique identifier</param>
        /// <returns>Result containing list of reading progress records for the user</returns>
        public async Task<Result<List<ReadingProgress>>> GetUserProgressAsync(Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                    return Result<List<ReadingProgress>>.Failure("User ID cannot be empty");

                var result = await _readingProgressRepository.GetByUserIdAsync(userId);
                if (!result.IsSuccess)
                    return Result<List<ReadingProgress>>.Failure(result.Error ?? "Error retrieving user progress");

                return Result<List<ReadingProgress>>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<List<ReadingProgress>>.Failure($"Error retrieving user progress: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves or creates reading progress for a user-chapter combination
        /// Automatically initializes progress on first read
        /// </summary>
        /// <param name="userId">The user's unique identifier</param>
        /// <param name="chapterId">The chapter's unique identifier</param>
        /// <returns>Result containing the reading progress record (existing or newly created)</returns>
        public async Task<Result<ReadingProgress>> GetOrCreateReadingProgressAsync(Guid userId, Guid chapterId)
        {
            try
            {
                if (userId == Guid.Empty)
                    return Result<ReadingProgress>.Failure("User ID cannot be empty");

                if (chapterId == Guid.Empty)
                    return Result<ReadingProgress>.Failure("Chapter ID cannot be empty");

                // Try to get existing progress
                var existingResult = await _readingProgressRepository.GetByUserAndChapterAsync(userId, chapterId);
                if (existingResult.IsSuccess && existingResult.Value != null)
                    return Result<ReadingProgress>.Success(existingResult.Value);

                // Create new progress record if not found
                var newProgress = new ReadingProgress
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    ChapterId = chapterId,
                    WordsRead = 0,
                    LastReadAt = DateTime.UtcNow
                };

                var createResult = await _readingProgressRepository.CreateAsync(newProgress);
                if (!createResult.IsSuccess)
                    return Result<ReadingProgress>.Failure(createResult.Error ?? "Error creating reading progress");

                return Result<ReadingProgress>.Success(createResult.Value);
            }
            catch (Exception ex)
            {
                return Result<ReadingProgress>.Failure($"Error with reading progress: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates reading progress with new words read and timestamp
        /// </summary>
        /// <param name="progress">The reading progress entity with updated values</param>
        /// <returns>Result containing the updated progress or failure message</returns>
        public async Task<Result<ReadingProgress>> UpdateAsync(ReadingProgress progress)
        {
            try
            {
                if (progress == null)
                    return Result<ReadingProgress>.Failure("Reading progress cannot be null");

                if (progress.Id == Guid.Empty)
                    return Result<ReadingProgress>.Failure("Reading progress ID cannot be empty");

                if (progress.UserId == Guid.Empty)
                    return Result<ReadingProgress>.Failure("User ID cannot be empty");

                if (progress.ChapterId == Guid.Empty)
                    return Result<ReadingProgress>.Failure("Chapter ID cannot be empty");

                if (progress.WordsRead < 0)
                    return Result<ReadingProgress>.Failure("Words read cannot be negative");

                // Verify progress exists before updating
                var existingProgress = await _readingProgressRepository.GetByIdAsync(progress.Id);
                if (!existingProgress.IsSuccess || existingProgress.Value == null)
                    return Result<ReadingProgress>.Failure("Reading progress not found");

                // Update timestamp to current UTC time
                progress.LastReadAt = DateTime.UtcNow;

                var result = await _readingProgressRepository.UpdateAsync(progress);
                if (!result.IsSuccess)
                    return Result<ReadingProgress>.Failure(result.Error ?? "Error updating reading progress");

                return Result<ReadingProgress>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<ReadingProgress>.Failure($"Error updating reading progress: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a reading progress record
        /// </summary>
        /// <param name="id">The unique identifier of the reading progress to delete</param>
        /// <returns>Result indicating success or failure of deletion</returns>
        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return Result<bool>.Failure("Reading progress ID cannot be empty");

                var result = await _readingProgressRepository.DeleteAsync(id);
                if (!result.IsSuccess)
                    return Result<bool>.Failure(result.Error ?? "Error deleting reading progress");

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error deleting reading progress: {ex.Message}");
            }
        }

        /// <summary>
        /// Calculates total words read by a user across all chapters
        /// Useful for user statistics and reading milestones
        /// </summary>
        /// <param name="userId">The user's unique identifier</param>
        /// <returns>Result containing the total word count for the user</returns>
        public async Task<Result<int>> GetTotalWordsReadByUserAsync(Guid userId)
        {
            try
            {
                if (userId == Guid.Empty)
                    return Result<int>.Failure("User ID cannot be empty");

                var result = await _readingProgressRepository.GetTotalWordsReadByUserAsync(userId);
                if (!result.IsSuccess)
                    return Result<int>.Failure(result.Error ?? "Error calculating total words");

                return Result<int>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Error calculating total words: {ex.Message}");
            }
        }
    }
}
