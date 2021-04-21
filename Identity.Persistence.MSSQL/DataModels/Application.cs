using Identity.Application;
using System;

namespace Identity.Persistence.MSSQL.DataModels
{
    internal record Application
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string HomepageUrl { get; set; }
        public string CallbackUrl { get; set; }
        public User User { get; set; }

        public Application(ApplicationDto applicationDto)
        {
            this.Id = applicationDto.Id;
            this.UserId = applicationDto.UserId;
            this.Name = applicationDto.Name;
            this.HomepageUrl = applicationDto.HomepageUrl;
            this.CallbackUrl = applicationDto.CallbackUrl;
        }

        public Application()
        {
        }

        public ApplicationDto ToDto()
            => new ApplicationDto(
                id: this.Id,
                userId: this.UserId,
                name: this.Name,
                homepageUrl: this.HomepageUrl,
                callbackUrl: this.CallbackUrl);
    }
}