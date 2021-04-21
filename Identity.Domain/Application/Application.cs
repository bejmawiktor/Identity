using DDD.Domain.Model;
using System;

namespace Identity.Domain
{
    public class Application : AggregateRoot<ApplicationId>
    {
        public UserId UserId { get; }
        public string Name { get; }
        public Url HomepageUrl { get; }
        public Url CallbackUrl { get; }

        public Application(
            ApplicationId id,
            UserId userId,
            string name,
            Url homepageUrl,
            Url callbackUrl)
        : base(id)
        {
            this.ValidateMembers(userId, name, homepageUrl, callbackUrl);

            this.Name = name;
            this.UserId = userId;
            this.HomepageUrl = homepageUrl;
            this.CallbackUrl = callbackUrl;
        }

        private void ValidateMembers(
            UserId userId,
            string name,
            Url homepageUrl,
            Url callbackUrl)
        {
            if(name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if(name == string.Empty)
            {
                throw new ArgumentException("Name can't be empty.");
            }

            if(userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if(homepageUrl == null)
            {
                throw new ArgumentNullException(nameof(homepageUrl));
            }

            if(callbackUrl == null)
            {
                throw new ArgumentNullException(nameof(callbackUrl));
            }
        }
    }
}