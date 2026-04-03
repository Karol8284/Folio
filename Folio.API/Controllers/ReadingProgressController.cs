using Folio.CORE.Entities;
using Folio.CORE.Interfaces.Services;
using Folio.CORE.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Folio.API.Controllers
{
    /// <summary>
    /// ReadingProgressController - REST API endpoints for reading progress tracking
    /// Handles CRUD operations and user reading statistics
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReadingProgressController : ControllerBase
    {
        private readonly IReadingProgressService _readingProgressService;

        /// <summary>
        /// Constructor - initializes controller with reading progress service dependency
        /// </summary>
        public ReadingProgressController(IReadingProgressService readingProgressService)
        {
            _readingProgressService = readingProgressService;
        }

        /// <summary>
        /// GET /api/readingprogress/{id} - Retrieves a single reading progress record by ID
        /// </summary>
        /// <param name="id">The reading progress unique identifier</param>
        /// <returns>Reading progress details if found</returns>
        /// <response code="200">Reading progress found and returned</response>
        /// <response code="404">Reading progress not found</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<ReadingProgress>>> GetReadingProgressById(Guid id)
        {
            var result = await _readingProgressService.GetByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// GET /api/readingprogress/by-user/{userId} - Retrieves all reading progress for a user
        /// </summary>
        /// <param name="userId">The user's unique identifier</param>
        /// <returns>List of reading progress records for the user</returns>
        /// <response code="200">Reading progress retrieved successfully</response>
        /// <response code="400">Error retrieving reading progress</response>
        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<Result<List<ReadingProgress>>>> GetUserReadingProgress(Guid userId)
        {
            var result = await _readingProgressService.GetUserProgressAsync(userId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// GET /api/readingprogress/user-chapter/{userId}/{chapterId} - Gets or creates reading progress for user-chapter combination
        /// </summary>
        /// <param name="userId">The user's unique identifier</param>
        /// <param name="chapterId">The chapter's unique identifier</param>
        /// <returns>Reading progress record (existing or newly created)</returns>
        /// <response code="200">Reading progress retrieved or created successfully</response>
        /// <response code="400">Error with reading progress</response>
        [HttpGet("user-chapter/{userId}/{chapterId}")]
        public async Task<ActionResult<Result<ReadingProgress>>> GetOrCreateReadingProgress(Guid userId, Guid chapterId)
        {
            var result = await _readingProgressService.GetOrCreateReadingProgressAsync(userId, chapterId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// GET /api/readingprogress/total-words/{userId} - Calculates total words read by a user
        /// </summary>
        /// <param name="userId">The user's unique identifier</param>
        /// <returns>Total words read across all chapters</returns>
        /// <response code="200">Total words calculated successfully</response>
        /// <response code="400">Error calculating total words</response>
        [HttpGet("total-words/{userId}")]
        public async Task<ActionResult<Result<int>>> GetTotalWordsReadByUser(Guid userId)
        {
            var result = await _readingProgressService.GetTotalWordsReadByUserAsync(userId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// POST /api/readingprogress - Creates a new reading progress record
        /// </summary>
        /// <param name="readingProgress">The reading progress entity to create</param>
        /// <returns>The created reading progress with generated ID</returns>
        /// <response code="201">Reading progress created successfully</response>
        /// <response code="400">Invalid reading progress data</response>
        [HttpPost]
        public async Task<ActionResult<Result<ReadingProgress>>> CreateReadingProgress([FromBody] ReadingProgress readingProgress)
        {
            var result = await _readingProgressService.UpdateAsync(readingProgress);
            if (!result.IsSuccess)
                return BadRequest(result);
            return CreatedAtAction(nameof(GetReadingProgressById), new { id = result.Value?.Id }, result);
        }

        /// <summary>
        /// PUT /api/readingprogress/{id} - Updates an existing reading progress record
        /// </summary>
        /// <param name="id">The reading progress unique identifier</param>
        /// <param name="readingProgress">The updated reading progress data</param>
        /// <returns>The updated reading progress</returns>
        /// <response code="200">Reading progress updated successfully</response>
        /// <response code="400">Invalid data or ID mismatch</response>
        /// <response code="404">Reading progress not found</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<Result<ReadingProgress>>> UpdateReadingProgress(Guid id, [FromBody] ReadingProgress readingProgress)
        {
            if (id != readingProgress.Id)
                return BadRequest(Result<ReadingProgress>.Failure("ID mismatch: URL ID does not match reading progress ID"));

            var result = await _readingProgressService.UpdateAsync(readingProgress);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// DELETE /api/readingprogress/{id} - Deletes a reading progress record
        /// </summary>
        /// <param name="id">The unique identifier of the reading progress to delete</param>
        /// <returns>Deletion result</returns>
        /// <response code="200">Reading progress deleted successfully</response>
        /// <response code="400">Error deleting reading progress</response>
        /// <response code="404">Reading progress not found</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<bool>>> DeleteReadingProgress(Guid id)
        {
            var result = await _readingProgressService.DeleteAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
