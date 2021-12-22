using Identity.Core.Application;
using Moq;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Application
{
    [TestFixture]
    public class RolesRepositoryAdapterTest
    {
        [Test]
        public void TestConstructor_WhenNullRolesRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("rolesRepository"),
               () => new RolesRepositoryAdapter(null));
        }

        [Test]
        public void TestConstructor_WhenRolesRepositoryGiven_ThenRolesRepositoryIsSet()
        {
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            var rolesRepositoryAdapter = new RolesRepositoryAdapter(rolesRepository);

            Assert.That(rolesRepositoryAdapter.RolesRepository, Is.EqualTo(rolesRepository));
        }
    }
}