using System;

namespace Identity.Core.Domain
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base()
        {
        }

        internal UserNotFoundException(UserId userId)
        : base($"User {userId} not found.")
        {
        }

        public UserNotFoundException(string message) : base(message)
        {
        }

        public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}