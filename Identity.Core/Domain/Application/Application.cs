﻿using DDD.Domain.Model;
using System;
using System.Collections.Generic;

namespace Identity.Core.Domain
{
    internal class Application : AggregateRoot<ApplicationId>
    {
        public UserId UserId { get; }
        public string Name { get; }
        public EncryptedSecretKey SecretKey { get; private set; }
        public Url HomepageUrl { get; }
        public Url CallbackUrl { get; }

        public Application(
            ApplicationId id,
            UserId userId,
            string name,
            EncryptedSecretKey secretKey,
            Url homepageUrl,
            Url callbackUrl)
        : base(id)
        {
            this.ValidateMembers(userId, name, secretKey, homepageUrl, callbackUrl);

            this.UserId = userId;
            this.Name = name;
            this.SecretKey = secretKey;
            this.HomepageUrl = homepageUrl;
            this.CallbackUrl = callbackUrl;
        }

        private void ValidateMembers(
            UserId userId,
            string name,
            EncryptedSecretKey secretKey,
            Url homepageUrl,
            Url callbackUrl)
        {
            if(userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if(name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if(name == string.Empty)
            {
                throw new ArgumentException("Name can't be empty.");
            }

            if(secretKey == null)
            {
                throw new ArgumentNullException(nameof(secretKey));
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

        public SecretKey DecryptSecretKey()
            => this.SecretKey.Decrypt();

        public void RegenerateSecretKey()
        {
            this.SecretKey = EncryptedSecretKey.Encrypt(Domain.SecretKey.Generate());
        }

        internal AuthorizationCode CreateAuthorizationCode(IEnumerable<PermissionId> permissions, out Code code)
            => AuthorizationCode.Create(this.Id, permissions, out code);

        internal AccessToken CreateAccessToken(IEnumerable<PermissionId> permissions)
        {
            return new AccessToken(TokenId.GenerateAccessTokenId(this.Id, permissions));
        }

        internal RefreshToken CreateRefreshToken(IEnumerable<PermissionId> permissions)
        {
            return new RefreshToken(TokenId.GenerateRefreshTokenId(this.Id, permissions));
        }

        internal AccessToken RefreshAccessToken(RefreshToken refreshToken)
        {
            if(refreshToken.ApplicationId != this.Id)
            {
                throw new InvalidTokenException("Wrong refresh token given.");
            }

            TokenVerificationResult tokenVerificationResult = refreshToken.Verify();

            if(tokenVerificationResult == TokenVerificationResult.Failed)
            {
                throw new InvalidTokenException(tokenVerificationResult.Message);
            }

            return new AccessToken(
                TokenId.GenerateAccessTokenId(this.Id, refreshToken.Permissions));
        }

        internal RefreshToken RefreshRefreshToken(RefreshToken refreshToken)
        {
            if(refreshToken.ApplicationId != this.Id)
            {
                throw new InvalidTokenException("Wrong refresh token given.");
            }

            TokenVerificationResult tokenVerificationResult = refreshToken.Verify();

            if(tokenVerificationResult == TokenVerificationResult.Failed)
            {
                throw new InvalidTokenException(tokenVerificationResult.Message);
            }

            return new RefreshToken(
                TokenId.GenerateRefreshTokenId(this.Id, refreshToken.Permissions, refreshToken.ExpiresAt));
        }
    }
}