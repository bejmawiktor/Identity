using Identity.Core.Domain;
using Identity.Core.Events;
using Identity.Tests.Unit.Core.Domain.Builders;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class PermissionCreatedTest
    {
        [Test]
        public void TestConstructor_WhenPermissionIdGiven_ThenPermissionIdIsSet()
        {
            PermissionId permissionId = new(new ResourceId("TestResource2"), "MyPermission2");
            PermissionCreated permissionCreated = new PermissionCreatedBuilder()
                .WithPermissionId(permissionId)
                .Build();

            Assert.That(permissionCreated.PermissionId, Is.EqualTo(permissionId.ToString()));
        }

        [Test]
        public void TestConstructor_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            PermissionCreated permissionCreated = new PermissionCreatedBuilder()
                .WithPermissionDescription("Test permission description 2")
                .Build();

            Assert.That(permissionCreated.PermissionDescription, Is.EqualTo("Test permission description 2"));
        }
    }
}