namespace Folio.CORE.Exceptions
{
    /// <summary>
    /// ReadingProgressConflictException - thrown when a reading progress operation violates constraints or conflicts with existing data
    /// Typical cause: attempting to create duplicate progress records for the same user-chapter combination
    /// </summary>
    public class ReadingProgressConflictException : Exception
    {
        /// <summary>
        /// Initializes a new instance of ReadingProgressConflictException
        /// </summary>
        /// <param name="readingProgressId">The reading progress identifier involved in the conflict</param>
        public ReadingProgressConflictException(Guid readingProgressId) : base($"Error that readingProgress with id = {readingProgressId}, is already exist") { }
    }
}
