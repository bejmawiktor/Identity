using DDD.Model;
using System;
using System.Collections.Generic;

namespace Identity.Domain
{
    public class Role : PermissionHolder<RoleId>, IAggregateRoot<RoleId>
    {
        private string name;
        private string description;

        public string Name
        {
            get => this.name;
            set
            {
                this.ValidateName(value);

                this.name = value;
            }
        }

        private void ValidateName(string name)
        {
            if(name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if(name.Length == 0)
            {
                throw new ArgumentException("Name can't be empty.");
            }
        }

        public string Description
        {
            get => this.description;
            set
            {
                this.ValidateDescription(value);

                this.description = value;
            }
        }

        private void ValidateDescription(string description)
        {
            if(description == null)
            {
                throw new ArgumentNullException(nameof(description));
            }

            if(description.Length == 0)
            {
                throw new ArgumentException("Description can't be empty.");
            }
        }

        public Role(
            RoleId id,
            string name,
            string description,
            IEnumerable<PermissionId> permissions = null)
        : base(id, permissions)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}