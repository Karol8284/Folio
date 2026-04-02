namespace Folio.CORE.Exceptions
{
    /// <summary>
    /// ChapterConflictException - thrown when a chapter operation violates constraints or conflicts with existing data
    /// Typical cause: attempting to create a chapter with duplicate content or invalid ordering
    /// </summary>
    public class ChapterConflictException : Exception
    {
        /// <summary>
        /// Initializes a new instance of ChapterConflictException
        /// </summary>
        /// <param name="chapterId">The chapter identifier involved in the conflict</param>
        public ChapterConflictException(Guid chapterId) : base($"Error that user with id = {chapterId}, is already exist") { }
    }
}
