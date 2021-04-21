using DDD.Application.Model;
using Identity.Domain;
using System;

namespace Identity.Application
{
    using Application = Identity.Domain.Application;
    using ApplicationId = Identity.Domain.ApplicationId;

    public class ApplicationDto : IAggregateRootDto<Application, ApplicationId>
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Name { get; }
        public string HomepageUrl { get; }
        public string CallbackUrl { get; }

        public ApplicationDto(
            Guid id,
            Guid userId,
            string name,
            string homepageUrl,
            string callbackUrl)
        {
            this.Id = id;
            this.UserId = userId;
            this.Name = name;
            this.HomepageUrl = homepageUrl;
            this.CallbackUrl = callbackUrl;
        }

        public Application ToApplication()
            => new Application(
                id: new ApplicationId(this.Id),
                userId: new UserId(this.UserId),
                name: this.Name,
                homepageUrl: new Url(this.HomepageUrl),
                callbackUrl: new Url(this.CallbackUrl));

        Application IDomainObjectDto<Application>.ToDomainObject()
            => this.ToApplication();

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id, this.UserId, this.Name, this.HomepageUrl, this.CallbackUrl);
        }

        public override bool Equals(object obj)
        {
            return obj is ApplicationDto dto 
                && this.Id.Equals(dto.Id) 
                && this.UserId.Equals(dto.UserId) 
                && this.Name == dto.Name 
                && this.HomepageUrl == dto.HomepageUrl 
                && this.CallbackUrl == dto.CallbackUrl;
        }
    }
}