using DDD.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Core.Domain
{
    internal abstract class PermissionHolder<TIdentifier> : Entity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        protected List<PermissionId> permissions;

        public IReadOnlyCollection<PermissionId> Permissions
            => this.permissions.AsReadOnly();

        protected PermissionHolder(TIdentifier id, IEnumerable<PermissionId> permissions = null) : base(id)
        {
            this.permissions = permissions?.ToList() ?? new List<PermissionId>();
        }

        public virtual bool IsPermittedTo(PermissionId permissionId)
        {
            if(permissionId == null)
            {
                throw new ArgumentNullException(nameof(permissionId));
            }

            return this.Permissions.Contains(permissionId);
        }

        public virtual void ObtainPermission(PermissionId permissionId)
        {
            if(permissionId == null)
            {
                throw new ArgumentNullException(nameof(permissionId));
            }

            if(this.IsPermittedTo(permissionId))
            {
                throw new InvalidOperationException("Permission was already obtained.");
            }

            this.permissions.Add(permissionId);
        }

        public virtual void RevokePermission(PermissionId permissionId)
        {
            if(permissionId == null)
            {
                throw new ArgumentNullException(nameof(permissionId));
            }

            if(!this.IsPermittedTo(permissionId))
            {
                throw new InvalidOperationException("Permission wasn't obtained.");
            }

            this.permissions.Remove(permissionId);
        }
    }
}