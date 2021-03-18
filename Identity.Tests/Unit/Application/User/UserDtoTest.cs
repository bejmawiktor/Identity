using Identity.Application;
using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Application
{
    [TestFixture]
    public class UserDtoTest
    {
        [Test]
        public void TestConstructing_WhenIdGiven_ThenIdIsSet()
        {
            var userId = Guid.NewGuid();
            var userDto = new UserDto(userId, "example@example.com", "asasdasfsag");

            Assert.That(userDto.Id, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstructing_WhenEmailGiven_ThenEmailIsSet()
        {
            var userId = Guid.NewGuid();
            var userDto = new UserDto(userId, "example@example.com", "asasdasfsag");

            Assert.That(userDto.Email, Is.EqualTo("example@example.com"));
        }

        [Test]
        public void TestConstructing_WhenHashedPasswordGiven_ThenHashedPasswordIsSet()
        {
            var userId = Guid.NewGuid();
            var userDto = new UserDto(userId, "example@example.com", "asasdasfsag");

            Assert.That(userDto.HashedPassword, Is.EqualTo("asasdasfsag"));
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
            var userDto = new UserDto(userId, "example@example.com", "asasdasfsag", roles);

            Assert.That(userDto.Roles, Is.EqualTo(roles));
        }

        [Test]
        public void TestConstructing_WhenNullRolesGiven_ThenEmptyRolesIsSet()
        {
            var userId = Guid.NewGuid();
            var userDto = new UserDto(userId, "example@example.com", "asasdasfsag", null);

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
            var userDto = new UserDto(userId, "example@example.com", "asasdasfsag", null, permissions);

            Assert.That(userDto.Permissions, Is.EqualTo(permissions));
        }

        [Test]
        public void TestConstructing_WhenNullPermissionsGiven_ThenPermissionsIsSet()
        {
            var userId = Guid.NewGuid();
            var userDto = new UserDto(userId, "example@example.com", "asasdasfsag", null, null);

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
            var hashedPassword = HashedPassword.Hash("MyPassword");
            var userDto = new UserDto(userId, "example@example.com", hashedPassword.ToString(), roles, permissions);

            User user = userDto.ToUser();

            Assert.Multiple(() =>
            {
                Assert.That(user.Id, Is.EqualTo(new UserId(userId)));
                Assert.That(user.Email, Is.EqualTo(new EmailAddress("example@example.com")));
                Assert.That(user.Password, Is.EqualTo(hashedPassword));
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
    }
}