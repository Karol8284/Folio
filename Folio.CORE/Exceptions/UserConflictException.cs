using System;
using System.Collections.Generic;
using System.Text;

namespace Folio.CORE.Exceptions
{
    public class UserConflictException : Exception
     {
        public UserConflictException(Guid userId) : base($"Error that user with id = {userId}, is already exist") { }
    }
}
