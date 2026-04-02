using System;
using System.Collections.Generic;
using System.Text;

namespace Folio.CORE.Exceptions
{
    /// <summary>
    /// BookConflictException - thrown when a book operation violates constraints or conflicts with existing data
    /// Typical cause: attempting to create a book with a title that already exists
    /// </summary>
    public class BookConflictException : Exception
    {
        /// <summary>
        /// Initializes a new instance of BookConflictException
        /// </summary>
        /// <param name="bookId">The book identifier involved in the conflict</param>
        public BookConflictException(Guid bookId) : base($"Error that book with id = {bookId}, is already exist") { }
    }
}
