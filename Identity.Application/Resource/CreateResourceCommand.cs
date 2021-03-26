using DDD.Application.CQRS;
using System;

namespace Identity.Application
{
    public class CreateResourceCommand : ICommand
    {
        public string ResourceId { get; }
        public string ResourceDescription { get; }
        public Guid UserId { get; }

        public CreateResourceCommand(string resourceId, string resourceDescription, Guid userId)
        {
            this.ResourceId = resourceId;
            this.ResourceDescription = resourceDescription;
            this.UserId = userId;
        }
    }
}