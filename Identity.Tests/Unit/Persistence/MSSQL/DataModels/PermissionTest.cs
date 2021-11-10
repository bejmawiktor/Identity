using Identity.Application;
using Identity.Persistence.MSSQL.DataModels;
using NUnit.Framework;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    public class PermissionTest
    {
        [Test]
        public void TestConstructor_WhenDtoGiven_ThenMembersAreSet()
        {
            var permission = new Permission(
                new PermissionDto("MyResource", "MyPermission", "My permission description."));

            Assert.Multiple(() =>
            {
                Assert.That(permission.ResourceId, Is.EqualTo("MyResource"));
                Assert.That(permission.Name, Is.EqualTo("MyPermission"));
                Assert.That(permission.Description, Is.EqualTo("My permission description."));
            });
        }

        [Test]
        public void TestSetFields_WhenDtoGiven_ThenMembersAreSet()
        {
            var permission = new Permission();

            permission.SetFields(new PermissionDto("MyResource", "MyPermission", "My permission description."));

            Assert.Multiple(() =>
            {
                Assert.That(permission.ResourceId, Is.EqualTo("MyResource"));
                Assert.That(permission.Name, Is.EqualTo("MyPermission"));
                Assert.That(permission.Description, Is.EqualTo("My permission description."));
            });
        }

        [Test]
        public void TestToDto_WhenConvertingToDto_ThenPermissionDtoIsReturned()
        {
            var permission = new Permission()
            {
                ResourceId = "MyResource",
                Name = "MyPermission",
                Description = "My permission description."
            };

            PermissionDto permissionDto = permission.ToDto();

            Assert.Multiple(() =>
            {
                Assert.That(permissionDto.Id, Is.EqualTo(("MyResource", "MyPermission")));
                Assert.That(permissionDto.Description, Is.EqualTo("My permission description."));
            });
        }
    }
}