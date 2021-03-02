using DDD.Domain.Events;

namespace Identity.Domain
{
    public class UserCreatedEvent : Event
    {
        public UserId UserId { get; }
        public EmailAddress UserEmail { get; }

        public UserCreatedEvent(UserId userId, EmailAddress userEmail)
        {
            this.UserId = userId;
            this.UserEmail = userEmail;
        }
    }
}