using Identity.Application;
using Identity.Domain;
using NUnit.Framework;
using System;
using System.Linq;

namespace Identity.Tests.Unit.Application
{
    [TestFixture]
    public class RoleDtoTest
    {
        [Test]
        public void TestConstructor_WhenIdGiven_ThenIdIsSet()
        {
            Guid id = Guid.NewGuid();
            var roleDto = new RoleDto(
                id: id,
                name: "MyRole",
                description: "My role description");

            Assert.That(roleDto.Id, Is.EqualTo(id));
        }

        [Test]
        public void TestConstructor_WhenNameGiven_ThenNameIsSet()
        {
            Guid id = Guid.NewGuid();
            var roleDto = new RoleDto(
                id: id,
                name: "MyRole",
                description: "My role description");

            Assert.That(roleDto.Name, Is.EqualTo("MyRole"));
        }

        [Test]
        public void TestConstructor_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            Guid id = Guid.NewGuid();
            var roleDto = new RoleDto(
                id: id,
                name: "MyRole",
                description: "My role description");

            Assert.That(roleDto.Description, Is.EqualTo("My role description"));
        }

        [Test]
        public void TestConstructor_WhenPermissionsGiven_ThenPermissionsAreSet()
        {
            Guid id = Guid.NewGuid();
            var roleDto = new RoleDto(
                id: id,
                name: "MyRole",
                description: "My role description",
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                });

            Assert.That(roleDto.Permissions, Is.EquivalentTo(new (string ResourceId, string Name)[]
            {
                ("MyResource", "MyPermission")
            }));
        }

        [Test]
        public void TestConstructor_WhenNullPermissionsGiven_ThenEmptyPermissionsIsSet()
        {
            Guid id = Guid.NewGuid();
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
            Guid id = Guid.NewGuid();
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

        [Test]
        public void TestEquals_WhenTwoIdentitcalRolesDtosGiven_ThenTrueIsReturned()
        {
            Guid id = Guid.NewGuid();
            var leftRoleDto = new RoleDto(
                id: id,
                name: "MyRole",
                description: "My role description",
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission"),
                    ("MyResource2", "MyPermission2")
                });
            var rightRoleDto = new RoleDto(
                id: id,
                name: "MyRole",
                description: "My role description",
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission"),
                    ("MyResource2", "MyPermission2")
                });

            Assert.That(leftRoleDto.Equals(rightRoleDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentRolesDtosGiven_ThenFalseIsReturned()
        {
            Guid id = Guid.NewGuid();
            var leftRoleDto = new RoleDto(
                id: id,
                name: "MyRole",
                description: "My role description",
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission"),
                    ("MyResource2", "MyPermission2")
                });
            var rightRoleDto = new RoleDto(
                id: id,
                name: "MyRole2",
                description: "My role description",
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission"),
                });

            Assert.That(leftRoleDto.Equals(rightRoleDto), Is.False);
        }

        [Test]
        public void TestGetHashCode_WhenTwoIdenticalRolesDtosGiven_ThenSameHashCodesAreReturned()
        {
            Guid id = Guid.NewGuid();
            var leftRoleDto = new RoleDto(
                id: id,
                name: "MyRole",
                description: "My role description",
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission"),
                    ("MyResource2", "MyPermission2")
                });
            var rightRoleDto = new RoleDto(
                id: id,
                name: "MyRole",
                description: "My role description",
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission"),
                    ("MyResource2", "MyPermission2")
                });

            Assert.That(leftRoleDto.GetHashCode(), Is.EqualTo(rightRoleDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentRolesDtosGiven_ThenDifferentHashCodesAreReturned()
        {
            Guid id = Guid.NewGuid();
            var leftRoleDto = new RoleDto(
                id: id,
                name: "MyRole",
                description: "My role description",
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission"),
                    ("MyResource2", "MyPermission2")
                });
            var rightRoleDto = new RoleDto(
                id: id,
                name: "MyRole2",
                description: "My role description",
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission"),
                });

            Assert.That(leftRoleDto.GetHashCode(), Is.Not.EqualTo(rightRoleDto.GetHashCode()));
        }
    }
}