using Folio.CORE.Entities;
using Folio.CORE.Interfaces.Repositories;
using Folio.CORE.Interfaces.Services;
using Folio.CORE.Responses;

namespace Folio.Infrastructure.Services
{
    /// <summary>
    /// ChapterService - implements business logic for chapter management
    /// Delegates data access to IChapterRepository and provides chapter-related operations
    /// </summary>
    public class ChapterService : IChapterService
    {
        private readonly IChapterRepository _chapterRepository;

        /// <summary>
        /// Constructor - initializes service with chapter repository dependency
        /// </summary>
        /// <param name="chapterRepository">The chapter repository for data access</param>
        public ChapterService(IChapterRepository chapterRepository)
        {
            _chapterRepository = chapterRepository;
        }

        /// <summary>
        /// Retrieves a single chapter by ID
        /// </summary>
        /// <param name="id">The chapter's unique identifier</param>
        /// <returns>Result containing chapter if found, otherwise failure message</returns>
        public async Task<Result<Chapter>> GetChapterByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return Result<Chapter>.Failure("Chapter ID cannot be empty");

                var result = await _chapterRepository.GetByIdAsync(id);
                if (!result.IsSuccess || result.Value == null)
                    return Result<Chapter>.Failure(result.Error ?? "Chapter not found");

                return Result<Chapter>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<Chapter>.Failure($"Error retrieving chapter: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all chapters for a specific book, sorted by reading order
        /// </summary>
        /// <param name="bookId">The book's unique identifier</param>
        /// <returns>Result containing list of chapters ordered by OrderIndex</returns>
        public async Task<Result<List<Chapter>>> GetChaptersByBookIdAsync(Guid bookId)
        {
            try
            {
                if (bookId == Guid.Empty)
                    return Result<List<Chapter>>.Failure("Book ID cannot be empty");

                var result = await _chapterRepository.GetByBookIdAsync(bookId);
                if (!result.IsSuccess)
                    return Result<List<Chapter>>.Failure(result.Error ?? "Error retrieving chapters");

                return Result<List<Chapter>>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<List<Chapter>>.Failure($"Error retrieving chapters: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves chapters for a specific book with pagination
        /// </summary>
        /// <param name="bookId">The book's unique identifier</param>
        /// <param name="pageNumber">The page number (must be >= 1)</param>
        /// <param name="pageSize">The number of records per page (must be >= 1)</param>
        /// <returns>Result containing paginated chapter data for the book</returns>
        public async Task<Result<PagedResponse<Chapter>>> GetChaptersPagedByBookIdAsync(Guid bookId, int pageNumber, int pageSize)
        {
            try
            {
                if (bookId == Guid.Empty)
                    return Result<PagedResponse<Chapter>>.Failure("Book ID cannot be empty");

                if (pageNumber < 1)
                    return Result<PagedResponse<Chapter>>.Failure("Page number must be >= 1");

                if (pageSize < 1)
                    return Result<PagedResponse<Chapter>>.Failure("Page size must be >= 1");

                var result = await _chapterRepository.GetPagedByBookIdAsync(bookId, pageNumber, pageSize);
                if (!result.IsSuccess)
                    return Result<PagedResponse<Chapter>>.Failure(result.Error ?? "Error retrieving chapters");

                return Result<PagedResponse<Chapter>>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<PagedResponse<Chapter>>.Failure($"Error retrieving paged chapters: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new chapter with validation
        /// </summary>
        /// <param name="chapter">The chapter entity to create</param>
        /// <returns>Result containing the created chapter with generated identifier</returns>
        public async Task<Result<Chapter>> CreateChapterAsync(Chapter chapter)
        {
            try
            {
                if (chapter == null)
                    return Result<Chapter>.Failure("Chapter cannot be null");

                if (chapter.BookId == Guid.Empty)
                    return Result<Chapter>.Failure("Book ID is required");

                if (string.IsNullOrWhiteSpace(chapter.Title))
                    return Result<Chapter>.Failure("Chapter title is required");

                if (chapter.OrderIndex < 0)
                    return Result<Chapter>.Failure("Chapter order index must be >= 0");

                // Set ID if not already set
                if (chapter.Id == Guid.Empty)
                    chapter.Id = Guid.NewGuid();

                var result = await _chapterRepository.CreateAsync(chapter);
                if (!result.IsSuccess)
                    return Result<Chapter>.Failure(result.Error ?? "Error creating chapter");

                return Result<Chapter>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<Chapter>.Failure($"Error creating chapter: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing chapter with validation
        /// </summary>
        /// <param name="chapter">The chapter entity with updated values</param>
        /// <returns>Result containing the updated chapter or failure message</returns>
        public async Task<Result<Chapter>> UpdateChapterAsync(Chapter chapter)
        {
            try
            {
                if (chapter == null)
                    return Result<Chapter>.Failure("Chapter cannot be null");

                if (chapter.Id == Guid.Empty)
                    return Result<Chapter>.Failure("Chapter ID cannot be empty");

                if (chapter.BookId == Guid.Empty)
                    return Result<Chapter>.Failure("Book ID is required");

                if (string.IsNullOrWhiteSpace(chapter.Title))
                    return Result<Chapter>.Failure("Chapter title is required");

                if (chapter.OrderIndex < 0)
                    return Result<Chapter>.Failure("Chapter order index must be >= 0");

                // Verify chapter exists before updating
                var existingChapter = await _chapterRepository.GetByIdAsync(chapter.Id);
                if (!existingChapter.IsSuccess || existingChapter.Value == null)
                    return Result<Chapter>.Failure("Chapter not found");

                var result = await _chapterRepository.UpdateAsync(chapter);
                if (!result.IsSuccess)
                    return Result<Chapter>.Failure(result.Error ?? "Error updating chapter");

                return Result<Chapter>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<Chapter>.Failure($"Error updating chapter: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a chapter and all associated reading progress records
        /// </summary>
        /// <param name="id">The unique identifier of the chapter to delete</param>
        /// <returns>Result indicating success or failure of deletion</returns>
        public async Task<Result<bool>> DeleteChapterAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return Result<bool>.Failure("Chapter ID cannot be empty");

                var result = await _chapterRepository.DeleteAsync(id);
                if (!result.IsSuccess)
                    return Result<bool>.Failure(result.Error ?? "Error deleting chapter");

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error deleting chapter: {ex.Message}");
            }
        }
    }
}
