using Folio.CORE.Entities;
using Folio.CORE.Responses;

namespace Folio.CORE.Interfaces.Repositories
{
    public interface IBookRepository
    {
        Task<Result<Book>> GetByIdAsync(Guid id);
        Task<Result<List<Book>>> GetAllAsync();
        Task<Result<PagedResponse<Book>>> GetPagedAsync(int pageNumber, int pageSize);
        Task<Result<List<Book>>> GetByGenreAsync(Folio.CORE.Enums.LiteraryGenres genre);
        Task<Result<Book>> GetByTitleAsync(string title);
        Task<Result<Book>> CreateAsync(Book book);
        Task<Result<Book>> UpdateAsync(Book book);
        Task<Result<bool>> DeleteAsync(Guid id);
        Task<Result<bool>> ExistsAsync(Guid id);
    }
}
