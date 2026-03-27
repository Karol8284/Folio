namespace Folio.CORE.Exceptions
{
    public class ChapterConflictException : Exception
    {
        public ChapterConflictException(Guid chapterId) : base($"Error that user with id = {chapterId}, is already exist") { }
    }
}
