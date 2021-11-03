using System;
using System.Runtime.Serialization;

namespace Identity.Domain
{
    public class AuthorizationCodeNotFoundException : Exception
    {
        public AuthorizationCodeNotFoundException() 
        : base("Authorization code not found.")
        {
        }

        public AuthorizationCodeNotFoundException(string message) : base(message)
        {
        }

        public AuthorizationCodeNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AuthorizationCodeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
