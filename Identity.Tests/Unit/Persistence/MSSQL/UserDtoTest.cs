using Identity.Domain;
using Identity.Persistence.MSSQL;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    using UserDto = Identity.Persistence.MSSQL.UserDto;

    [TestFixture]
    public class UserDtoTest
    {
        [Test]
        public void TestConstructing_WhenApplictaionDtoGiven_ThenMembersAreSet()
        {
            var roles = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var permissions = new (string ResourceId, string Name)[]
            {
                ("MyResource", "MyPermission"),
                ("MyResource2", "MyPermission2")
            };
            var userId = Guid.NewGuid();
            var hashedPassword = HashedPassword.Hash("MyPassword");
            var appUserDto = new Identity.Application.UserDto(
                userId,
                "example@example.com",
                hashedPassword.ToString(),
                roles,
                permissions);

            var userDto = new UserDto(
                appUserDto);

            Assert.Multiple(() =>
            {
                Assert.That(userDto.Id, Is.EqualTo(appUserDto.Id));
                Assert.That(userDto.Email, Is.EqualTo(appUserDto.Email));
                Assert.That(userDto.HashedPassword, Is.EqualTo(appUserDto.HashedPassword));
                Assert.That(userDto.Roles, Is.EqualTo(new UserRoleDto[]
                {
                    new UserRoleDto()
                    {
                        UserId = userId,
                        UserDto = userDto,
                        RoleId = roles[0]
                    },
                    new UserRoleDto()
                    {
                        UserId = userId,
                        UserDto = userDto,
                        RoleId = roles[1]
                    }
                }));
                Assert.That(userDto.Permissions, Is.EqualTo(new UserPermissionDto[]
                {
                    new UserPermissionDto()
                    {
                        UserId = userId,
                        UserDto = userDto,
                        PermissionResourceId = "MyResource",
                        PermissionName = "MyPermission"
                    },
                    new UserPermissionDto()
                    {
                        UserId = userId,
                        UserDto = userDto,
                        PermissionResourceId = "MyResource2",
                        PermissionName = "MyPermission2"
                    }
                }));
            });
        }

        [Test]
        public void TestToApplicationDto_WhenConvertingToApplicationDto_ThenUserDtoIsReturned()
        {
            var roles = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var permissions = new (string ResourceId, string Name)[]
            {
                ("MyResource", "MyPermission"),
                ("MyResource2", "MyPermission2")
            };
            var userId = Guid.NewGuid();
            var hashedPassword = HashedPassword.Hash("MyPassword");
            var userDto = new UserDto()
            {
                Id = userId,
                Email = "example@example.com",
                HashedPassword = hashedPassword.ToString(),
                
            };
            userDto.Permissions = new UserPermissionDto[]
            {
                new UserPermissionDto()
                {
                    UserId = userDto.Id,
                    UserDto = userDto,
                    PermissionName = "MyPermission",
                    PermissionResourceId = "MyResource"
                },
                new UserPermissionDto()
                {
                    UserId = userDto.Id,
                    UserDto = userDto,
                    PermissionName = "MyPermission2",
                    PermissionResourceId = "MyResource2"
                },
            };
            userDto.Roles = new UserRoleDto[]
            {
                new UserRoleDto()
                {
                    UserId = userDto.Id,
                    UserDto = userDto,
                    RoleId = roles[0]
                },
                new UserRoleDto()
                {
                    UserId = userDto.Id,
                    UserDto = userDto,
                    RoleId = roles[1]
                },
            };

            Identity.Application.UserDto appUserDto = userDto.ToApplicationDto();

            Assert.Multiple(() =>
            {
                Assert.That(appUserDto.Id, Is.EqualTo(userId));
                Assert.That(appUserDto.Email, Is.EqualTo(appUserDto.Email));
                Assert.That(appUserDto.HashedPassword, Is.EqualTo(appUserDto.HashedPassword));
                Assert.That(appUserDto.Roles, Is.EqualTo(roles));
                Assert.That(appUserDto.Permissions, Is.EqualTo(permissions));
            });
        }
    }
}