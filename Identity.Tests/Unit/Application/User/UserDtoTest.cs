using Identity.Application;
using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Application
{
    [TestFixture]
    public class UserDtoTest
    {
        private static readonly HashedPassword TestPassword = HashedPassword.Hash(new Password("MyPassword"));

        [Test]
        public void TestConstructing_WhenIdGiven_ThenIdIsSet()
        {
            var userId = Guid.NewGuid();
            var userDto = new UserDto(userId, "example@example.com", UserDtoTest.TestPassword.ToString());

            Assert.That(userDto.Id, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstructing_WhenEmailGiven_ThenEmailIsSet()
        {
            var userId = Guid.NewGuid();
            var userDto = new UserDto(userId, "example@example.com", UserDtoTest.TestPassword.ToString());

            Assert.That(userDto.Email, Is.EqualTo("example@example.com"));
        }

        [Test]
        public void TestConstructing_WhenHashedPasswordGiven_ThenHashedPasswordIsSet()
        {
            var userId = Guid.NewGuid();
            var userDto = new UserDto(userId, "example@example.com", UserDtoTest.TestPassword.ToString());

            Assert.That(userDto.HashedPassword, Is.EqualTo(UserDtoTest.TestPassword.ToString()));
        }

        [Test]
        public void TestConstructing_WhenRolesGiven_ThenRolesIsSet()
        {
            var roles = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var userId = Guid.NewGuid();
            var userDto = new UserDto(userId, "example@example.com", UserDtoTest.TestPassword.ToString(), roles);

            Assert.That(userDto.Roles, Is.EqualTo(roles));
        }

        [Test]
        public void TestConstructing_WhenNullRolesGiven_ThenEmptyRolesIsSet()
        {
            var userId = Guid.NewGuid();
            var userDto = new UserDto(userId, "example@example.com", UserDtoTest.TestPassword.ToString(), null);

            Assert.That(userDto.Roles, Is.Empty);
        }

        [Test]
        public void TestConstructing_WhenPermissionsGiven_ThenPermissionsIsSet()
        {
            var permissions = new (string ResourceId, string Name)[]
            {
                ("MyResource", "MyPermission"),
                ("MyResource2", "MyPermission2")
            };
            var userId = Guid.NewGuid();
            var userDto = new UserDto(userId, "example@example.com", UserDtoTest.TestPassword.ToString(), null, permissions);

            Assert.That(userDto.Permissions, Is.EqualTo(permissions));
        }

        [Test]
        public void TestConstructing_WhenNullPermissionsGiven_ThenPermissionsIsSet()
        {
            var userId = Guid.NewGuid();
            var userDto = new UserDto(userId, "example@example.com", UserDtoTest.TestPassword.ToString(), null, null);

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
            var userDto = new UserDto(userId, "example@example.com", UserDtoTest.TestPassword.ToString(), roles, permissions);

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
            var rolesLeft = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var rolesRight = new Guid[]
            {
                new Guid(rolesLeft[0].ToString()),
                new Guid(rolesLeft[1].ToString())
            };
            var permissionsLeft = new (string ResourceId, string Name)[]
            {
                ("MyResource", "MyPermission"),
                ("MyResource2", "MyPermission2")
            };
            var permissionsRight = new (string ResourceId, string Name)[]
            {
                ("MyResource", "MyPermission"),
                ("MyResource2", "MyPermission2")
            };
            var userIdLeft = Guid.NewGuid();
            var userIdRight = new Guid(userIdLeft.ToString());
            var leftUserDto = new UserDto(
                userIdLeft,
                "example@example.com",
                UserDtoTest.TestPassword.ToString(),
                rolesLeft,
                permissionsLeft);
            var rightUserDto = new UserDto(
                userIdRight,
                "example@example.com",
                UserDtoTest.TestPassword.ToString(),
                rolesRight,
                permissionsRight);

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
            var userIdLeft = Guid.NewGuid();
            var userIdRight = Guid.NewGuid();
            var hashedPasswordLeft = UserDtoTest.TestPassword;
            var hashedPasswordRight = HashedPassword.Hash(new Password("MyPassword2"));
            var leftUserDto = new UserDto(
                userIdLeft,
                "example@example.com",
                hashedPasswordLeft.ToString(),
                rolesLeft,
                permissionsLeft);
            var rightUserDto = new UserDto(
                userIdRight,
                "example2@example.com",
                hashedPasswordRight.ToString(),
                rolesRight,
                permissionsRight);

            Assert.That(leftUserDto.Equals(rightUserDto), Is.False);
        }

        [Test]
        public void TestGetHashCode_WhenTwoIdenticalUsersDtosGiven_ThenSameHashCodesIsReturned()
        {
            var rolesLeft = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var rolesRight = new Guid[]
            {
                new Guid(rolesLeft[0].ToString()),
                new Guid(rolesLeft[1].ToString())
            };
            var permissionsLeft = new (string ResourceId, string Name)[]
            {
                ("MyResource", "MyPermission"),
                ("MyResource2", "MyPermission2")
            };
            var permissionsRight = new (string ResourceId, string Name)[]
            {
                ("MyResource", "MyPermission"),
                ("MyResource2", "MyPermission2")
            };
            var userIdLeft = Guid.NewGuid();
            var userIdRight = new Guid(userIdLeft.ToString());
            var leftUserDto = new UserDto(
                userIdLeft,
                "example@example.com",
                UserDtoTest.TestPassword.ToString(),
                rolesLeft,
                permissionsLeft);
            var rightUserDto = new UserDto(
                userIdRight,
                "example@example.com",
                UserDtoTest.TestPassword.ToString(),
                rolesRight,
                permissionsRight);

            Assert.That(leftUserDto.GetHashCode(), Is.EqualTo(rightUserDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentUsersDtosGiven_ThenDifferentHashCodesIsReturned()
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
            var userIdLeft = Guid.NewGuid();
            var userIdRight = Guid.NewGuid();
            var hashedPasswordLeft = UserDtoTest.TestPassword;
            var hashedPasswordRight = HashedPassword.Hash(new Password("MyPassword2"));
            var leftUserDto = new UserDto(
                userIdLeft,
                "example@example.com",
                hashedPasswordLeft.ToString(),
                rolesLeft,
                permissionsLeft);
            var rightUserDto = new UserDto(
                userIdRight,
                "example2@example.com",
                hashedPasswordRight.ToString(),
                rolesRight,
                permissionsRight);

            Assert.That(leftUserDto.GetHashCode(), Is.Not.EqualTo(rightUserDto.GetHashCode()));
        }
    }
}