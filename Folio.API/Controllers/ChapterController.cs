using Folio.CORE.Entities;
using Folio.CORE.Interfaces.Services;
using Folio.CORE.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Folio.API.Controllers
{
    /// <summary>
    /// ChaptersController - REST API endpoints for chapter management
    /// Handles CRUD operations and chapter-related queries
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ChaptersController : ControllerBase
    {
        private readonly IChapterService _chapterService;

        /// <summary>
        /// Constructor - initializes controller with chapter service dependency
        /// </summary>
        public ChaptersController(IChapterService chapterService)
        {
            _chapterService = chapterService;
        }

        /// <summary>
        /// GET /api/chapters/{id} - Retrieves a single chapter by ID
        /// </summary>
        /// <param name="id">The chapter's unique identifier</param>
        /// <returns>Chapter details if found</returns>
        /// <response code="200">Chapter found and returned</response>
        /// <response code="404">Chapter not found</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<Chapter>>> GetChapterById(Guid id)
        {
            var result = await _chapterService.GetChapterByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// GET /api/chapters/by-book/{bookId} - Retrieves all chapters for a specific book
        /// </summary>
        /// <param name="bookId">The book's unique identifier</param>
        /// <returns>List of chapters in the book, sorted by reading order</returns>
        /// <response code="200">Chapters retrieved successfully</response>
        /// <response code="400">Invalid book ID or error retrieving chapters</response>
        [HttpGet("by-book/{bookId}")]
        public async Task<ActionResult<Result<List<Chapter>>>> GetChaptersByBookId(Guid bookId)
        {
            var result = await _chapterService.GetChaptersByBookIdAsync(bookId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// GET /api/chapters/paged-by-book/{bookId} - Retrieves chapters for a book with pagination
        /// </summary>
        /// <param name="bookId">The book's unique identifier</param>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Records per page (default: 10)</param>
        /// <returns>Paginated list of chapters</returns>
        /// <response code="200">Chapters retrieved successfully</response>
        /// <response code="400">Invalid parameters</response>
        [HttpGet("paged-by-book/{bookId}")]
        public async Task<ActionResult<Result<PagedResponse<Chapter>>>> GetChaptersPagedByBookId(
            Guid bookId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _chapterService.GetChaptersPagedByBookIdAsync(bookId, pageNumber, pageSize);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// POST /api/chapters - Creates a new chapter
        /// </summary>
        /// <param name="chapter">The chapter entity to create</param>
        /// <returns>The created chapter with generated ID</returns>
        /// <response code="201">Chapter created successfully</response>
        /// <response code="400">Invalid chapter data</response>
        [HttpPost]
        public async Task<ActionResult<Result<Chapter>>> CreateChapter([FromBody] Chapter chapter)
        {
            var result = await _chapterService.CreateChapterAsync(chapter);
            if (!result.IsSuccess)
                return BadRequest(result);
            return CreatedAtAction(nameof(GetChapterById), new { id = result.Value?.Id }, result);
        }

        /// <summary>
        /// PUT /api/chapters/{id} - Updates an existing chapter
        /// </summary>
        /// <param name="id">The chapter's unique identifier</param>
        /// <param name="chapter">The updated chapter data</param>
        /// <returns>The updated chapter</returns>
        /// <response code="200">Chapter updated successfully</response>
        /// <response code="400">Invalid chapter data or ID mismatch</response>
        /// <response code="404">Chapter not found</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<Result<Chapter>>> UpdateChapter(Guid id, [FromBody] Chapter chapter)
        {
            if (id != chapter.Id)
                return BadRequest(Result<Chapter>.Failure("ID mismatch: URL ID does not match chapter ID"));

            var result = await _chapterService.UpdateChapterAsync(chapter);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// DELETE /api/chapters/{id} - Deletes a chapter and all associated reading progress
        /// </summary>
        /// <param name="id">The unique identifier of the chapter to delete</param>
        /// <returns>Deletion result</returns>
        /// <response code="200">Chapter deleted successfully</response>
        /// <response code="400">Error deleting chapter</response>
        /// <response code="404">Chapter not found</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<bool>>> DeleteChapter(Guid id)
        {
            var result = await _chapterService.DeleteChapterAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
