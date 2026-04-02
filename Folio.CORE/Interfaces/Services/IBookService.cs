using Folio.CORE.Entities;
using Folio.CORE.Responses;

namespace Folio.CORE.Interfaces.Services
{
    /// <summary>
    /// IBookService - defines business logic operations for books
    /// Provides methods for retrieving, creating, updating, and deleting books with validation and error handling
    /// </summary>
    public interface IBookService
    {
        /// <summary>
        /// Retrieves a single book by its unique identifier
        /// </summary>
        /// <param name="id">The book's unique identifier</param>
        /// <returns>Result containing the book if found, otherwise failure message</returns>
        Task<Result<Book>> GetBookByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all books without pagination
        /// Warning: Use GetBooksPagedAsync for large datasets to avoid performance issues
        /// </summary>
        /// <returns>Result containing list of all books</returns>
        Task<Result<List<Book>>> GetAllBooksAsync();

        /// <summary>
        /// Retrieves books with pagination support for improved performance on large datasets
        /// </summary>
        /// <param name="pageNumber">The page number (must be >= 1)</param>
        /// <param name="pageSize">The number of records per page (must be >= 1)</param>
        /// <returns>Result containing paginated book data with metadata</returns>
        Task<Result<PagedResponse<Book>>> GetBooksPagedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves all books filtered by literary genre
        /// Useful for category browsing and genre-based queries
        /// </summary>
        /// <param name="genre">The literary genre to filter by</param>
        /// <returns>Result containing list of books matching the specified genre</returns>
        Task<Result<List<Book>>> GetBooksByGenreAsync(Enums.LiteraryGenres genre);

        /// <summary>
        /// Creates a new book with validation
        /// Validates required fields and ensures data consistency
        /// </summary>
        /// <param name="book">The book entity to create with all required fields</param>
        /// <returns>Result containing the created book with generated identifier</returns>
        Task<Result<Book>> CreateBookAsync(Book book);

        /// <summary>
        /// Updates an existing book
        /// Validates the book exists before applying changes
        /// </summary>
        /// <param name="book">The book entity with updated values</param>
        /// <returns>Result containing the updated book or failure message if not found</returns>
        Task<Result<Book>> UpdateBookAsync(Book book);

        /// <summary>
        /// Deletes a book and all associated chapters and reading progress records
        /// </summary>
        /// <param name="id">The unique identifier of the book to delete</param>
        /// <returns>Result indicating success or failure of delete operation</returns>
        Task<Result<bool>> DeleteBookAsync(Guid id);
    }
}
