using Folio.CORE.Entities;
using Folio.CORE.Responses;

namespace Folio.CORE.Interfaces.Services
{
    /// <summary>
    /// IChapterService - defines business logic operations for book chapters
    /// Provides methods for managing chapters within books including retrieval, creation, and modification
    /// </summary>
    public interface IChapterService
    {
        /// <summary>
        /// Retrieves a single chapter by its unique identifier
        /// </summary>
        /// <param name="id">The chapter's unique identifier</param>
        /// <returns>Result containing the chapter if found, otherwise failure message</returns>
        Task<Result<Chapter>> GetChapterByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all chapters belonging to a specific book, sorted by reading order
        /// </summary>
        /// <param name="bookId">The book's unique identifier</param>
        /// <returns>Result containing list of chapters ordered by OrderIndex</returns>
        Task<Result<List<Chapter>>> GetChaptersByBookIdAsync(Guid bookId);

        /// <summary>
        /// Retrieves chapters for a specific book with pagination support
        /// Useful for incremental loading of chapters in large books
        /// </summary>
        /// <param name="bookId">The book's unique identifier</param>
        /// <param name="pageNumber">The page number (must be >= 1)</param>
        /// <param name="pageSize">The number of records per page (must be >= 1)</param>
        /// <returns>Result containing paginated chapter data for the book</returns>
        Task<Result<PagedResponse<Chapter>>> GetChaptersPagedByBookIdAsync(Guid bookId, int pageNumber, int pageSize);

        /// <summary>
        /// Creates a new chapter with validation
        /// Validates book exists and chapter properties are valid
        /// </summary>
        /// <param name="chapter">The chapter entity to create</param>
        /// <returns>Result containing the created chapter with generated identifier</returns>
        Task<Result<Chapter>> CreateChapterAsync(Chapter chapter);

        /// <summary>
        /// Updates an existing chapter
        /// Validates the chapter exists before applying changes
        /// </summary>
        /// <param name="chapter">The chapter entity with updated values</param>
        /// <returns>Result containing the updated chapter or failure message if not found</returns>
        Task<Result<Chapter>> UpdateChapterAsync(Chapter chapter);

        /// <summary>
        /// Deletes a chapter and all associated reading progress records
        /// </summary>
        /// <param name="id">The unique identifier of the chapter to delete</param>
        /// <returns>Result indicating success or failure of delete operation</returns>
        Task<Result<bool>> DeleteChapterAsync(Guid id);
    }
}
