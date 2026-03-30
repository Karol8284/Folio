using Folio.CORE.Entities;
using Folio.CORE.Responses;

namespace Folio.CORE.Interfaces
{
    public interface IBookService
    {
        Task<Result<Book>> GetBookByIdAsync(Guid id);
        Task<Result<List<Book>>> GetAllBooksAsync();
        Task<Result<PagedResponse<Book>>> GetBooksPagedAsync(int pageNumber, int pageSize);
        Task<Result<List<Book>>> GetBooksByGenreAsync(Folio.CORE.Enums.LiteraryGenres genre);
        Task<Result<Book>> CreateBookAsync(Book book);
        Task<Result<Book>> UpdateBookAsync(Book book);
        Task<Result<bool>> DeleteBookAsync(Guid id);
    }
}
