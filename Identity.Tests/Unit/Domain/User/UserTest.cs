using DDD.Domain.Events;
using Identity.Domain;
using Moq;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class UserTest
    {
        private static HashedPassword TestPassword = HashedPassword.Hash("MyPassword");

        [Test]
        public void TestConstructing_WhenEmailGiven_ThenEmailIsSet()
        {
            var user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("myemail@example.com"),
                password: UserTest.TestPassword);

            Assert.That(user.Email, Is.EqualTo(new EmailAddress("myemail@example.com")));
        }

        [Test]
        public void TestConstructing_WhenNullEmailGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("email"),
                () => new User(
                    id: UserId.Generate(),
                    email: null,
                    password: UserTest.TestPassword));
        }

        [Test]
        public void TestConstructing_WhenPasswordGiven_ThenPasswordIsSet()
        {
            var user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("myemail@example.com"),
                password: UserTest.TestPassword);

            Assert.That(user.Password, Is.EqualTo(UserTest.TestPassword));
        }

        [Test]
        public void TestConstructing_WhenNullPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("password"),
                () => new User(
                    id: UserId.Generate(),
                    email: new EmailAddress("myemail@example.com"),
                    password: null));
        }

        [Test]
        public void TestConstructing_WhenRolesGiven_ThenRolesIsSet()
        {
            var roles = new RoleId[]
            {
                RoleId.Generate(),
                RoleId.Generate()
            };
            var user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("myemail@example.com"),
                password: UserTest.TestPassword,
                roles: roles);

            Assert.That(user.Roles, Is.EqualTo(roles));
        }

        [Test]
        public void TestConstructing_WhenRolesNotGiven_ThenRolesAreEmpty()
        {
            var user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("myemail@example.com"),
                password: UserTest.TestPassword);

            Assert.That(user.Roles, Is.Empty);
        }

        [Test]
        public void TestCreate_WhenCreatingUser_ThenNewUserIsReturned()
        {
            User user = User.Create(
                email: new EmailAddress("myemail@example.com"),
                password: UserTest.TestPassword);

            Assert.Multiple(() =>
            {
                Assert.That(user.Email, Is.EqualTo(new EmailAddress("myemail@example.com")));
                Assert.That(user.Password, Is.EqualTo(UserTest.TestPassword));
            });
        }

        [Test]
        public void TestCreate_WhenCreatingUser_ThenUserCreatedIsNotified()
        {
            UserCreated userCreatedEvent = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<UserCreated>()))
                .Callback((UserCreated p) => userCreatedEvent = p);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            User user = User.Create(
                email: new EmailAddress("myemail@example.com"),
                password: UserTest.TestPassword);

            Assert.Multiple(() =>
            {
                Assert.That(userCreatedEvent.UserId, Is.EqualTo(user.Id));
                Assert.That(userCreatedEvent.UserEmail, Is.EqualTo(user.Email));
            });
        }

        [Test]
        public void TestEmailSet_WhenNullEmailGiven_ThenArgumentNullExceptionIsThrown()
        {
            var userId = UserId.Generate();
            var user = new User(
                id: userId,
                email: new EmailAddress("myemail@example.com"),
                password: UserTest.TestPassword);

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("email"),
                () => user.Email = null);
        }

        [Test]
        public void TestEmailSet_WhenEmailGiven_ThenEmailIsSet()
        {
            var userId = UserId.Generate();
            var user = new User(
                id: userId,
                email: new EmailAddress("myemail@example.com"),
                password: UserTest.TestPassword);

            user.Email = new EmailAddress("newemail@example.com");

            Assert.That(user.Email, Is.EqualTo(new EmailAddress("newemail@example.com")));
        }

        [Test]
        public void TestPasswordSet_WhenNullPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            var userId = UserId.Generate();
            var user = new User(
                id: userId,
                email: new EmailAddress("myemail@example.com"),
                password: UserTest.TestPassword);

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("password"),
                () => user.Password = null);
        }

        [Test]
        public void TestPasswordSet_WhenPasswordGiven_ThenPasswordIsSet()
        {
            var newPassword = HashedPassword.Hash("MyPassword2");
            var userId = UserId.Generate();
            var user = new User(
                id: userId,
                email: new EmailAddress("myemail@example.com"),
                password: UserTest.TestPassword);

            user.Password = newPassword;

            Assert.That(user.Password, Is.EqualTo(newPassword));
        }

        [Test]
        public void TestObtainPermission_WhenObtainingPermission_ThenUserPermissionObtainedIsNotified()
        {
            UserPermissionObtained userPermissionObtained = null;
            var permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<UserPermissionObtained>()))
                .Callback((UserPermissionObtained p) => userPermissionObtained = p);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            User user = User.Create(
                email: new EmailAddress("myemail@example.com"),
                password: UserTest.TestPassword);

            user.ObtainPermission(permissionId);

            Assert.Multiple(() =>
            {
                Assert.That(userPermissionObtained.UserId, Is.EqualTo(user.Id));
                Assert.That(userPermissionObtained.ObtainedPermissionId, Is.EqualTo(permissionId));
            });
        }

        [Test]
        public void TestObtainPermission_WhenPermissionGiven_ThenUserHasPermission()
        {
            var permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");

            User user = User.Create(
                email: new EmailAddress("myemail@example.com"),
                password: UserTest.TestPassword);

            user.ObtainPermission(permissionId);

            Assert.That(user.IsPermittedTo(permissionId), Is.True);
        }

        [Test]
        public void TestRevokePermission_WhenRovekingPermission_ThenUserPermissionRevokedIsNotified()
        {
            UserPermissionRevoked userPermissionRevokedEvent = null;
            var permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<UserPermissionRevoked>()))
                .Callback((UserPermissionRevoked p) => userPermissionRevokedEvent = p);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;

            User user = User.Create(
                email: new EmailAddress("myemail@example.com"),
                password: UserTest.TestPassword);
            user.ObtainPermission(permissionId);

            user.RevokePermission(permissionId);

            Assert.Multiple(() =>
            {
                Assert.That(userPermissionRevokedEvent.UserId, Is.EqualTo(user.Id));
                Assert.That(userPermissionRevokedEvent.RevokedPermissionId, Is.EqualTo(permissionId));
            });
        }

        [Test]
        public void TestRevokePermission_WhenPermissionIdGiven_ThenPermissionIsRevoked()
        {
            var permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");

            User user = User.Create(
                email: new EmailAddress("myemail@example.com"),
                password: UserTest.TestPassword);
            user.ObtainPermission(permissionId);

            user.RevokePermission(permissionId);

            Assert.That(user.IsPermittedTo(permissionId), Is.False);
        }

        [Test]
        public void TestHasRole_WhenNullRoleIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            User user = User.Create(
                email: new EmailAddress("myemail@example.com"),
                password: UserTest.TestPassword);

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("roleId"),
                () => user.HasRole(null));
        }

        [Test]
        public void TestHasRole_WhenUserHasRole_ThenTrueIsReturned()
        {
            var roleId = RoleId.Generate();
            var roles = new RoleId[]
            {
                roleId
            };

            User user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("email@example.com"),
                password: UserTest.TestPassword,
                roles: roles);

            bool result = user.HasRole(roleId);

            Assert.That(result, Is.True);
        }

        [Test]
        public void TestHasRole_WhenUserHasntRole_ThenFalseIsReturned()
        {
            var roleId = RoleId.Generate();

            User user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("email@example.com"),
                password: UserTest.TestPassword);

            bool result = user.HasRole(roleId);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TestAssumeRole_WhenNullRoleIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            var user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("email@example.com"),
                password: UserTest.TestPassword);

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("roleId"),
                () => user.AssumeRole(null));
        }

        [Test]
        public void TestAssumeRole_WhenHolderWasRoleWasAlreadyAssumeed_ThenInvalidOperationIsThrown()
        {
            var roleId = RoleId.Generate();
            var roles = new RoleId[]
            {
                roleId
            };

            User user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("email@example.com"),
                password: UserTest.TestPassword,
                roles: roles);

            Assert.Throws(
                Is.InstanceOf<InvalidOperationException>()
                    .And.Message
                    .EqualTo("Role was already assumed."),
                () => user.AssumeRole(roleId));
        }

        [Test]
        public void TestAssumeRole_WhenRoleGiven_ThenUserHasRole()
        {
            var roleId = RoleId.Generate();

            var user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("email@example.com"),
                password: UserTest.TestPassword);

            user.AssumeRole(roleId);

            Assert.That(user.HasRole(roleId), Is.True);
        }

        [Test]
        public void TestAssumeRole_WhenAssumingRole_ThenUserRoleAssumedIsNotified()
        {
            UserRoleAssumed userRoleAssumed = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<UserRoleAssumed>()))
                .Callback((UserRoleAssumed p) => userRoleAssumed = p);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var roleId = RoleId.Generate();

            var user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("email@example.com"),
                password: UserTest.TestPassword);

            user.AssumeRole(roleId);

            Assert.Multiple(() =>
            {
                Assert.That(userRoleAssumed.UserId, Is.EqualTo(user.Id));
                Assert.That(userRoleAssumed.AssumedRoleId, Is.EqualTo(roleId));
            });
        }

        [Test]
        public void TestRevokeRole_WhenNullRoleIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            var user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("email@example.com"),
                password: UserTest.TestPassword);

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("roleId"),
                () => user.RevokeRole(null));
        }

        [Test]
        public void TestRevokeRole_WhenUserRoleWasAlreadyRevoked_ThenInvalidOperationIsThrown()
        {
            var roleId = RoleId.Generate();

            User user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("email@example.com"),
                password: UserTest.TestPassword);

            Assert.Throws(
                Is.InstanceOf<InvalidOperationException>()
                    .And.Message
                    .EqualTo("Role wasn't assumed."),
                () => user.RevokeRole(roleId));
        }

        [Test]
        public void TestRevokeRole_WhenRoleIdGiven_ThenUserRoleIsRevoked()
        {
            var roleId = RoleId.Generate();

            var user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("email@example.com"),
                password: UserTest.TestPassword);
            user.AssumeRole(roleId);

            user.RevokeRole(roleId);

            Assert.That(user.HasRole(roleId), Is.False);
        }

        [Test]
        public void TestRevokeRole_WhenAssumingRole_ThenUserRoleRevokedIsNotified()
        {
            UserRoleRevoked userRoleRevoked = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<UserRoleRevoked>()))
                .Callback((UserRoleRevoked p) => userRoleRevoked = p);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            var roleId = RoleId.Generate();

            var user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("email@example.com"),
                password: UserTest.TestPassword);
            user.AssumeRole(roleId);

            user.RevokeRole(roleId);

            Assert.Multiple(() =>
            {
                Assert.That(userRoleRevoked.UserId, Is.EqualTo(user.Id));
                Assert.That(userRoleRevoked.RevokedRoleId, Is.EqualTo(roleId));
            });
        }
    }
}