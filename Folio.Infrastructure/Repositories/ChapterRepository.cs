using Folio.CORE.Interfaces.Repositories;
using Folio.Infrastructure.Data;
using Folio.CORE.Responses;
using Folio.CORE.Entities;
using Microsoft.EntityFrameworkCore;

namespace Folio.Infrastructure.Repositories
{
    public class ChapterRepository : IChapterRepository
    {
        private readonly ApplicationDbContext _context;
        public ChapterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Chapter>> CreateAsync(Chapter chapter)
        {
            try
            {
                if (chapter == null)
                    return Result<Chapter>.Failure("chapter is null");

                await _context.Chapters.AddAsync(chapter);
                await _context.SaveChangesAsync();
                return Result<Chapter>.Success(chapter);
            }
            catch (DbUpdateException dbEx)
            {
                return Result<Chapter>.Failure($"Database error: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return Result<Chapter>.Failure($"Error when creating chapter: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var chapter = await _context.Chapters.FirstOrDefaultAsync(x => x.Id == id);
                if (chapter == null)
                    return Result<bool>.Failure($"Chapter with ID {id} not found");

                _context.Chapters.Remove(chapter);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (DbUpdateException dbEx)
            {
                return Result<bool>.Failure($"Database error: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error when deleting chapter: {ex.Message}");
            }
        }

        public async Task<Result<bool>> ExistsAsync(Guid id)
        {
            try
            {
                var exists = await _context.Chapters.AnyAsync(x => x.Id == id);
                return Result<bool>.Success(exists);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error when checking if chapter exists: {ex.Message}");
            }
        }

        public async Task<Result<List<Chapter>>> GetAllAsync()
        {
            try
            {
                var chapters = await _context.Chapters
                    .AsNoTracking()
                    .OrderBy(x => x.OrderIndex)
                    .ToListAsync();

                if (chapters == null || chapters.Count == 0)
                    return Result<List<Chapter>>.Failure("No chapters found");

                return Result<List<Chapter>>.Success(chapters);
            }
            catch (Exception ex)
            {
                return Result<List<Chapter>>.Failure($"Error when fetching all chapters: {ex.Message}");
            }
        }

        public async Task<Result<List<Chapter>>> GetByBookIdAsync(Guid bookId)
        {
            try
            {
                //var chapters = _context.Chapters.Where(x => x.BookId == bookId).AsNoTracking().OrderBy(x => x.ReadingProgressRepository).ToList();
                var chapters =  await _context.Chapters
                    .Where(x => x.BookId == bookId)
                    .AsNoTracking()
                    .OrderBy(x => x.OrderIndex)
                    .ToListAsync();
                    if (chapters == null || chapters.Count == 0)
                    return Result<List<Chapter>>.Failure("Error");
                return Result<List<Chapter>>.Success(chapters);
            }
            catch (Exception ex)
            {
                return Result<List<Chapter>>.Failure($"Error: {ex}");
            }
        }
        public async Task<Result<Chapter>> GetByIdAsync(Guid id)
        {
            try
            {
                var chapter = await _context.Chapters
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (chapter == null)
                    return Result<Chapter>.Failure("Error");
                return Result<Chapter>.Success(chapter);
            } catch (Exception ex)
            {
                return Result<Chapter>.Failure($"Error: {ex}");
            }
        }

        public async Task<Result<PagedResponse<Chapter>>> GetPagedByBookIdAsync(Guid bookId, int pageNumber, int pageSize)
        {
            try
            {
                if (pageNumber < 1)
                    return Result<PagedResponse<Chapter>>.Failure("Page number must be >= 1");
                if (pageSize < 1)
                    return Result<PagedResponse<Chapter>>.Failure("Page size must be >= 1");

                var totalRecords = await _context.Chapters
                    .Where(x => x.BookId == bookId)
                    .CountAsync();

                var chapters = await _context.Chapters
                    .Where(x => x.BookId == bookId)
                    .AsNoTracking()
                    .OrderBy(x => x.OrderIndex)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var pagedResponse = new PagedResponse<Chapter>(chapters, pageNumber, pageSize, totalRecords);
                return Result<PagedResponse<Chapter>>.Success(pagedResponse);
            }
            catch (Exception ex)
            {
                return Result<PagedResponse<Chapter>>.Failure($"Error when fetching paged chapters: {ex.Message}");
            }
        }
        public async Task<Result<Chapter>> UpdateAsync(Chapter chapter)
        {
            try
            {
                if(chapter == null) 
                    return Result<Chapter>.Failure("Chapter cannot be null");

                var existingChapter = await _context.Chapters
                    .FirstOrDefaultAsync(x => x.Id == chapter.Id);

                if (existingChapter == null)
                    return Result<Chapter>.Failure($"Chapter with ID {chapter.Id} not found");

                _context.Chapters.Update(chapter);
                await _context.SaveChangesAsync();
                return Result<Chapter>.Success(chapter);   
            }
            catch (DbUpdateException dbEx)
            {
                return Result<Chapter>.Failure($"Database error: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return Result<Chapter>.Failure($"Error when updating chapter: {ex.Message}");
            }
        }
    }
}
