using System;
using System.Collections.Generic;
using System.Text;

namespace Folio.CORE.Exceptions
{
    /// <summary>
    /// UserConflictException - thrown when a user operation violates constraints or conflicts with existing data
    /// Typical cause: attempting to create a user with an email that already exists
    /// </summary>
    public class UserConflictException : Exception
    {
        /// <summary>
        /// Initializes a new instance of UserConflictException
        /// </summary>
        /// <param name="userId">The user identifier involved in the conflict</param>
        public UserConflictException(Guid userId) : base($"Error that user with id = {userId}, is already exist") { }
    }
}
