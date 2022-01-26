using Identity.Core.Domain;
using Identity.Core.Events;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    internal class PermissionCreatedBuilder
    {
        public PermissionId PermissionId { get; private set; } = new(new ResourceId("TestResource"), "MyPermission");
        public string PermissionDescription { get; private set; } = "Test permission description";

        public PermissionCreatedBuilder WithPermissionId(PermissionId permissionId)
        {
            this.PermissionId = permissionId;

            return this;
        }

        public PermissionCreatedBuilder WithPermissionDescription(string permissionDescription)
        {
            this.PermissionDescription = permissionDescription;

            return this;
        }

        public PermissionCreated Build()
            => new PermissionCreated(
                this.PermissionId,
                this.PermissionDescription);
    }
}