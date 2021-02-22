using DDD.Events;
using System;

namespace Identity.Domain
{
    public class RoleCreatedEvent : Event
    {
        public RoleId RoleId { get; }
        public string Name { get; }
        public string Description { get; }

        public RoleCreatedEvent(
            RoleId roleId,
            string name,
            string description)
        {
            this.RoleId = roleId;
            this.Name = name;
            this.Description = description;
        }
    }
}