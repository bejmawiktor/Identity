﻿using Identity.Application;
using Moq;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Application
{
    [TestFixture]
    public class RolesRepositoryAdapterTest
    {
        [Test]
        public void TestConstructing_WhenNullRolesRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("rolesRepository"),
               () => new RolesRepositoryAdapter(null));
        }

        [Test]
        public void TestConstructing_WhenRolesRepositoryGiven_ThenRolesRepositoryIsSet()
        {
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            var rolesRepositoryAdapter = new RolesRepositoryAdapter(rolesRepository);

            Assert.That(rolesRepositoryAdapter.RolesRepository, Is.EqualTo(rolesRepository));
        }
    }
}