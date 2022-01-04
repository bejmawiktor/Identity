using Identity.Core.Application;
using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Application
{
    [TestFixture]
    public class RoleDtoConverterTest
    {
        [Test]
        public void TestToDto_WhenRoleGiven_ThenRoleDtoIsReturned()
        {
            HashedPassword password = HashedPassword.Hash(new Password("MyPassword"));
            RoleId roleId = new(Guid.NewGuid());
            Role role = new(
                id: roleId,
                name: "MyRole",
                description: "My role description",
                permissions: new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource"), "MyPermission"),
                    new PermissionId(new ResourceId("MyResource2"), "MyPermission2")
                });
            RoleDtoConverter roleDtoConverter = new();

            RoleDto roleDto = roleDtoConverter.ToDto(role);

            Assert.Multiple(() =>
            {
                Assert.That(roleDto.Id, Is.EqualTo(roleId.ToGuid()));
                Assert.That(roleDto.Name, Is.EqualTo(role.Name));
                Assert.That(roleDto.Description, Is.EqualTo(role.Description));
                Assert.That(roleDto.Permissions, Is.EquivalentTo(new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission"),
                    ("MyResource2", "MyPermission2")
                }));
            });
        }

        [Test]
        public void TestToDto_WhenNullRoleGiven_ThenArgumentNullExceptionIsThrown()
        {
            RoleDtoConverter roleDtoConverter = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("role"),
                () => roleDtoConverter.ToDto(null));
        }

        [Test]
        public void TestToDtoIdentifier_WhenRoleIdGiven_ThenDtoIdentifierIsReturned()
        {
            RoleId roleId = new(Guid.NewGuid());
            RoleDtoConverter roleDtoConverter = new();

            Guid roleDtoIdentifier = roleDtoConverter.ToDtoIdentifier(roleId);

            Assert.That(roleDtoIdentifier, Is.EqualTo(roleId.ToGuid()));
        }

        [Test]
        public void TestToDtoIdentifier_WhenNullRoleIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            RoleDtoConverter roleDtoConverter = new();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("roleId"),
                () => roleDtoConverter.ToDtoIdentifier(null));
        }
    }
}