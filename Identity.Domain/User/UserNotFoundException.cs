using System;

namespace Identity.Domain
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base()
        {
        }

        public UserNotFoundException(UserId userId)
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