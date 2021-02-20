using Identity.Domain;
using System;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Domain.TestDoubles
{
    public class PermissionHolderStub : PermissionHolder<Guid>
    {
        public PermissionHolderStub(Guid id, IEnumerable<PermissionId> permissions = null)
        : base(id, permissions)
        {
        }
    }
}