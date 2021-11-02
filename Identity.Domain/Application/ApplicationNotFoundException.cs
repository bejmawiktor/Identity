using System;

namespace Identity.Domain
{
    public class ApplicationNotFoundException : Exception
    {
        public ApplicationNotFoundException() : base()
        {
        }

        public ApplicationNotFoundException(ApplicationId applicationId)
        : base($"Application {applicationId} not found.")
        {
        }

        public ApplicationNotFoundException(string message) : base(message)
        {
        }

        public ApplicationNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
