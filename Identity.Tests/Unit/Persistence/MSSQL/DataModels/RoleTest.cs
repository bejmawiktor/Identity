using Identity.Core.Application;
using Identity.Persistence.MSSQL.DataModels;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    [TestFixture]
    public class RoleTest
    {
        [Test]
        public void TestConstructor_WhenDtoGiven_ThenMembersAreSet()
        {
            Guid roleId = Guid.NewGuid();
            Role role = new(
                new RoleDto(
                    id: roleId,
                    name: "MyRole",
                    description: "My role description.",
                    permissions: new (string ResourceId, string Name)[]
                    {
                       ("MyResource", "MyPermission")
                    }));

            Assert.Multiple(() =>
            {
                Assert.That(role.Id, Is.EqualTo(roleId));
                Assert.That(role.Name, Is.EqualTo("MyRole"));
                Assert.That(role.Description, Is.EqualTo("My role description."));
                Assert.That(role.Permissions, Is.EqualTo(new RolePermission[]
                {
                    new RolePermission()
                    {
                        RoleId = roleId,
                        Role = role,
                        PermissionResourceId = "MyResource",
                        PermissionName = "MyPermission"
                    }
                }));
            });
        }

        [Test]
        public void TestSetFields_WhenDtoGiven_ThenMembersAreSet()
        {
            Guid roleId = Guid.NewGuid();
            Role role = new();

            role.SetFields(
                new RoleDto(
                    id: roleId,
                    name: "MyRole",
                    description: "My role description.",
                    permissions: new (string ResourceId, string Name)[]
                    {
                       ("MyResource", "MyPermission")
                    }));

            Assert.Multiple(() =>
            {
                Assert.That(role.Id, Is.EqualTo(roleId));
                Assert.That(role.Name, Is.EqualTo("MyRole"));
                Assert.That(role.Description, Is.EqualTo("My role description."));
                Assert.That(role.Permissions, Is.EqualTo(new RolePermission[]
                {
                    new RolePermission()
                    {
                        RoleId = roleId,
                        Role = role,
                        PermissionResourceId = "MyResource",
                        PermissionName = "MyPermission"
                    }
                }));
            });
        }

        [Test]
        public void TestToDto_WhenConvertingToDto_ThenRoleDtoIsReturned()
        {
            Guid roleId = Guid.NewGuid();
            Role role = new()
            {
                Id = roleId,
                Name = "MyRole",
                Description = "My role description.",
            };
            role.Permissions = new RolePermission[]
            {
                new RolePermission()
                {
                    RoleId = roleId,
                    Role = role,
                    PermissionResourceId = "MyResource",
                    PermissionName = "MyPermission"
                }
            };

            RoleDto roleDto = role.ToDto();

            Assert.Multiple(() =>
            {
                Assert.That(roleDto.Id, Is.EqualTo(roleId));
                Assert.That(roleDto.Name, Is.EqualTo("MyRole"));
                Assert.That(roleDto.Description, Is.EqualTo("My role description."));
                Assert.That(roleDto.Permissions, Is.EqualTo(new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                }));
            });
        }
    }
}