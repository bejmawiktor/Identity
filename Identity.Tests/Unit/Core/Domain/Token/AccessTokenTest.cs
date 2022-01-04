using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    using ApplicationId = Identity.Core.Domain.ApplicationId;

    [TestFixture]
    public class AccessTokenTest
    {
        [Test]
        public void TestConstructor_WhenRefreshTokenIdGiven_ThenInvalidOperationExceptionIsThrown()
        {
            PermissionId[] permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource"), "Add"),
                new PermissionId(new ResourceId("MyResource"), "Remove")
            };

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Refresh token id given."),
                () => new AccessToken(TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions)));
        }
    }
}