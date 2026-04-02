using Folio.CORE.Entities;
using Folio.CORE.Enums;
using Folio.CORE.Interfaces.Repositories;
using Folio.CORE.Responses;
using Folio.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Folio.Infrastructure.Repositories
{
    /// <summary>
    /// BookRepository - implements all data access operations for Book entity
    /// Handles CRUD operations and specialized queries like filtering by genre or title
    /// </summary>
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor - initializes repository with database context
        /// </summary>
        /// <param name="context">The database context instance</param>
        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a single book by its unique identifier
        /// </summary>
        /// <param name="id">The book's unique identifier</param>
        /// <returns>Result containing the book if found, otherwise failure message</returns>
        public async Task<Result<Book>> GetByIdAsync(Guid id)
        {
            try
            {
                var book = await _context.Books
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (book == null)
                    return Result<Book>.Failure($"Book with ID {id} not found");

                return Result<Book>.Success(book);
            }
            catch (Exception ex)
            {
                return Result<Book>.Failure($"Error when fetching book: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all books from the database
        /// Warning: Use GetPagedAsync for large datasets
        /// </summary>
        /// <returns>Result containing list of all books</returns>
        public async Task<Result<List<Book>>> GetAllAsync()
        {
            try
            {
                var books = await _context.Books
                    .AsNoTracking()
                    .OrderBy(b => b.CreatedAt)
                    .ToListAsync();

                return Result<List<Book>>.Success(books);
            }
            catch (Exception ex)
            {
                return Result<List<Book>>.Failure($"Error when fetching all books: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a paginated list of books
        /// Supports pagination with specified page number and page size
        /// </summary>
        /// <param name="pageNumber">Page number (starts from 1)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>Result containing paginated response with books</returns>
        public async Task<Result<PagedResponse<Book>>> GetPagedAsync(int pageNumber, int pageSize)
        {
            try
            {
                if (pageNumber < 1)
                    return Result<PagedResponse<Book>>.Failure("Page number must be >= 1");
                if (pageSize < 1)
                    return Result<PagedResponse<Book>>.Failure("Page size must be >= 1");

                var totalRecords = await _context.Books.CountAsync();

                var books = await _context.Books
                    .AsNoTracking()
                    .OrderBy(b => b.CreatedAt)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var pagedResponse = new PagedResponse<Book>(books, pageNumber, pageSize, totalRecords);
                return Result<PagedResponse<Book>>.Success(pagedResponse);
            }
            catch (Exception ex)
            {
                return Result<PagedResponse<Book>>.Failure($"Error when fetching paged books: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all books filtered by literary genre
        /// </summary>
        /// <param name="genre">The literary genre to filter by</param>
        /// <returns>Result containing list of books matching the genre</returns>
        public async Task<Result<List<Book>>> GetByGenreAsync(LiteraryGenres genre)
        {
            try
            {
                var books = await _context.Books
                    .AsNoTracking()
                    .Where(b => b.Genres == genre)
                    .OrderBy(b => b.CreatedAt)
                    .ToListAsync();

                return Result<List<Book>>.Success(books);
            }
            catch (Exception ex)
            {
                return Result<List<Book>>.Failure($"Error when fetching books by genre: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a single book by its title
        /// Performs case-insensitive search
        /// </summary>
        /// <param name="title">The book's title to search for</param>
        /// <returns>Result containing the book if found, otherwise failure message</returns>
        public async Task<Result<Book>> GetByTitleAsync(string title)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title))
                    return Result<Book>.Failure("Title cannot be empty");

                var book = await _context.Books
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.Title.ToLower() == title.ToLower());

                if (book == null)
                    return Result<Book>.Failure($"Book with title '{title}' not found");

                return Result<Book>.Success(book);
            }
            catch (Exception ex)
            {
                return Result<Book>.Failure($"Error when fetching book by title: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new book in the database
        /// </summary>
        /// <param name="book">The book entity to create</param>
        /// <returns>Result containing the created book with generated ID</returns>
        public async Task<Result<Book>> CreateAsync(Book book)
        {
            try
            {
                if (book == null)
                    return Result<Book>.Failure("Book cannot be null");

                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();
                return Result<Book>.Success(book);
            }
            catch (DbUpdateException dbEx)
            {
                return Result<Book>.Failure($"Database error: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return Result<Book>.Failure($"Error when creating book: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing book in the database
        /// </summary>
        /// <param name="book">The book entity with updated values</param>
        /// <returns>Result indicating success or failure of update operation</returns>
        public async Task<Result<Book>> UpdateAsync(Book book)
        {
            try
            {
                if (book == null)
                    return Result<Book>.Failure("Book cannot be null");

                var existingBook = await _context.Books
                    .FirstOrDefaultAsync(b => b.Id == book.Id);

                if (existingBook == null)
                    return Result<Book>.Failure($"Book with ID {book.Id} not found");

                _context.Books.Update(book);
                await _context.SaveChangesAsync();
                return Result<Book>.Success(book);
            }
            catch (DbUpdateException dbEx)
            {
                return Result<Book>.Failure($"Database error: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return Result<Book>.Failure($"Error when updating book: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a book from the database by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the book to delete</param>
        /// <returns>Result indicating success or failure of delete operation</returns>
        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

                if (book == null)
                    return Result<bool>.Failure($"Book with ID {id} not found");

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (DbUpdateException dbEx)
            {
                return Result<bool>.Failure($"Database error: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error when deleting book: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if a book exists in the database by its ID
        /// </summary>
        /// <param name="id">The unique identifier to check</param>
        /// <returns>Result containing true if book exists, false otherwise</returns>
        public async Task<Result<bool>> ExistsAsync(Guid id)
        {
            try
            {
                var exists = await _context.Books.AnyAsync(b => b.Id == id);
                return Result<bool>.Success(exists);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error when checking if book exists: {ex.Message}");
            }
        }
    }
}
