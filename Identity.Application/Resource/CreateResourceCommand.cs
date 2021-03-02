using DDD.Application.CQRS;

namespace Identity.Application
{
    public class CreateResourceCommand : ICommand
    {
        public string ResourceId { get; }
        public string ResourceDescription { get; }

        public CreateResourceCommand(string resourceId, string resourceDescription)
        {
            this.ResourceId = resourceId;
            this.ResourceDescription = resourceDescription;
        }
    }
}