using Identity.Core.Application;
using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Application.Builders;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Application
{
    [TestFixture]
    public class UserDtoTest
    {
        private static readonly HashedPassword TestPassword = HashedPassword.Hash(new Password("MyPassword"));

        [Test]
        public void TestConstructor_WhenIdGiven_ThenIdIsSet()
        {
            Guid userId = Guid.NewGuid();
            UserDto userDto = new UserDtoBuilder()
                .WithId(userId)
                .Build();

            Assert.That(userDto.Id, Is.EqualTo(userId));
        }

        [Test]
        public void TestConstructor_WhenEmailGiven_ThenEmailIsSet()
        {
            UserDto userDto = new UserDtoBuilder()
                .WithEmail("example@example.com")
                .Build();

            Assert.That(userDto.Email, Is.EqualTo("example@example.com"));
        }

        [Test]
        public void TestConstructor_WhenHashedPasswordGiven_ThenHashedPasswordIsSet()
        {
            UserDto userDto = new UserDtoBuilder()
                .WithPassword(UserDtoTest.TestPassword.ToString())
                .Build();

            Assert.That(userDto.HashedPassword, Is.EqualTo(UserDtoTest.TestPassword.ToString()));
        }

        [Test]
        public void TestConstructor_WhenRolesGiven_ThenRolesAreSet()
        {
            Guid[] roles = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            UserDto userDto = new UserDtoBuilder()
                .WithRoles(roles)
                .Build();

            Assert.That(userDto.Roles, Is.EqualTo(roles));
        }

        [Test]
        public void TestConstructor_WhenNullRolesGiven_ThenEmptyRolesAreSet()
        {
            UserDto userDto = new UserDtoBuilder()
                .WithRoles(null)
                .Build();

            Assert.That(userDto.Roles, Is.Empty);
        }

        [Test]
        public void TestConstructor_WhenPermissionsGiven_ThenPermissionsAreSet()
        {
            (string ResourceId, string Name)[] permissions = new (string ResourceId, string Name)[]
            {
                ("MyResource3", "MyPermission3"),
                ("MyResource4", "MyPermission4")
            };
            UserDto userDto = new UserDtoBuilder()
                .WithPermissions(permissions)
                .Build();

            Assert.That(userDto.Permissions, Is.EqualTo(permissions));
        }

        [Test]
        public void TestConstructor_WhenNullPermissionsGiven_ThenPermissionsAreSet()
        {
            UserDto userDto = new UserDtoBuilder()
                .WithPermissions(null)
                .Build();

            Assert.That(userDto.Permissions, Is.Empty);
        }

        [Test]
        public void TestToUser_WhenConvertingToUser_ThenUserIsReturned()
        {
            Guid[] roles = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            (string ResourceId, string Name)[] permissions = new (string ResourceId, string Name)[]
            {
                ("MyResource", "MyPermission"),
                ("MyResource2", "MyPermission2")
            };
            Guid userId = Guid.NewGuid();
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
            UserDto leftUserDto = UserDtoBuilder.DefaultUserDto;
            UserDto rightUserDto = UserDtoBuilder.DefaultUserDto;

            Assert.That(leftUserDto.Equals(rightUserDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentUsersDtosGiven_ThenFalseIsReturned()
        {
            Guid[] rolesLeft = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            Guid[] rolesRight = new Guid[]
            {
                rolesLeft[0]
            };
            (string ResourceId, string Name)[] permissionsLeft = new (string ResourceId, string Name)[]
            {
                ("MyResource", "MyPermission"),
                ("MyResource2", "MyPermission2")
            };
            (string ResourceId, string Name)[] permissionsRight = new (string ResourceId, string Name)[]
            {
                ("MyResource2", "MyPermission2")
            };
            Guid userIdLeft = Guid.NewGuid();
            Guid userIdRight = Guid.NewGuid();
            HashedPassword hashedPasswordLeft = UserDtoTest.TestPassword;
            HashedPassword hashedPasswordRight = HashedPassword.Hash(new Password("MyPassword2"));
            UserDto leftUserDto = new UserDtoBuilder()
                .WithId(userIdLeft)
                .WithEmail("example@example.com")
                .WithPassword(hashedPasswordLeft.ToString())
                .WithRoles(rolesLeft)
                .WithPermissions(permissionsLeft).Build();
            UserDto rightUserDto = new UserDtoBuilder()
                .WithId(userIdRight)
                .WithEmail("example2@example.com")
                .WithPassword(hashedPasswordRight.ToString())
                .WithRoles(rolesRight)
                .WithPermissions(permissionsRight)
                .Build();

            Assert.That(leftUserDto.Equals(rightUserDto), Is.False);
        }

        [Test]
        public void TestGetHashCode_WhenTwoIdenticalUsersDtosGiven_ThenSameHashCodesAreReturned()
        {
            UserDto leftUserDto = UserDtoBuilder.DefaultUserDto;
            UserDto rightUserDto = UserDtoBuilder.DefaultUserDto;

            Assert.That(leftUserDto.GetHashCode(), Is.EqualTo(rightUserDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentUsersDtosGiven_ThenDifferentHashCodesAreReturned()
        {
            Guid[] rolesLeft = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            Guid[] rolesRight = new Guid[]
            {
                rolesLeft[0]
            };
            (string ResourceId, string Name)[] permissionsLeft = new (string ResourceId, string Name)[]
            {
                ("MyResource", "MyPermission"),
                ("MyResource2", "MyPermission2")
            };
            (string ResourceId, string Name)[] permissionsRight = new (string ResourceId, string Name)[]
            {
                ("MyResource2", "MyPermission2")
            };
            Guid userIdLeft = Guid.NewGuid();
            Guid userIdRight = Guid.NewGuid();
            HashedPassword hashedPasswordLeft = UserDtoTest.TestPassword;
            HashedPassword hashedPasswordRight = HashedPassword.Hash(new Password("MyPassword2"));
            UserDto leftUserDto = new UserDtoBuilder()
                .WithId(userIdLeft)
                .WithEmail("example@example.com")
                .WithPassword(hashedPasswordLeft.ToString())
                .WithRoles(rolesLeft)
                .WithPermissions(permissionsLeft).Build();
            UserDto rightUserDto = new UserDtoBuilder()
                .WithId(userIdRight)
                .WithEmail("example2@example.com")
                .WithPassword(hashedPasswordRight.ToString())
                .WithRoles(rolesRight)
                .WithPermissions(permissionsRight)
                .Build();

            Assert.That(leftUserDto.GetHashCode(), Is.Not.EqualTo(rightUserDto.GetHashCode()));
        }
    }
}