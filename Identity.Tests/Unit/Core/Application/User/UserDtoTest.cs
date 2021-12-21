using Identity.Core.Application;
using Identity.Core.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Identity.Tests.Unit.Application
{
    [TestFixture]
    public class UserDtoTest
    {
        private static readonly HashedPassword TestPassword = HashedPassword.Hash(new Password("MyPassword"));

        [Test]
        public void TestConstructor_WhenIdGiven_ThenIdIsSet()
        {
            Guid userId = Guid.NewGuid();
            UserDto userDto = this.GetUserDto(userId);

            Assert.That(userDto.Id, Is.EqualTo(userId));
        }

        private UserDto GetUserDto(
            Guid? id = null,
            string email = null,
            string password = null,
            IEnumerable<Guid> roles = null,
            IEnumerable<(string ResourceId, string Name)> permissions = null)
        {
            return new UserDto(
                id ?? Guid.NewGuid(),
                email ?? "example@example.com",
                password ?? UserDtoTest.TestPassword.ToString(),
                roles ?? new Guid[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid()
                },
                permissions ?? new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission"),
                    ("MyResource2", "MyPermission2")
                });
        }

        [Test]
        public void TestConstructor_WhenEmailGiven_ThenEmailIsSet()
        {
            UserDto userDto = this.GetUserDto(email: "example@example.com");

            Assert.That(userDto.Email, Is.EqualTo("example@example.com"));
        }

        [Test]
        public void TestConstructor_WhenHashedPasswordGiven_ThenHashedPasswordIsSet()
        {
            UserDto userDto = this.GetUserDto(password: UserDtoTest.TestPassword.ToString());

            Assert.That(userDto.HashedPassword, Is.EqualTo(UserDtoTest.TestPassword.ToString()));
        }

        [Test]
        public void TestConstructor_WhenRolesGiven_ThenRolesAreSet()
        {
            var roles = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            UserDto userDto = this.GetUserDto(roles: roles);

            Assert.That(userDto.Roles, Is.EqualTo(roles));
        }

        [Test]
        public void TestConstructor_WhenNullRolesGiven_ThenEmptyRolesAreSet()
        {
            Guid userId = Guid.NewGuid();
            UserDto userDto = new UserDto(userId, "example@example.com", UserDtoTest.TestPassword.ToString(), null);

            Assert.That(userDto.Roles, Is.Empty);
        }

        [Test]
        public void TestConstructor_WhenPermissionsGiven_ThenPermissionsAreSet()
        {
            var permissions = new (string ResourceId, string Name)[]
            {
                ("MyResource", "MyPermission"),
                ("MyResource2", "MyPermission2")
            };

            UserDto userDto = this.GetUserDto(permissions: permissions);

            Assert.That(userDto.Permissions, Is.EqualTo(permissions));
        }

        [Test]
        public void TestConstructor_WhenNullPermissionsGiven_ThenPermissionsAreSet()
        {
            var userId = Guid.NewGuid();
            UserDto userDto = new UserDto(userId, "example@example.com", UserDtoTest.TestPassword.ToString(), null, null);

            Assert.That(userDto.Permissions, Is.Empty);
        }

        [Test]
        public void TestToUser_WhenConvertingToUser_ThenUserIsReturned()
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
            UserDto userDto = new UserDto(userId, "example@example.com", UserDtoTest.TestPassword.ToString(), roles, permissions);

            User user = userDto.ToUser();

            Assert.Multiple(() =>
            {
                Assert.That(user.Id, Is.EqualTo(new UserId(userId)));
                Assert.That(user.Email, Is.EqualTo(new EmailAddress("example@example.com")));
                Assert.That(user.Password, Is.EqualTo(UserDtoTest.TestPassword));
                Assert.That(user.Roles, Is.EqualTo(new RoleId[]
                {
                    new RoleId(roles[0]),
                    new RoleId(roles[1])
                }));
                Assert.That(user.Permissions, Is.EqualTo(new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource"), "MyPermission"),
                    new PermissionId(new ResourceId("MyResource2"), "MyPermission2")
                }));
            });
        }

        [Test]
        public void TestEquals_WhenTwoIdentitcalUsersDtosGiven_ThenTrueIsReturned()
        {
            var roles = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            Guid userId = Guid.NewGuid();
            UserDto leftUserDto = this.GetUserDto(
                userId,
                roles: roles);
            UserDto rightUserDto = this.GetUserDto(
                userId,
                roles: roles);

            Assert.That(leftUserDto.Equals(rightUserDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentUsersDtosGiven_ThenFalseIsReturned()
        {
            var rolesLeft = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var rolesRight = new Guid[]
            {
                rolesLeft[0]
            };
            var permissionsLeft = new (string ResourceId, string Name)[]
            {
                ("MyResource", "MyPermission"),
                ("MyResource2", "MyPermission2")
            };
            var permissionsRight = new (string ResourceId, string Name)[]
            {
                ("MyResource2", "MyPermission2")
            };
            Guid userIdLeft = Guid.NewGuid();
            Guid userIdRight = Guid.NewGuid();
            HashedPassword hashedPasswordLeft = UserDtoTest.TestPassword;
            HashedPassword hashedPasswordRight = HashedPassword.Hash(new Password("MyPassword2"));
            UserDto leftUserDto = this.GetUserDto(
                userIdLeft,
                "example@example.com",
                hashedPasswordLeft.ToString(),
                rolesLeft,
                permissionsLeft);
            UserDto rightUserDto = this.GetUserDto(
                userIdRight,
                "example2@example.com",
                hashedPasswordRight.ToString(),
                rolesRight,
                permissionsRight);

            Assert.That(leftUserDto.Equals(rightUserDto), Is.False);
        }

        [Test]
        public void TestGetHashCode_WhenTwoIdenticalUsersDtosGiven_ThenSameHashCodesAreReturned()
        {
            var roles = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            Guid userId = Guid.NewGuid();
            var leftUserDto = this.GetUserDto(
                userId,
                roles: roles);
            var rightUserDto = this.GetUserDto(
                userId,
                roles: roles);

            Assert.That(leftUserDto.GetHashCode(), Is.EqualTo(rightUserDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentUsersDtosGiven_ThenDifferentHashCodesAreReturned()
        {
            var rolesLeft = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var rolesRight = new Guid[]
            {
                rolesLeft[0]
            };
            var permissionsLeft = new (string ResourceId, string Name)[]
            {
                ("MyResource", "MyPermission"),
                ("MyResource2", "MyPermission2")
            };
            var permissionsRight = new (string ResourceId, string Name)[]
            {
                ("MyResource2", "MyPermission2")
            };
            Guid userIdLeft = Guid.NewGuid();
            Guid userIdRight = Guid.NewGuid();
            HashedPassword hashedPasswordLeft = UserDtoTest.TestPassword;
            HashedPassword hashedPasswordRight = HashedPassword.Hash(new Password("MyPassword2"));
            UserDto leftUserDto = this.GetUserDto(
                userIdLeft,
                "example@example.com",
                hashedPasswordLeft.ToString(),
                rolesLeft,
                permissionsLeft);
            UserDto rightUserDto = this.GetUserDto(
                userIdRight,
                "example2@example.com",
                hashedPasswordRight.ToString(),
                rolesRight,
                permissionsRight);

            Assert.That(leftUserDto.GetHashCode(), Is.Not.EqualTo(rightUserDto.GetHashCode()));
        }
    }
}