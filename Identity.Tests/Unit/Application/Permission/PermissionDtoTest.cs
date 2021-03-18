using Identity.Application;
using Identity.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Application
{
    public class PermissionDtoTest
    {
        [Test]
        public void TestConstructing_WhenIdGiven_ThenIdIsSet()
        {
            var permissionDto = new PermissionDto("MyResource", "MyPermission", "My permission description.");

            Assert.That(permissionDto.Id.Name, Is.EqualTo("MyPermission"));
        }

        [Test]
        public void TestConstructing_WhenResourceIdGiven_ThenResourceIdIsSet()
        {
            var permissionDto = new PermissionDto("MyResource", "MyPermission", "My permission description.");

            Assert.That(permissionDto.Id.ResourceId, Is.EqualTo("MyResource"));
        }

        [Test]
        public void TestConstructing_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            var permissionDto = new PermissionDto("MyResource", "MyPermission", "My permission description.");

            Assert.That(permissionDto.Description, Is.EqualTo("My permission description."));
        }

        [Test]
        public void TestToPermission_WhenConvertingToPermission_ThenPermissionIsReturned()
        {
            var permissionDto = new PermissionDto("MyResource", "MyPermission", "My permission description.");

            Permission permission = permissionDto.ToPermission();

            Assert.Multiple(() =>
            {
                Assert.That(permission.Id, Is.EqualTo(new PermissionId(new ResourceId("MyResource"), "MyPermission")));
                Assert.That(permission.Description, Is.EqualTo("My permission description."));
            });
        }
    }
}