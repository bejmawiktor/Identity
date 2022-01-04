﻿using Identity.Core.Application;
using Moq;
using NUnit.Framework;
using System;
using IPermissionsRepository = Identity.Core.Application.IPermissionsRepository;

namespace Identity.Tests.Unit.Core.Application
{
    [TestFixture]
    public class PermissionsRepositoryAdapterTest
    {
        [Test]
        public void TestConstructor_WhenNullPermissionsRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("permissionsRepository"),
               () => new PermissionsRepositoryAdapter(null));
        }

        [Test]
        public void TestConstructor_WhenPermissionsRepositoryGiven_ThenPermissionsRepositoryIsSet()
        {
            Mock<IPermissionsRepository> permissionsRepositoryMock = new();
            IPermissionsRepository permissionsRepository = permissionsRepositoryMock.Object;
            PermissionsRepositoryAdapter permissionsRepositoryAdapter = new(permissionsRepository);

            Assert.That(permissionsRepositoryAdapter.PermissionsRepository, Is.EqualTo(permissionsRepository));
        }
    }
}