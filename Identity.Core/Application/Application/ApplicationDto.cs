﻿using DDD.Application.Model;
using Identity.Core.Domain;
using System;

namespace Identity.Core.Application
{
    using Application = Identity.Core.Domain.Application;
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    public class ApplicationDto : IAggregateRootDto<Application, ApplicationId>
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Name { get; }
        public string SecretKey { get; }
        public string HomepageUrl { get; }
        public string CallbackUrl { get; }

        public ApplicationDto(
            Guid id,
            Guid userId,
            string name,
            string secretKey,
            string homepageUrl,
            string callbackUrl)
        {
            this.Id = id;
            this.UserId = userId;
            this.Name = name;
            this.SecretKey = secretKey;
            this.HomepageUrl = homepageUrl;
            this.CallbackUrl = callbackUrl;
        }

        internal Application ToApplication()
            => new Application(
                id: new ApplicationId(this.Id),
                userId: new UserId(this.UserId),
                name: this.Name,
                secretKey: new EncryptedSecretKey(this.SecretKey),
                homepageUrl: new Url(this.HomepageUrl),
                callbackUrl: new Url(this.CallbackUrl));

        Application IDomainObjectDto<Application>.ToDomainObject()
            => this.ToApplication();

        public override int GetHashCode()
        {
            return HashCode.Combine(
                this.Id,
                this.UserId,
                this.Name,
                this.SecretKey,
                this.HomepageUrl,
                this.CallbackUrl);
        }

        public override bool Equals(object obj)
        {
            return obj is ApplicationDto dto
                && this.Id.Equals(dto.Id)
                && this.UserId.Equals(dto.UserId)
                && this.Name == dto.Name
                && this.SecretKey == dto.SecretKey
                && this.HomepageUrl == dto.HomepageUrl
                && this.CallbackUrl == dto.CallbackUrl;
        }
    }
}