using System;

namespace Identity.Core.Domain
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException() : base()
        {
        }

        internal ResourceNotFoundException(ResourceId resourceId)
        : base($"Resource {resourceId} not found.")
        {
        }

        public ResourceNotFoundException(string message) : base(message)
        {
        }

        public ResourceNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}