namespace Folio.CORE.Entities
{
    /// <summary>
    /// Chapter - represents a chapter or section within a book
    /// Contains reading content and tracks user reading progress
    /// </summary>
    public class Chapter
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
        public Book Book { get; set; }

        /// <summary>Navigation property - collection of reading progress records for this chapter by different users</summary>
        public ICollection<ReadingProgress> ReadingProgress { get; set; }
    }
}