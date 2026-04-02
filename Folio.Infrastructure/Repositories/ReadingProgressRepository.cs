using Folio.CORE.Entities;
using Folio.CORE.Interfaces.Repositories;
using Folio.CORE.Responses;
using Folio.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Folio.Infrastructure.Repositories
{
    /// <summary>
    /// ReadingProgressRepository - implements all data access operations for ReadingProgress entity
    /// Tracks user reading progress across chapters with specialized queries for user statistics
    /// </summary>
    public class ReadingProgressRepository : IReadingProgressRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor - initializes repository with database context
        /// </summary>
        /// <param name="context">The database context instance</param>
        public ReadingProgressRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a single reading progress record by its unique identifier
        /// </summary>
        /// <param name="id">The reading progress unique identifier</param>
        /// <returns>Result containing the reading progress if found, otherwise failure message</returns>
        public async Task<Result<ReadingProgress>> GetByIdAsync(Guid id)
        {
            try
            {
                var readingProgress = await _context.ReadingProgresses
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (readingProgress == null)
                    return Result<ReadingProgress>.Failure($"Reading progress with ID {id} not found");

                return Result<ReadingProgress>.Success(readingProgress);
            }
            catch (Exception ex)
            {
                return Result<ReadingProgress>.Failure($"Error when fetching reading progress: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all reading progress records
        /// Warning: Use GetByUserIdAsync for large datasets
        /// </summary>
        /// <returns>Result containing list of all reading progress records</returns>
        public async Task<Result<List<ReadingProgress>>> GetAllAsync()
        {
            try
            {
                var readingProgresses = await _context.ReadingProgresses
                    .AsNoTracking()
                    .OrderBy(x => x.LastReadAt)
                    .ToListAsync();

                return Result<List<ReadingProgress>>.Success(readingProgresses);
            }
            catch (Exception ex)
            {
                return Result<List<ReadingProgress>>.Failure($"Error when fetching all reading progress: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all reading progress records for a specific user
        /// </summary>
        /// <param name="userId">The user's unique identifier</param>
        /// <returns>Result containing list of reading progress records for the user</returns>
        public async Task<Result<List<ReadingProgress>>> GetByUserIdAsync(Guid userId)
        {
            try
            {
                var userProgress = await _context.ReadingProgresses
                    .Where(x => x.UserId == userId)
                    .AsNoTracking()
                    .OrderBy(x => x.LastReadAt)
                    .ToListAsync();

                return Result<List<ReadingProgress>>.Success(userProgress);
            }
            catch (Exception ex)
            {
                return Result<List<ReadingProgress>>.Failure($"Error when fetching user progress: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all reading progress records for a specific chapter
        /// </summary>
        /// <param name="chapterId">The chapter's unique identifier</param>
        /// <returns>Result containing list of reading progress records for the chapter</returns>
        public async Task<Result<List<ReadingProgress>>> GetByChapterIdAsync(Guid chapterId)
        {
            try
            {
                var chapterProgress = await _context.ReadingProgresses
                    .Where(x => x.ChapterId == chapterId)
                    .AsNoTracking()
                    .ToListAsync();

                return Result<List<ReadingProgress>>.Success(chapterProgress);
            }
            catch (Exception ex)
            {
                return Result<List<ReadingProgress>>.Failure($"Error when fetching chapter progress: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves reading progress for a specific user and chapter combination
        /// Useful for tracking progress on a specific chapter
        /// </summary>
        /// <param name="userId">The user's unique identifier</param>
        /// <param name="chapterId">The chapter's unique identifier</param>
        /// <returns>Result containing the reading progress record if found, otherwise null in success</returns>
        public async Task<Result<ReadingProgress>> GetByUserAndChapterAsync(Guid userId, Guid chapterId)
        {
            try
            {
                var readingProgress = await _context.ReadingProgresses
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.ChapterId == chapterId);

                // This is not an error - it's okay if no progress exists yet
                return Result<ReadingProgress>.Success(readingProgress);
            }
            catch (Exception ex)
            {
                return Result<ReadingProgress>.Failure($"Error when fetching reading progress: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new reading progress record
        /// </summary>
        /// <param name="progress">The reading progress entity to create</param>
        /// <returns>Result containing the created reading progress</returns>
        public async Task<Result<ReadingProgress>> CreateAsync(ReadingProgress progress)
        {
            try
            {
                if (progress == null)
                    return Result<ReadingProgress>.Failure("Reading progress cannot be null");

                await _context.ReadingProgresses.AddAsync(progress);
                await _context.SaveChangesAsync();
                return Result<ReadingProgress>.Success(progress);
            }
            catch (DbUpdateException dbEx)
            {
                return Result<ReadingProgress>.Failure($"Database error: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return Result<ReadingProgress>.Failure($"Error when creating reading progress: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing reading progress record
        /// Typically used to update WordsRead and LastReadAt
        /// </summary>
        /// <param name="progress">The reading progress entity with updated values</param>
        /// <returns>Result indicating success or failure of update operation</returns>
        public async Task<Result<ReadingProgress>> UpdateAsync(ReadingProgress progress)
        {
            try
            {
                if (progress == null)
                    return Result<ReadingProgress>.Failure("Reading progress cannot be null");

                var existingProgress = await _context.ReadingProgresses
                    .FirstOrDefaultAsync(x => x.Id == progress.Id);

                if (existingProgress == null)
                    return Result<ReadingProgress>.Failure($"Reading progress with ID {progress.Id} not found");

                _context.ReadingProgresses.Update(progress);
                await _context.SaveChangesAsync();
                return Result<ReadingProgress>.Success(progress);
            }
            catch (DbUpdateException dbEx)
            {
                return Result<ReadingProgress>.Failure($"Database error: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return Result<ReadingProgress>.Failure($"Error when updating reading progress: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a reading progress record
        /// </summary>
        /// <param name="id">The unique identifier of the reading progress to delete</param>
        /// <returns>Result indicating success or failure of delete operation</returns>
        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var readingProgress = await _context.ReadingProgresses
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (readingProgress == null)
                    return Result<bool>.Failure($"Reading progress with ID {id} not found");

                _context.ReadingProgresses.Remove(readingProgress);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (DbUpdateException dbEx)
            {
                return Result<bool>.Failure($"Database error: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error when deleting reading progress: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if a reading progress record exists
        /// </summary>
        /// <param name="id">The unique identifier to check</param>
        /// <returns>Result containing true if reading progress exists, false otherwise</returns>
        public async Task<Result<bool>> ExistsAsync(Guid id)
        {
            try
            {
                var exists = await _context.ReadingProgresses
                    .AnyAsync(x => x.Id == id);

                return Result<bool>.Success(exists);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error when checking if reading progress exists: {ex.Message}");
            }
        }

        /// <summary>
        /// Calculates total words read by a specific user across all chapters
        /// Useful for user statistics and achievements
        /// </summary>
        /// <param name="userId">The user's unique identifier</param>
        /// <returns>Result containing the total words read by the user</returns>
        public async Task<Result<int>> GetTotalWordsReadByUserAsync(Guid userId)
        {
            try
            {
                var totalWordsRead = await _context.ReadingProgresses
                    .Where(x => x.UserId == userId)
                    .SumAsync(x => x.WordsRead);

                return Result<int>.Success(totalWordsRead);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Error when calculating total words read: {ex.Message}");
            }
        }
    }
}
