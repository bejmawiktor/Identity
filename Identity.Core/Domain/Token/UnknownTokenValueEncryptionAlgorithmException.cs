using System;

namespace Identity.Core.Domain
{
    public class UnknownTokenValueEncryptionAlgorithmException : Exception
    {
        public UnknownTokenValueEncryptionAlgorithmException() : base()
        {
        }

        public UnknownTokenValueEncryptionAlgorithmException(string message) : base(message)
        {
        }

        public UnknownTokenValueEncryptionAlgorithmException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}