using System;
using System.Runtime.Serialization;

namespace Identity.Core.Domain
{
    [Serializable]
    public class UnknownSecretKeyEncryptionAlgorithmException : Exception
    {
        public UnknownSecretKeyEncryptionAlgorithmException()
        {
        }

        public UnknownSecretKeyEncryptionAlgorithmException(string message) : base(message)
        {
        }

        public UnknownSecretKeyEncryptionAlgorithmException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownSecretKeyEncryptionAlgorithmException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}