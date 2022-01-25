using Identity.Core.Application;
using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Application.Builders;
using NUnit.Framework;

namespace Identity.Tests.Unit.Core.Application
{
    public class PermissionDtoTest
    {
        [Test]
        public void TestConstructor_WhenNameGiven_ThenIdIsSet()
        {
            PermissionDto permissionDto = new PermissionDtoBuilder()
                .WithName("MyPermission")
                .Build();

            Assert.That(permissionDto.Id.Name, Is.EqualTo("MyPermission"));
        }

        [Test]
        public void TestConstructor_WhenResourceIdGiven_ThenResourceIdIsSet()
        {
            PermissionDto permissionDto = new PermissionDtoBuilder()
                .WithResourceId("MyResource")
                .Build();

            Assert.That(permissionDto.Id.ResourceId, Is.EqualTo("MyResource"));
        }

        [Test]
        public void TestConstructor_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            PermissionDto permissionDto = new PermissionDtoBuilder()
                .WithDescription("My permission description.")
                .Build();

            Assert.That(permissionDto.Description, Is.EqualTo("My permission description."));
        }

        [Test]
        public void TestToPermission_WhenConvertingToPermission_ThenPermissionIsReturned()
        {
            PermissionDto permissionDto = PermissionDtoBuilder.DefaultPermissionDto;

            Permission permission = permissionDto.ToPermission();

            Assert.Multiple(() =>
            {
                Assert.That(permission.Id, Is.EqualTo(new PermissionId(
                    new ResourceId(permissionDto.Id.ResourceId), permissionDto.Id.Name)));
                Assert.That(permission.Description, Is.EqualTo(permissionDto.Description));
            });
        }

        [Test]
        public void TestEquals_WhenTwoIdentitcalPermissionsDtosGiven_ThenTrueIsReturned()
        {
            PermissionDto leftPermissionDto = PermissionDtoBuilder.DefaultPermissionDto;
            PermissionDto rightPermissionDto = PermissionDtoBuilder.DefaultPermissionDto;

            Assert.That(leftPermissionDto.Equals(rightPermissionDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentPermissionsDtosGiven_ThenFalseIsReturned()
        {
            PermissionDto leftPermissionDto = new PermissionDtoBuilder()
                .WithResourceId("MyResource")
                .WithName("MyPermission")
                .Build();
            PermissionDto rightPermissionDto = new PermissionDtoBuilder()
                .WithResourceId("MyResource2")
                .WithName("MyPermission2")
                .Build();

            Assert.That(leftPermissionDto.Equals(rightPermissionDto), Is.False);
        }

        [Test]
        public void TestGetHashCode_WhenTwoIdenticalPermissionsDtosGiven_ThenSameHashCodesIsReturned()
        {
            PermissionDto leftPermissionDto = PermissionDtoBuilder.DefaultPermissionDto;
            PermissionDto rightPermissionDto = PermissionDtoBuilder.DefaultPermissionDto;

            Assert.That(leftPermissionDto.GetHashCode(), Is.EqualTo(rightPermissionDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentPermissionsDtosGiven_ThenDifferentHashCodesIsReturned()
        {
            PermissionDto leftPermissionDto = new PermissionDtoBuilder()
                .WithResourceId("MyResource")
                .WithName("MyPermission")
                .Build();
            PermissionDto rightPermissionDto = new PermissionDtoBuilder()
                .WithResourceId("MyResource2")
                .WithName("MyPermission2")
                .Build();

            Assert.That(leftPermissionDto.GetHashCode(), Is.Not.EqualTo(rightPermissionDto.GetHashCode()));
        }
    }
}