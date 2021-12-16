using DDD.Domain.Events;
using System;

namespace Identity.Domain
{
    public class UserCreated : Event
    {
        public Guid UserId { get; }
        public string UserEmail { get; }

        public UserCreated(UserId userId, EmailAddress userEmail)
        {
            this.UserId = userId.ToGuid();
            this.UserEmail = userEmail.ToString();
        }
    }
}