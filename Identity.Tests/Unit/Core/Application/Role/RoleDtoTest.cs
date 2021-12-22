using Identity.Core.Application;
using Identity.Core.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Tests.Unit.Core.Application
{
    [TestFixture]
    public class RoleDtoTest
    {
        [Test]
        public void TestConstructor_WhenIdGiven_ThenIdIsSet()
        {
            Guid id = Guid.NewGuid();
            RoleDto roleDto = this.GetRoleDto(id: id);

            Assert.That(roleDto.Id, Is.EqualTo(id));
        }

        private RoleDto GetRoleDto(
            Guid? id = null, 
            string name = null, 
            string description = null,
            IEnumerable<(string ResourceId, string Name)> permissions = null)
        {
            return new RoleDto(
                id: id ?? Guid.NewGuid(),
                name: name ?? "MyRole",
                description: description ?? "My role description",
                permissions: permissions ?? new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission"),
                    ("MyResource2", "MyPermission2")
                });
        }

        [Test]
        public void TestConstructor_WhenNameGiven_ThenNameIsSet()
        {
            RoleDto roleDto = this.GetRoleDto(name: "MyRole");

            Assert.That(roleDto.Name, Is.EqualTo("MyRole"));
        }

        [Test]
        public void TestConstructor_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            RoleDto roleDto = this.GetRoleDto(description: "My role description");

            Assert.That(roleDto.Description, Is.EqualTo("My role description"));
        }

        [Test]
        public void TestConstructor_WhenPermissionsGiven_ThenPermissionsAreSet()
        {
            var permissions = new (string ResourceId, string Name)[]
            {
                ("MyResource", "MyPermission")
            };
            RoleDto roleDto = this.GetRoleDto(permissions: permissions);

            Assert.That(roleDto.Permissions, Is.EquivalentTo(permissions));
        }

        [Test]
        public void TestConstructor_WhenNullPermissionsGiven_ThenEmptyPermissionsIsSet()
        {
            Guid id = Guid.NewGuid();
            RoleDto roleDto = new RoleDto(
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
            RoleDto roleDto = new RoleDto(
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
            RoleDto leftRoleDto = this.GetRoleDto(id: id);
            RoleDto rightRoleDto = this.GetRoleDto(id: id);

            Assert.That(leftRoleDto.Equals(rightRoleDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentRolesDtosGiven_ThenFalseIsReturned()
        {
            Guid id = Guid.NewGuid();
            RoleDto leftRoleDto = this.GetRoleDto(
                id: id,
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission"),
                    ("MyResource2", "MyPermission2")
                });
            RoleDto rightRoleDto = this.GetRoleDto(
                id: id,
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
            RoleDto leftRoleDto = this.GetRoleDto(id: id);
            RoleDto rightRoleDto = this.GetRoleDto(id: id);

            Assert.That(leftRoleDto.GetHashCode(), Is.EqualTo(rightRoleDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentRolesDtosGiven_ThenDifferentHashCodesAreReturned()
        {
            Guid id = Guid.NewGuid();
            RoleDto leftRoleDto = this.GetRoleDto(
                id: id,
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission"),
                    ("MyResource2", "MyPermission2")
                });
            RoleDto rightRoleDto = this.GetRoleDto(
                id: id,
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission"),
                });

            Assert.That(leftRoleDto.GetHashCode(), Is.Not.EqualTo(rightRoleDto.GetHashCode()));
        }
    }
}