namespace Folio.CORE.Exceptions
{
    public class ReadingProgressConflictException : Exception
    {
        public ReadingProgressConflictException(Guid readingProgressId) : base($"Error that readingProgress with id = {readingProgressId}, is already exist") { }
    }
}
