using DDD.Domain.Events;
using Identity.Core.Domain;
using System;

namespace Identity.Core.Events
{
    public class UserCreated : Event
    {
        public Guid UserId { get; }
        public string UserEmail { get; }

        internal UserCreated(UserId userId, EmailAddress userEmail)
        {
            this.UserId = userId.ToGuid();
            this.UserEmail = userEmail.ToString();
        }
    }
}