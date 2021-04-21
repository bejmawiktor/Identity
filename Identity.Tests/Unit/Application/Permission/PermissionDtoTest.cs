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

        [Test]
        public void TestEquals_WhenTwoIdentitcalPermissionsDtosGiven_ThenTrueIsReturned()
        {
            var leftPermissionDto = new PermissionDto("MyResource", "MyPermission", "My permission description.");
            var rightPermissionDto = new PermissionDto("MyResource", "MyPermission", "My permission description.");

            Assert.That(leftPermissionDto.Equals(rightPermissionDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentPermissionsDtosGiven_ThenFalseIsReturned()
        {
            var leftPermissionDto = new PermissionDto("MyResource", "MyPermission", "My permission description.");
            var rightPermissionDto = new PermissionDto("MyResource2", "MyPermission2", "My permission description.");

            Assert.That(leftPermissionDto.Equals(rightPermissionDto), Is.False);
        }

        [Test]
        public void TestGetHashCode_WhenTwoIdenticalPermissionsDtosGiven_ThenSameHashCodesIsReturned()
        {
            var leftPermissionDto = new PermissionDto("MyResource", "MyPermission", "My permission description.");
            var rightPermissionDto = new PermissionDto("MyResource", "MyPermission", "My permission description.");

            Assert.That(leftPermissionDto.GetHashCode(), Is.EqualTo(rightPermissionDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentPermissionsDtosGiven_ThenDifferentHashCodesIsReturned()
        {
            var leftPermissionDto = new PermissionDto("MyResource", "MyPermission", "My permission description.");
            var rightPermissionDto = new PermissionDto("MyResource2", "MyPermission2", "My permission description.");

            Assert.That(leftPermissionDto.GetHashCode(), Is.Not.EqualTo(rightPermissionDto.GetHashCode()));
        }
    }
}