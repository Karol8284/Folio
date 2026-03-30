namespace Folio.CORE.Responses
{
    public class ChapterResponse<T>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public T? Content { get; set; }
    }
}  
