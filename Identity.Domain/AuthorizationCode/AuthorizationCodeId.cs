using DDD.Domain.Model;
using System;

namespace Identity.Domain
{
    public class AuthorizationCodeId : Identifier<(HashedCode Code, ApplicationId ApplicationId), AuthorizationCodeId>
    {
        public HashedCode Code => this.Value.Code;
        public ApplicationId ApplicationId => this.Value.ApplicationId;

        public AuthorizationCodeId(HashedCode code, ApplicationId applicationId) : base((code, applicationId))
        {
        }

        protected override void ValidateValue((HashedCode Code, ApplicationId ApplicationId) value)
        {
            if(value.Code == null)
            {
                throw new ArgumentNullException("code");
            }

            if(value.ApplicationId == null)
            {
                throw new ArgumentNullException("applicationId");
            }
        }

        internal static AuthorizationCodeId Generate(ApplicationId applicationId)
            => new AuthorizationCodeId(
                HashedCode.Hash(Domain.Code.Generate()), 
                applicationId);
    }
}