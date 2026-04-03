using Folio.CORE.Entities;

namespace Folio.Shared.DTOs.Chapter
{
    public class ChapterDto
    {
        /// <summary>Unique identifier for the chapter (primary key)</summary>
        public Guid Id { get; set; }

        /// <summary>Foreign key reference to the parent book (required)</summary>
        public Guid BookId { get; set; }

        /// <summary>Sequential order/position of this chapter within the book (starting from 0)</summary>
        public int OrderIndex { get; set; }

        /// <summary>Title or heading of the chapter</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>Full text content of the chapter for reading</summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>Total word count in this chapter; used for progress tracking and statistics</summary>
        public int TotalWords { get; set; }

        /// <summary>Navigation property - reference to the parent book</summary>
        public Folio.CORE.Entities.Book Book { get; set; }
    }
}
