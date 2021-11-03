﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Domain
{
    public class AuthorizationService
    {
        public IUsersRepository UsersRepository { get; }
        public IRolesRepository RolesRepository { get; }
        public IApplicationsRepository ApplicationsRepository { get; }
        public IAuthorizationCodesRepository AuthorizationCodesRepository { get; }

        public AuthorizationService(
            IUsersRepository usersRepository,
            IRolesRepository rolesRepository,
            IApplicationsRepository applicationsRepository,
            IAuthorizationCodesRepository authorizationCodesRepository)
        {
            this.UsersRepository = usersRepository
                ?? throw new ArgumentNullException(nameof(usersRepository));
            this.RolesRepository = rolesRepository
                ?? throw new ArgumentNullException(nameof(rolesRepository));
            this.ApplicationsRepository = applicationsRepository
                ?? throw new ArgumentNullException(nameof(applicationsRepository));
            this.AuthorizationCodesRepository = authorizationCodesRepository
                ?? throw new ArgumentNullException(nameof(authorizationCodesRepository));
        }

        public async Task<bool> CheckUserAccess(UserId userId, PermissionId permissionId)
        {
            this.ValidateCheckUserParameters(userId, permissionId);

            User user = await this.UsersRepository.GetAsync(userId);

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
                Role role = await this.RolesRepository.GetAsync(roleId);

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

            Application application = await this.ApplicationsRepository.GetAsync(applicationId);

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

            Code code = Code.Generate();

            AuthorizationCode authorizationCode = application.CreateAuthorizationCode(code, permissions);

            await this.AuthorizationCodesRepository.AddAsync(authorizationCode);

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
            User user = await this.UsersRepository.GetAsync(userId);

            if(user == null)
            {
                throw new UserNotFoundException(userId);
            }

            var userPermissions = new List<PermissionId>();
            userPermissions.AddRange(user.Permissions);

            foreach(var roleId in user.Roles)
            {
                Role role = await this.RolesRepository.GetAsync(roleId);

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

            Application application = await this.ApplicationsRepository.GetAsync(applicationId);

            if (application == null)
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

            var authorizationCodeId = new AuthorizationCodeId(HashedCode.Hash(code), applicationId);
            AuthorizationCode authorizationCode = await this.AuthorizationCodesRepository.GetAsync(authorizationCodeId);
            
            if(authorizationCode == null)
            {
                throw new AuthorizationCodeNotFoundException();
            }

            authorizationCode.Use();

            TokenPair tokens = application.GenerateTokens();

            await this.AuthorizationCodesRepository.UpdateAsync(authorizationCode);

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
    }
}