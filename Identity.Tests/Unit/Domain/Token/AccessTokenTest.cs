﻿using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    using ApplicationId = Identity.Domain.ApplicationId;

    [TestFixture]
    public class AccessTokenTest
    {
        [Test]
        public void TestConstructor_WhenRefreshTokenIdGiven_ThenInvalidOperationExceptionIsThrown()
        {
            var permissions = new PermissionId[]
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