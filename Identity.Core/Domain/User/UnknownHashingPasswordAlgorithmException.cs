using System;

namespace Identity.Core.Domain
{
    public class UnknownHashingPasswordAlgorithmException : Exception
    {
        public UnknownHashingPasswordAlgorithmException() : base()
        {
        }

        public UnknownHashingPasswordAlgorithmException(string message) : base(message)
        {
        }

        public UnknownHashingPasswordAlgorithmException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}