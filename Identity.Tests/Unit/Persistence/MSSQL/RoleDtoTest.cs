using Identity.Persistence.MSSQL;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    [TestFixture]
    public class RoleDtoTest
    {
        [Test]
        public void TestConstructing_WhenApplictaionDtoGiven_ThenMembersAreSet()
        {
            var roleId = Guid.NewGuid();
            var roleDto = new RoleDto(
                new Identity.Application.RoleDto(
                    id: roleId,
                    name: "MyRole",
                    description: "My role description.",
                    permissions: new (string ResourceId, string Name)[]
                    {
                       ("MyResource", "MyPermission")
                    }));

            Assert.Multiple(() =>
            {
                Assert.That(roleDto.Id, Is.EqualTo(roleId));
                Assert.That(roleDto.Name, Is.EqualTo("MyRole"));
                Assert.That(roleDto.Description, Is.EqualTo("My role description."));
                Assert.That(roleDto.Permissions, Is.EqualTo(new RolePermissionDto[]
                {
                    new RolePermissionDto()
                    {
                        RoleId = roleId,
                        RoleDto = roleDto,
                        PermissionResourceId = "MyResource",
                        PermissionName = "MyPermission"
                    }
                }));
            });
        }

        [Test]
        public void TestToApplicationDto_WhenConvertingToApplicationDto_ThenRoleDtoIsReturned()
        {
            var roleId = Guid.NewGuid();
            var roleDto = new RoleDto
            {
                Id = roleId,
                Name = "MyRole",
                Description = "My role description.",
            };
            roleDto.Permissions = new RolePermissionDto[]
            {
                new RolePermissionDto()
                {
                    RoleId = roleId,
                    RoleDto = roleDto,
                    PermissionResourceId = "MyResource",
                    PermissionName = "MyPermission"
                }
            };

            Identity.Application.RoleDto appRoleDto = roleDto.ToApplicationDto();

            Assert.Multiple(() =>
            {
                Assert.That(appRoleDto.Id, Is.EqualTo(roleId));
                Assert.That(appRoleDto.Name, Is.EqualTo("MyRole"));
                Assert.That(appRoleDto.Description, Is.EqualTo("My role description."));
                Assert.That(appRoleDto.Permissions, Is.EqualTo(new (string ResourceId, string Name)[] 
                { 
                    ("MyResource", "MyPermission")
                }));
            });
        }
    }
}