using Folio.CORE.Entities;
using Folio.CORE.Enums;
using Folio.CORE.Interfaces.Services;
using Folio.CORE.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Folio.API.Controllers
{
    /// <summary>
    /// BooksController - REST API endpoints for book management
    /// Handles CRUD operations and book-related queries
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        /// <summary>
        /// Constructor - initializes controller with book service dependency
        /// </summary>
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// GET /api/books/{id} - Retrieves a single book by ID
        /// </summary>
        /// <param name="id">The book's unique identifier</param>
        /// <returns>Book details if found</returns>
        /// <response code="200">Book found and returned</response>
        /// <response code="404">Book not found</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<Book>>> GetBookById(Guid id)
        {
            var result = await _bookService.GetBookByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// GET /api/books - Retrieves all books (warning: use pagination for large datasets)
        /// </summary>
        /// <returns>List of all books</returns>
        /// <response code="200">Books retrieved successfully</response>
        /// <response code="400">Error retrieving books</response>
        [HttpGet]
        public async Task<ActionResult<Result<List<Book>>>> GetAllBooks()
        {
            var result = await _bookService.GetAllBooksAsync();
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// GET /api/books/paged - Retrieves books with pagination
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Records per page (default: 10)</param>
        /// <returns>Paginated list of books</returns>
        /// <response code="200">Books retrieved successfully</response>
        /// <response code="400">Invalid pagination parameters</response>
        [HttpGet("paged")]
        public async Task<ActionResult<Result<PagedResponse<Book>>>> GetBooksPaged(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            var result = await _bookService.GetBooksPagedAsync(pageNumber, pageSize);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// GET /api/books/by-genre/{genre} - Retrieves books filtered by genre
        /// </summary>
        /// <param name="genre">The literary genre to filter by</param>
        /// <returns>List of books in specified genre</returns>
        /// <response code="200">Books retrieved successfully</response>
        /// <response code="400">Error retrieving books</response>
        [HttpGet("by-genre/{genre}")]
        public async Task<ActionResult<Result<List<Book>>>> GetBooksByGenre(LiteraryGenres genre)
        {
            var result = await _bookService.GetBooksByGenreAsync(genre);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// POST /api/books - Creates a new book
        /// </summary>
        /// <param name="book">The book entity to create</param>
        /// <returns>The created book with generated ID</returns>
        /// <response code="201">Book created successfully</response>
        /// <response code="400">Invalid book data</response>
        [HttpPost]
        public async Task<ActionResult<Result<Book>>> CreateBook([FromBody] Book book)
        {
            var result = await _bookService.CreateBookAsync(book);
            if (!result.IsSuccess)
                return BadRequest(result);
            return CreatedAtAction(nameof(GetBookById), new { id = result.Value?.Id }, result);
        }

        /// <summary>
        /// PUT /api/books/{id} - Updates an existing book
        /// </summary>
        /// <param name="id">The book's unique identifier</param>
        /// <param name="book">The updated book data</param>
        /// <returns>The updated book</returns>
        /// <response code="200">Book updated successfully</response>
        /// <response code="400">Invalid book data or ID mismatch</response>
        /// <response code="404">Book not found</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<Result<Book>>> UpdateBook(Guid id, [FromBody] Book book)
        {
            if (id != book.Id)
                return BadRequest(Result<Book>.Failure("ID mismatch: URL ID does not match book ID"));

            var result = await _bookService.UpdateBookAsync(book);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// DELETE /api/books/{id} - Deletes a book and all associated data
        /// </summary>
        /// <param name="id">The unique identifier of the book to delete</param>
        /// <returns>Deletion result</returns>
        /// <response code="200">Book deleted successfully</response>
        /// <response code="400">Error deleting book</response>
        /// <response code="404">Book not found</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<bool>>> DeleteBook(Guid id)
        {
            var result = await _bookService.DeleteBookAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
