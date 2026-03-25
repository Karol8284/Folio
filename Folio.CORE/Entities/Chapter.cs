namespace Folio.CORE.Entities
{
    public class Chapter
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public int OrderIndex { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int TotalWords { get; set; }
        public Book Book { get; set; }
        public ICollection<ReadingProgress> ReadingProgress { get; set; }
    }
}