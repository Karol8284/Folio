using Folio.CORE.Enums;

namespace Folio.CORE.Entities
{
    /// <summary>
    /// Book - represents a book in the library with metadata and reading content
    /// Aggregate root for chapters and reading material organization
    /// </summary>
    public class Book
    {
        /// <summary>Unique identifier for the book (primary key)</summary>
        public Guid Id { get; set; }

        /// <summary>Title of the book displayed in the application</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>Author's name or pen name</summary>
        public string Author { get; set; } = string.Empty;

        /// <summary>Literary genre classification (Fiction, Fantasy, Thriller, etc.)</summary>
        public LiteraryGenres Genres { get; set; }

        /// <summary>Optional detailed description or synopsis of the book</summary>
        public string? Description { get; set; }

        /// <summary>Optional URL to the book's cover image displayed in listings</summary>
        public string? CoverUrl { get; set; }

        /// <summary>Original publication date or date the book was written</summary>
        public DateTime WroteDate { get; set; }

        /// <summary>Timestamp when the book was added to the library in UTC</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>Navigation property - collection of chapters that make up this book</summary>
        public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
    }
}
