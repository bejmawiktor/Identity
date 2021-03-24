using Identity.Persistence.MSSQL;
using NUnit.Framework;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    public class PermissionDtoTest
    {
        [Test]
        public void TestConstructing_WhenApplictaionDtoGiven_ThenMembersAreSet()
        {
            var permissionDto = new PermissionDto(
                new Identity.Application.PermissionDto("MyResource", "MyPermission", "My permission description."));

            Assert.Multiple(() =>
            {
                Assert.That(permissionDto.ResourceId, Is.EqualTo("MyResource"));
                Assert.That(permissionDto.Name, Is.EqualTo("MyPermission"));
                Assert.That(permissionDto.Description, Is.EqualTo("My permission description."));
            });
        }

        [Test]
        public void TestToApplicationDto_WhenConvertingToApplicationDto_ThenPermissionDtoIsReturned()
        {
            var permissionDto = new PermissionDto
            {
                ResourceId = "MyResource",
                Name = "MyPermission",
                Description = "My permission description."
            };

            Identity.Application.PermissionDto appPermissionDto = permissionDto.ToApplicationDto();

            Assert.Multiple(() =>
            {
                Assert.That(appPermissionDto.Id, Is.EqualTo(("MyResource", "MyPermission")));
                Assert.That(appPermissionDto.Description, Is.EqualTo("My permission description."));
            });
        }
    }
}