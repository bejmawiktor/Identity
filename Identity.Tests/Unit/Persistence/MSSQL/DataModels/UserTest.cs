﻿using Identity.Application;
using Identity.Domain;
using Identity.Persistence.MSSQL.DataModels;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    using User = Identity.Persistence.MSSQL.DataModels.User;

    [TestFixture]
    public class UserTest
    {
        [Test]
        public void TestConstructing_WhenDtoGiven_ThenMembersAreSet()
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
            var userDto = new Identity.Application.UserDto(
                userId,
                "example@example.com",
                hashedPassword.ToString(),
                roles,
                permissions);

            var user = new User(
                userDto);

            Assert.Multiple(() =>
            {
                Assert.That(user.Id, Is.EqualTo(userDto.Id));
                Assert.That(user.Email, Is.EqualTo(userDto.Email));
                Assert.That(user.HashedPassword, Is.EqualTo(userDto.HashedPassword));
                Assert.That(user.Roles, Is.EqualTo(new UserRole[]
                {
                    new UserRole()
                    {
                        UserId = userId,
                        User = user,
                        RoleId = roles[0]
                    },
                    new UserRole()
                    {
                        UserId = userId,
                        User = user,
                        RoleId = roles[1]
                    }
                }));
                Assert.That(user.Permissions, Is.EqualTo(new UserPermission[]
                {
                    new UserPermission()
                    {
                        UserId = userId,
                        User = user,
                        PermissionResourceId = "MyResource",
                        PermissionName = "MyPermission"
                    },
                    new UserPermission()
                    {
                        UserId = userId,
                        User = user,
                        PermissionResourceId = "MyResource2",
                        PermissionName = "MyPermission2"
                    }
                }));
            });
        }

        [Test]
        public void TestToDto_WhenConvertingToDto_ThenUserDtoIsReturned()
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
            var user = new User()
            {
                Id = userId,
                Email = "example@example.com",
                HashedPassword = hashedPassword.ToString(),
            };
            user.Permissions = new UserPermission[]
            {
                new UserPermission()
                {
                    UserId = user.Id,
                    User = user,
                    PermissionName = "MyPermission",
                    PermissionResourceId = "MyResource"
                },
                new UserPermission()
                {
                    UserId = user.Id,
                    User = user,
                    PermissionName = "MyPermission2",
                    PermissionResourceId = "MyResource2"
                },
            };
            user.Roles = new UserRole[]
            {
                new UserRole()
                {
                    UserId = user.Id,
                    User = user,
                    RoleId = roles[0]
                },
                new UserRole()
                {
                    UserId = user.Id,
                    User = user,
                    RoleId = roles[1]
                },
            };

            UserDto userDto = user.ToDto();

            Assert.Multiple(() =>
            {
                Assert.That(userDto.Id, Is.EqualTo(userId));
                Assert.That(userDto.Email, Is.EqualTo(userDto.Email));
                Assert.That(userDto.HashedPassword, Is.EqualTo(userDto.HashedPassword));
                Assert.That(userDto.Roles, Is.EqualTo(roles));
                Assert.That(userDto.Permissions, Is.EqualTo(permissions));
            });
        }
    }
}