﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Core.Domain
{
    internal class AuthorizationService
    {
        public IUnitOfWork UnitOfWork { get; }

        public AuthorizationService(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> CheckUserAccess(UserId userId, PermissionId permissionId)
        {
            this.ValidateCheckUserParameters(userId, permissionId);

            User user = await this.UnitOfWork.UsersRepository.GetAsync(userId);

            if(user == null)
            {
                throw new UserNotFoundException(userId);
            }

            if(user.IsPermittedTo(permissionId))
            {
                return true;
            }

            foreach(var roleId in user.Roles)
            {
                Role role = await this.UnitOfWork.RolesRepository.GetAsync(roleId);

                if(role.IsPermittedTo(permissionId))
                {
                    return true;
                }
            }

            return false;
        }

        private void ValidateCheckUserParameters(UserId userId, PermissionId permissionId)
        {
            if(userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if(permissionId == null)
            {
                throw new ArgumentNullException(nameof(permissionId));
            }
        }

        public async Task<Code> GenerateAuthorizationCode(
            ApplicationId applicationId,
            Url callbackUrl,
            IEnumerable<PermissionId> permissions)
        {
            this.ValidateGenerateAuthorizationCodeParameters(applicationId, callbackUrl, permissions);

            Application application = await this.UnitOfWork.ApplicationsRepository.GetAsync(applicationId);

            if(application == null)
            {
                throw new ApplicationNotFoundException(applicationId);
            }

            if(application.CallbackUrl != callbackUrl)
            {
                throw new ArgumentException("Wrong callback url given.");
            }

            if(!await this.ComparePermissions(application.UserId, permissions))
            {
                throw new ArgumentException("One or more permissions are incorrect for given application.");
            }

            AuthorizationCode authorizationCode = application.CreateAuthorizationCode(permissions, out Code code);

            await this.UnitOfWork.AuthorizationCodesRepository.AddAsync(authorizationCode);

            return code;
        }

        private void ValidateGenerateAuthorizationCodeParameters(
            ApplicationId applicationId,
            Url callbackUrl,
            IEnumerable<PermissionId> permissions)
        {
            if(applicationId == null)
            {
                throw new ArgumentNullException(nameof(applicationId));
            }

            if(callbackUrl == null)
            {
                throw new ArgumentNullException(nameof(callbackUrl));
            }

            if(permissions == null)
            {
                throw new ArgumentNullException(nameof(permissions));
            }

            if(!permissions.Any())
            {
                throw new ArgumentException("Permissions can't be empty.");
            }
        }

        private async Task<bool> ComparePermissions(UserId userId, IEnumerable<PermissionId> requestedPermissions)
        {
            User user = await this.UnitOfWork.UsersRepository.GetAsync(userId);

            if(user == null)
            {
                throw new UserNotFoundException(userId);
            }

            var userPermissions = new List<PermissionId>();
            userPermissions.AddRange(user.Permissions);

            foreach(var roleId in user.Roles)
            {
                Role role = await this.UnitOfWork.RolesRepository.GetAsync(roleId);

                userPermissions.AddRange(role.Permissions);
            }

            if(!requestedPermissions.All(r => userPermissions.Contains(r)))
            {
                return false;
            }

            return true;
        }

        public async Task<TokenPair> GenerateTokens(
            ApplicationId applicationId,
            SecretKey secretKey,
            Url callbackUrl,
            Code code)
        {
            this.ValidateGenerateTokensParameters(applicationId, secretKey, callbackUrl, code);
            TokenPair tokens = null;

            using(var transactionScope = this.UnitOfWork.BeginScope())
            {
                Application application = await this.UnitOfWork.ApplicationsRepository.GetAsync(applicationId);

                this.ValidateGenerateTokensApplication(applicationId, secretKey, callbackUrl, application);

                var authorizationCodeId = new AuthorizationCodeId(HashedCode.Hash(code), applicationId);
                AuthorizationCode authorizationCode = await this.UnitOfWork.AuthorizationCodesRepository.GetAsync(authorizationCodeId);

                if(authorizationCode == null)
                {
                    throw new AuthorizationCodeNotFoundException();
                }

                authorizationCode.Use();

                AccessToken accessToken = application.CreateAccessToken(authorizationCode.Permissions);
                RefreshToken refreshToken = application.CreateRefreshToken(authorizationCode.Permissions);

                tokens = new TokenPair(accessToken.Id.Decrypt(), refreshToken.Id.Decrypt());

                await this.UnitOfWork.AuthorizationCodesRepository.UpdateAsync(authorizationCode);
                await this.UnitOfWork.RefreshTokensRepository.AddAsync(refreshToken);

                transactionScope.Complete();
            }

            return tokens;
        }

        private void ValidateGenerateTokensParameters(
            ApplicationId applicationId,
            SecretKey secretKey,
            Url callbackUrl,
            Code code)
        {
            if(applicationId == null)
            {
                throw new ArgumentNullException(nameof(applicationId));
            }

            if(code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            if(secretKey == null)
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            if(callbackUrl == null)
            {
                throw new ArgumentNullException(nameof(callbackUrl));
            }
        }

        private void ValidateGenerateTokensApplication(
            ApplicationId applicationId,
            SecretKey secretKey,
            Url callbackUrl,
            Application application)
        {
            if(application == null)
            {
                throw new ApplicationNotFoundException(applicationId);
            }

            if(application.SecretKey.Decrypt() != secretKey)
            {
                throw new ArgumentException("Wrong secret key given.");
            }

            if(application.CallbackUrl != callbackUrl)
            {
                throw new ArgumentException("Wrong callback url given.");
            }
        }

        public async Task<TokenPair> RefreshTokens(TokenValue refreshTokenValue, Url callbackUrl)
        {
            this.ValidateRefreshTokensParamters(refreshTokenValue, callbackUrl);

            TokenPair tokens = null;

            using(var transactionScope = this.UnitOfWork.BeginScope())
            {
                Application application = await this.UnitOfWork.ApplicationsRepository
                    .GetAsync(refreshTokenValue.ApplicationId);

                this.ValidateRefreshTokensApplication(refreshTokenValue, callbackUrl, application);

                var tokenId = new TokenId(TokenValueEncrypter.Encrypt(refreshTokenValue));
                RefreshToken refreshToken = await this.UnitOfWork.RefreshTokensRepository.GetAsync(tokenId);

                this.ValidateRefreshTokensRefreshToken(tokenId, refreshToken);

                AccessToken accessToken = application.RefreshAccessToken(refreshToken);
                RefreshToken newRefreshToken = application.RefreshRefreshToken(refreshToken);

                tokens = new TokenPair(accessToken.Id.Decrypt(), newRefreshToken.Id.Decrypt());

                refreshToken.Use();

                await this.UnitOfWork.RefreshTokensRepository.UpdateAsync(refreshToken);
                await this.UnitOfWork.RefreshTokensRepository.AddAsync(newRefreshToken);

                transactionScope.Complete();
            }

            return tokens;
        }

        private void ValidateRefreshTokensParamters(TokenValue refreshTokenValue, Url callbackUrl)
        {
            if(refreshTokenValue == null)
            {
                throw new ArgumentNullException(nameof(refreshTokenValue));
            }

            if(callbackUrl == null)
            {
                throw new ArgumentNullException(nameof(callbackUrl));
            }
        }

        private void ValidateRefreshTokensApplication(TokenValue refreshTokenValue, Url callbackUrl, Application application)
        {
            if(application == null)
            {
                throw new ApplicationNotFoundException(refreshTokenValue.ApplicationId);
            }

            if(application.CallbackUrl != callbackUrl)
            {
                throw new ArgumentException("Wrong callback url given.");
            }
        }

        private void ValidateRefreshTokensRefreshToken(TokenId tokenId, RefreshToken refreshToken)
        {
            if(refreshToken == null)
            {
                throw new RefreshTokenNotFoundException(tokenId);
            }

            TokenVerificationResult tokenVerificationResult = refreshToken.Verify();

            if(tokenVerificationResult == TokenVerificationResult.Failed)
            {
                throw new InvalidTokenException(tokenVerificationResult.Message);
            }
        }
    }
}