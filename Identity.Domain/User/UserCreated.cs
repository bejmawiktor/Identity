using DDD.Domain.Events;

namespace Identity.Domain
{
    public class UserCreated : Event
    {
        public UserId UserId { get; }
        public EmailAddress UserEmail { get; }

        public UserCreated(UserId userId, EmailAddress userEmail)
        {
            this.UserId = userId;
            this.UserEmail = userEmail;
        }
    }
}