using DDD.Domain.Model;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Domain
{
    public class AuthorizationCodeId : Identifier<(string Code, ApplicationId ApplicationId), AuthorizationCodeId>
    {
        public string Code => this.Value.Code;
        public ApplicationId ApplicationId => this.Value.ApplicationId;

        private static int CodeLength => 32;

        private static readonly char[] CodeCharacterSet
            = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        public AuthorizationCodeId(string code, ApplicationId applicationId) : base((code, applicationId))
        {
        }

        protected override void ValidateValue((string Code, ApplicationId ApplicationId) value)
        {
            if(value.Code == null)
            {
                throw new ArgumentNullException("code");
            }

            if(value.ApplicationId == null)
            {
                throw new ArgumentNullException("applicationId");
            }

            if(value.Code.Length == 0)
            {
                throw new ArgumentException("Code can't be empty.");
            }

            if(value.Code.Length != AuthorizationCodeId.CodeLength)
            {
                throw new ArgumentException("Invalid code given.");
            }
        }

        internal static AuthorizationCodeId Generate(ApplicationId applicationId)
        {
            byte[] data = new byte[4 * CodeLength];
            StringBuilder result = new StringBuilder(AuthorizationCodeId.CodeLength);

            using(var crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }

            for(int i = 0; i < AuthorizationCodeId.CodeLength; i++)
            {
                uint random = BitConverter.ToUInt32(data, i * 4);
                long index = random % AuthorizationCodeId.CodeCharacterSet.Length;

                result.Append(AuthorizationCodeId.CodeCharacterSet[index]);
            }

            return new AuthorizationCodeId(result.ToString(), applicationId);
        }
    }
}