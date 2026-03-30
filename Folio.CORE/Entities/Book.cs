using Folio.CORE.Enums;

namespace Folio.CORE.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public LiteraryGenres Genres { get; set; }
        public string? Description { get; set; }
        public string? CoverUrl { get; set; }
        public DateTime WroteDate { get; set; }
        public DateTime CreatedAt { get; set; }

        // Relacja - Book zawiera wiele Chapters
        public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
    }
}
