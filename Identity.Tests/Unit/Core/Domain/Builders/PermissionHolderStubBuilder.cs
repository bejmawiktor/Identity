using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Domain.TestDoubles;
using System;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Core.Domain.Builders
{
    internal class PermissionHolderStubBuilder
    {
        public static readonly Guid DefaultId = Guid.NewGuid();

        public static PermissionHolderStub DefaultPermissionHolderStub
            => new PermissionHolderStubBuilder().Build();

        public Guid Id { get; private set; } = PermissionHolderStubBuilder.DefaultId;
        public IEnumerable<PermissionId> Permissions { get; private set; } = new PermissionId[]
        {
            new PermissionId(new ResourceId("MyResource"), "AddSomething")
        };

        public PermissionHolderStubBuilder WithId(Guid id)
        {
            this.Id = id;

            return this;
        }

        public PermissionHolderStubBuilder WithPermissions(IEnumerable<PermissionId> permissions)
        {
            this.Permissions = permissions;

            return this;
        }

        public PermissionHolderStub Build()
            => new PermissionHolderStub(this.Id, this.Permissions);
    }
}