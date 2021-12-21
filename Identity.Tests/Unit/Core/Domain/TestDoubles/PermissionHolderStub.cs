using Identity.Core.Domain;
using System;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Core.Domain.TestDoubles
{
    internal class PermissionHolderStub : PermissionHolder<Guid>
    {
        public PermissionHolderStub(Guid id, IEnumerable<PermissionId> permissions = null)
        : base(id, permissions)
        {
        }
    }
}