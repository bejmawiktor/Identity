using System;
using System.Runtime.Serialization;

namespace Identity.Core.Domain
{
    public class UnknownTokenValueEncodingAlgorithmException : InvalidTokenException
    {
        public UnknownTokenValueEncodingAlgorithmException() : base()
        {
        }

        public UnknownTokenValueEncodingAlgorithmException(string message) : base(message)
        {
        }

        public UnknownTokenValueEncodingAlgorithmException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}