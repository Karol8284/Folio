using System;
using System.Collections.Generic;
using System.Text;

namespace Folio.CORE.Exceptions
{
     public class BookConflictException : Exception
    {
        public BookConflictException(Guid bookId) : base($"Error that book with id = {bookId}, is already exist") { }
    }
}
