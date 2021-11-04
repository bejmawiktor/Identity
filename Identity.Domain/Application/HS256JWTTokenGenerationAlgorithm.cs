using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Identity.Domain
{
    internal class HS256JWTTokenGenerationAlgorithm : ITokenGenerationAlgorithm
    {
        internal static readonly string SecretKey = "4U@2d&yDyWMqe&@k2%vB6p$SWR&qufg8";
        internal static readonly string Issuer = "Identity";
        internal static readonly string Audience = "Users";
        internal static readonly string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;

        private string TokenTypeClaimName => "tokenType";
        private string PermissionsClaimName => "permissions";

        public HS256JWTTokenGenerationAlgorithm()
        {
        }

        public string Encode(TokenInformation tokenInformation)
        {
            if(tokenInformation == null)
            {
                throw new ArgumentNullException(nameof(tokenInformation));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithm);
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, tokenInformation.ApplicationId.ToString()),
                new Claim(this.TokenTypeClaimName, tokenInformation.TokenType.Name),
                new Claim(this.PermissionsClaimName, this.ConvertPermissionsToPermissionsText(tokenInformation.Permissions))
            };
            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                notBefore: null,
                expires: tokenInformation.ExpirationDate,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string ConvertPermissionsToPermissionsText(IEnumerable<PermissionId> permissions)
        {
            var permissionsText = new StringBuilder();

            foreach (PermissionId permission in permissions)
            {
                permissionsText.Append(permission.ToString());
                permissionsText.Append(" ");
            }

            return permissionsText.ToString().TrimEnd(' ');
        }

        public TokenInformation Decode(string token)
        {
            if(!this.TryGetValidJwtSecurityToken(token, out JwtSecurityToken jwtSecurityToken))
            {
                throw new InvalidTokenException("Invalid token given.");
            }

            return new TokenInformation(
                applicationId: this.ExtractApplicationId(jwtSecurityToken),
                tokenType: this.ExtractTokenType(jwtSecurityToken),
                permissions: this.ExtractPermissions(jwtSecurityToken),
                expirationDate: this.ExtractExpirationDate(jwtSecurityToken));
        }

        private IEnumerable<PermissionId> ExtractPermissions(JwtSecurityToken jwtSecurityToken)
        {
            string permissionsText = this.ExtractPermissionsText(jwtSecurityToken);

            return this.ConvertPermissionsTextToPermissions(permissionsText);
        }

        private IEnumerable<PermissionId> ConvertPermissionsTextToPermissions(string permissionsText)
        {
            List<PermissionId> permissions = new List<PermissionId>();
            string[] splitedPermissionsText = permissionsText.Split(' ');

            if(splitedPermissionsText.Length == 0)
            {
                throw new ArgumentException("Wrong permissions given.");
            }

            foreach(var permission in splitedPermissionsText)
            {
                string[] permissionComponents = permission.Split('.');

                if(permissionComponents.Length != 2)
                {
                    throw new ArgumentException("Wrong permissions given.");
                }

                permissions.Add(new PermissionId(new ResourceId(permissionComponents[0]), permissionComponents[1]));
            }

            return permissions;
        }

        private string ExtractPermissionsText(JwtSecurityToken jwtSecurityToken)
        {
            return jwtSecurityToken.Claims
                .FirstOrDefault(x => x.Type == this.PermissionsClaimName)?
                .Value;
        }


        private bool TryGetValidJwtSecurityToken(string token, out JwtSecurityToken jwtSecurityToken)
        {
            if(!this.TryGetJwtSecurityToken(token, out jwtSecurityToken))
            {
                return false;
            }

            if(jwtSecurityToken.Issuer != Issuer)
            {
                return false;
            }

            if(jwtSecurityToken.Audiences.FirstOrDefault() != Audience)
            {
                return false;
            }

            if(!jwtSecurityToken.Claims.Any(j => j.Type == JwtRegisteredClaimNames.Exp))
            {
                return false;
            }

            if(!jwtSecurityToken.Claims.Any(j => j.Type == this.PermissionsClaimName))
            {
                return false;
            }

            try
            {
                ApplicationId applicationId = this.ExtractApplicationId(jwtSecurityToken);
            }
            catch
            {
                return false;
            }

            try
            {
                var tokenType = this.ExtractTokenType(jwtSecurityToken);
            }
            catch
            {
                return false;
            }

            try
            {
                var permissions = this.ExtractPermissions(jwtSecurityToken);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private bool TryGetJwtSecurityToken(string token, out JwtSecurityToken jwtSecurityToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = Encoding.ASCII.GetBytes(SecretKey);

            try
            {
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero,
                        ValidateLifetime = false
                    },
                    out SecurityToken validatedToken);

                jwtSecurityToken = (JwtSecurityToken)validatedToken;
            }
            catch
            {
                jwtSecurityToken = null;

                return false;
            }

            return true;
        }

        private ApplicationId ExtractApplicationId(JwtSecurityToken jwtSecurityToken)
        {
            string sub = jwtSecurityToken.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?
                .Value;

            return new ApplicationId(new Guid(sub));
        }

        private TokenType ExtractTokenType(JwtSecurityToken jwtSecurityToken)
        {
            return TokenType.FromName(
                name: this.ExtractTokenTypeName(jwtSecurityToken));
        }

        private string ExtractTokenTypeName(JwtSecurityToken jwtSecurityToken)
        {
            return jwtSecurityToken.Claims
                .FirstOrDefault(x => x.Type == this.TokenTypeClaimName)?
                .Value;
        }

        public void Validate(string token)
        {
            if(!this.CheckTokenIsValid(token))
            {
                throw new InvalidTokenException("Invalid token given.");
            }
        }

        private bool CheckTokenIsValid(string token)
        {
            return this.TryGetValidJwtSecurityToken(token, out _);
        }

        private DateTime ExtractExpirationDate(JwtSecurityToken jwtSecurityToken)
        {
            var expirationUnixTimeStamp = long.Parse(jwtSecurityToken.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)
                .Value);

            return this.UnixTimeStampToDateTime(expirationUnixTimeStamp);
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();

            return dateTime;
        }
    }
}