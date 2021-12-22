using Identity.Core.Application;
using Identity.Core.Domain;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Application
{
    public class PermissionDtoTest
    {
        [Test]
        public void TestConstructor_WhenNameGiven_ThenIdIsSet()
        {
            PermissionDto permissionDto = this.GetPermissionDto(name: "MyPermission");

            Assert.That(permissionDto.Id.Name, Is.EqualTo("MyPermission"));
        }

        private PermissionDto GetPermissionDto(
            string resourceId = null, 
            string name = null, 
            string description = null)
        {
            return new PermissionDto(
                resourceId ?? "MyResource", 
                name ?? "MyPermission", 
                description ?? "My permission description.");
        }

        [Test]
        public void TestConstructor_WhenResourceIdGiven_ThenResourceIdIsSet()
        {
            PermissionDto permissionDto = this.GetPermissionDto(resourceId: "MyResource");

            Assert.That(permissionDto.Id.ResourceId, Is.EqualTo("MyResource"));
        }

        [Test]
        public void TestConstructor_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            PermissionDto permissionDto = this.GetPermissionDto(description: "My permission description.");

            Assert.That(permissionDto.Description, Is.EqualTo("My permission description."));
        }

        [Test]
        public void TestToPermission_WhenConvertingToPermission_ThenPermissionIsReturned()
        {
            PermissionDto permissionDto = new PermissionDto("MyResource", "MyPermission", "My permission description.");

            Permission permission = permissionDto.ToPermission();

            Assert.Multiple(() =>
            {
                Assert.That(permission.Id, Is.EqualTo(new PermissionId(new ResourceId("MyResource"), "MyPermission")));
                Assert.That(permission.Description, Is.EqualTo("My permission description."));
            });
        }

        [Test]
        public void TestEquals_WhenTwoIdentitcalPermissionsDtosGiven_ThenTrueIsReturned()
        {
            PermissionDto leftPermissionDto = this.GetPermissionDto();
            PermissionDto rightPermissionDto = this.GetPermissionDto();

            Assert.That(leftPermissionDto.Equals(rightPermissionDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentPermissionsDtosGiven_ThenFalseIsReturned()
        {
            PermissionDto leftPermissionDto = this.GetPermissionDto("MyResource", "MyPermission");
            PermissionDto rightPermissionDto = this.GetPermissionDto("MyResource2", "MyPermission2");

            Assert.That(leftPermissionDto.Equals(rightPermissionDto), Is.False);
        }

        [Test]
        public void TestGetHashCode_WhenTwoIdenticalPermissionsDtosGiven_ThenSameHashCodesIsReturned()
        {
            PermissionDto leftPermissionDto = this.GetPermissionDto();
            PermissionDto rightPermissionDto = this.GetPermissionDto();

            Assert.That(leftPermissionDto.GetHashCode(), Is.EqualTo(rightPermissionDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentPermissionsDtosGiven_ThenDifferentHashCodesIsReturned()
        {
            PermissionDto leftPermissionDto = this.GetPermissionDto("MyResource", "MyPermission");
            PermissionDto rightPermissionDto = this.GetPermissionDto("MyResource2", "MyPermission2");

            Assert.That(leftPermissionDto.GetHashCode(), Is.Not.EqualTo(rightPermissionDto.GetHashCode()));
        }
    }
}