using NUnit.Framework;
using Identity.Application;
using Identity.Domain;
using System;
using System.Linq;

namespace Identity.Tests.Unit.Application
{
    [TestFixture]
    public class RoleDtoTest
    {
        [Test]
        public void TestConstructing_WhenIdGiven_ThenIdIsSet()
        {
            var id = Guid.NewGuid();
            var roleDto = new RoleDto(
                id: id,
                name: "MyRole",
                description: "My role description");

            Assert.That(roleDto.Id, Is.EqualTo(id));
        }

        [Test]
        public void TestConstructing_WhenNameGiven_ThenNameIsSet()
        {
            var id = Guid.NewGuid();
            var roleDto = new RoleDto(
                id: id,
                name: "MyRole",
                description: "My role description");

            Assert.That(roleDto.Name, Is.EqualTo("MyRole"));
        }

        [Test]
        public void TestConstructing_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            var id = Guid.NewGuid();
            var roleDto = new RoleDto(
                id: id,
                name: "MyRole",
                description: "My role description");

            Assert.That(roleDto.Description, Is.EqualTo("My role description"));
        }

        [Test]
        public void TestConstructing_WhenPermissionsGiven_ThenPermissionsIsSet()
        {
            var id = Guid.NewGuid();
            var roleDto = new RoleDto(
                id: id,
                name: "MyRole",
                description: "My role description",
                permissions: new(string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                });

            Assert.That(roleDto.Permissions, Is.EquivalentTo(new (string ResourceId, string Name)[]
            {
                ("MyResource", "MyPermission")
            }));
        }

        [Test]
        public void TestConstructing_WhenNullPermissionsGiven_ThenEmptyPermissionsIsSet()
        {
            var id = Guid.NewGuid();
            var roleDto = new RoleDto(
                id: id,
                name: "MyRole",
                description: "My role description",
                permissions: null);

            Assert.That(roleDto.Permissions, Is.EquivalentTo(Enumerable.Empty<(string ResourceId, string Name)>()));
        }

        [Test]
        public void TestToRole_WhenConvertingToRole_ThenRoleIsReturned()
        {
            var id = Guid.NewGuid();
            var roleDto = new RoleDto(
                id: id,
                name: "MyRole",
                description: "My role description",
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission"),
                    ("MyResource2", "MyPermission2")
                });

            Role role = roleDto.ToRole();

            Assert.Multiple(() =>
            {
                Assert.That(role.Id, Is.EqualTo(new RoleId(id)));
                Assert.That(role.Name, Is.EqualTo("MyRole"));
                Assert.That(role.Description, Is.EqualTo("My role description"));
                Assert.That(role.Permissions, Is.EquivalentTo(new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource"), "MyPermission"),
                    new PermissionId(new ResourceId("MyResource2"), "MyPermission2")
                }));
            });
        }
    }
}