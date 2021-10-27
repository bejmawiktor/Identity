using Identity.Application;
using Identity.Persistence.MSSQL.DataModels;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    [TestFixture]
    public class RoleTest
    {
        [Test]
        public void TestConstructing_WhenDtoGiven_ThenMembersAreSet()
        {
            var roleId = Guid.NewGuid();
            var role = new Role(
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
            var roleId = Guid.NewGuid();
            var role = new Role();

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
            var roleId = Guid.NewGuid();
            var role = new Role
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