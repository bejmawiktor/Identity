using Identity.Core.Application;
using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Application
{
    public class PermissionDtoConverterTest
    {
        [Test]
        public void TestToDto_WhenPermissionGiven_ThenPermissionDtoIsReturned()
        {
            var permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            var permission = new Permission(
                id: permissionId,
                description: "My permission description");
            var permissionDtoConverter = new PermissionDtoConverter();

            PermissionDto permissionDto = permissionDtoConverter.ToDto(permission);

            Assert.That(permissionDto.Id, Is.EqualTo((permissionId.ResourceId.ToString(), permissionId.Name)));
            Assert.That(permissionDto.Description, Is.EqualTo("My permission description"));
        }

        [Test]
        public void TestToDto_WhenNullPermissionGiven_ThenArgumentNullExceptionIsThrown()
        {
            var permissionDtoConverter = new PermissionDtoConverter();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("permission"),
                () => permissionDtoConverter.ToDto(null));
        }

        [Test]
        public void TestToDtoIdentifier_WhenPermissionIdGiven_ThenDtoIdentifierIsReturned()
        {
            var permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            var permissionDtoConverter = new PermissionDtoConverter();

            (string resourceId, string name) = permissionDtoConverter.ToDtoIdentifier(permissionId);

            Assert.That(resourceId, Is.EqualTo(permissionId.ResourceId.ToString()));
            Assert.That(name, Is.EqualTo(permissionId.Name));
        }

        [Test]
        public void TestToDtoIdentifier_WhenNullPermissionIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            var permissionDtoConverter = new PermissionDtoConverter();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("permissionId"),
                () => permissionDtoConverter.ToDtoIdentifier(null));
        }
    }
}