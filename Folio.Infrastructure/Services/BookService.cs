using Folio.CORE.Entities;
using Folio.CORE.Enums;
using Folio.CORE.Interfaces.Repositories;
using Folio.CORE.Interfaces.Services;
using Folio.CORE.Responses;

namespace Folio.Infrastructure.Services
{
    /// <summary>
    /// BookService - implements business logic for book management
    /// Delegates data access to IBookRepository and provides book-related operations
    /// </summary>
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        /// <summary>
        /// Constructor - initializes service with book repository dependency
        /// </summary>
        /// <param name="bookRepository">The book repository for data access</param>
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        /// <summary>
        /// Retrieves a single book by ID
        /// </summary>
        /// <param name="id">The book's unique identifier</param>
        /// <returns>Result containing book if found, otherwise failure message</returns>
        public async Task<Result<Book>> GetBookByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return Result<Book>.Failure("Book ID cannot be empty");

                var result = await _bookRepository.GetByIdAsync(id);
                if (!result.IsSuccess || result.Value == null)
                    return Result<Book>.Failure(result.Error ?? "Book not found");

                return Result<Book>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<Book>.Failure($"Error retrieving book: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all books without pagination
        /// Warning: Use GetBooksPagedAsync for large datasets
        /// </summary>
        /// <returns>Result containing list of all books</returns>
        public async Task<Result<List<Book>>> GetAllBooksAsync()
        {
            try
            {
                var result = await _bookRepository.GetAllAsync();
                if (!result.IsSuccess)
                    return Result<List<Book>>.Failure(result.Error ?? "Error retrieving books");

                return Result<List<Book>>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<List<Book>>.Failure($"Error retrieving books: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves books with pagination for improved performance
        /// </summary>
        /// <param name="pageNumber">The page number (must be >= 1)</param>
        /// <param name="pageSize">The number of records per page (must be >= 1)</param>
        /// <returns>Result containing paginated book data with metadata</returns>
        public async Task<Result<PagedResponse<Book>>> GetBooksPagedAsync(int pageNumber, int pageSize)
        {
            try
            {
                if (pageNumber < 1)
                    return Result<PagedResponse<Book>>.Failure("Page number must be >= 1");
                if (pageSize < 1)
                    return Result<PagedResponse<Book>>.Failure("Page size must be >= 1");

                var result = await _bookRepository.GetPagedAsync(pageNumber, pageSize);
                if (!result.IsSuccess)
                    return Result<PagedResponse<Book>>.Failure(result.Error ?? "Error retrieving books");

                return Result<PagedResponse<Book>>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<PagedResponse<Book>>.Failure($"Error retrieving paged books: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all books filtered by literary genre
        /// </summary>
        /// <param name="genre">The literary genre to filter by</param>
        /// <returns>Result containing list of books matching the specified genre</returns>
        public async Task<Result<List<Book>>> GetBooksByGenreAsync(LiteraryGenres genre)
        {
            try
            {
                var result = await _bookRepository.GetByGenreAsync(genre);
                if (!result.IsSuccess)
                    return Result<List<Book>>.Failure(result.Error ?? "Error retrieving books by genre");

                return Result<List<Book>>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<List<Book>>.Failure($"Error retrieving books by genre: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new book with validation
        /// </summary>
        /// <param name="book">The book entity to create</param>
        /// <returns>Result containing the created book with generated identifier</returns>
        public async Task<Result<Book>> CreateBookAsync(Book book)
        {
            try
            {
                if (book == null)
                    return Result<Book>.Failure("Book cannot be null");

                if (string.IsNullOrWhiteSpace(book.Title))
                    return Result<Book>.Failure("Book title is required");

                if (string.IsNullOrWhiteSpace(book.Author))
                    return Result<Book>.Failure("Book author is required");

                // Set timestamps if not already set
                if (book.Id == Guid.Empty)
                    book.Id = Guid.NewGuid();
                if (book.CreatedAt == default)
                    book.CreatedAt = DateTime.UtcNow;

                var result = await _bookRepository.CreateAsync(book);
                if (!result.IsSuccess)
                    return Result<Book>.Failure(result.Error ?? "Error creating book");

                return Result<Book>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<Book>.Failure($"Error creating book: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing book with validation
        /// </summary>
        /// <param name="book">The book entity with updated values</param>
        /// <returns>Result containing the updated book or failure message</returns>
        public async Task<Result<Book>> UpdateBookAsync(Book book)
        {
            try
            {
                if (book == null)
                    return Result<Book>.Failure("Book cannot be null");

                if (book.Id == Guid.Empty)
                    return Result<Book>.Failure("Book ID cannot be empty");

                if (string.IsNullOrWhiteSpace(book.Title))
                    return Result<Book>.Failure("Book title is required");

                if (string.IsNullOrWhiteSpace(book.Author))
                    return Result<Book>.Failure("Book author is required");

                // Verify book exists before updating
                var existingBook = await _bookRepository.GetByIdAsync(book.Id);
                if (!existingBook.IsSuccess || existingBook.Value == null)
                    return Result<Book>.Failure("Book not found");

                var result = await _bookRepository.UpdateAsync(book);
                if (!result.IsSuccess)
                    return Result<Book>.Failure(result.Error ?? "Error updating book");

                return Result<Book>.Success(result.Value);
            }
            catch (Exception ex)
            {
                return Result<Book>.Failure($"Error updating book: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a book and all associated chapters and reading progress
        /// </summary>
        /// <param name="id">The unique identifier of the book to delete</param>
        /// <returns>Result indicating success or failure of deletion</returns>
        public async Task<Result<bool>> DeleteBookAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return Result<bool>.Failure("Book ID cannot be empty");

                var result = await _bookRepository.DeleteAsync(id);
                if (!result.IsSuccess)
                    return Result<bool>.Failure(result.Error ?? "Error deleting book");

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error deleting book: {ex.Message}");
            }
        }
    }
}
