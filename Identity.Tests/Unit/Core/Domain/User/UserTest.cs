using DDD.Domain.Events;
using Identity.Core.Domain;
using Identity.Core.Events;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Tests.Unit.Core.Domain
{
    using Application = Identity.Core.Domain.Application;

    [TestFixture]
    public class UserTest
    {
        private static HashedPassword TestPassword = HashedPassword.Hash(new Password("MyPassword"));

        [Test]
        public void TestConstructor_WhenEmailGiven_ThenEmailIsSet()
        {
            User user = this.GetUser(email: new EmailAddress("myemail@example.com"));

            Assert.That(user.Email, Is.EqualTo(new EmailAddress("myemail@example.com")));
        }

        private User GetUser(
            UserId id = null, 
            EmailAddress email = null, 
            HashedPassword password = null,
            IEnumerable<RoleId> roles = null)
        {
            return new User(
                id: id ?? UserId.Generate(),
                email: email ?? new EmailAddress("myemail@example.com"),
                password: password ?? UserTest.TestPassword,
                roles: roles ?? null);
        }

        [Test]
        public void TestConstructor_WhenNullEmailGiven_ThenArgumentNullExceptionIsThrown()
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
        public void TestConstructor_WhenPasswordGiven_ThenPasswordIsSet()
        {
            User user = this.GetUser(password: UserTest.TestPassword);

            Assert.That(user.Password, Is.EqualTo(UserTest.TestPassword));
        }

        [Test]
        public void TestConstructor_WhenNullPasswordGiven_ThenArgumentNullExceptionIsThrown()
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
        public void TestConstructor_WhenRolesGiven_ThenRolesIsSet()
        {
            var roles = new RoleId[]
            {
                RoleId.Generate(),
                RoleId.Generate()
            };
            User user = this.GetUser(
                roles: roles);

            Assert.That(user.Roles, Is.EqualTo(roles));
        }

        [Test]
        public void TestConstructor_WhenRolesNotGiven_ThenRolesAreEmpty()
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
                Assert.That(userCreatedEvent.UserId, Is.EqualTo(user.Id.ToGuid()));
                Assert.That(userCreatedEvent.UserEmail, Is.EqualTo(user.Email.ToString()));
            });
        }

        [Test]
        public void TestEmailSet_WhenNullEmailGiven_ThenArgumentNullExceptionIsThrown()
        {
            User user = this.GetUser();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("email"),
                () => user.Email = null);
        }

        [Test]
        public void TestEmailSet_WhenEmailGiven_ThenEmailIsSet()
        {
            User user = this.GetUser();

            user.Email = new EmailAddress("newemail@example.com");

            Assert.That(user.Email, Is.EqualTo(new EmailAddress("newemail@example.com")));
        }

        [Test]
        public void TestPasswordSet_WhenNullPasswordGiven_ThenArgumentNullExceptionIsThrown()
        {
            User user = this.GetUser();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("password"),
                () => user.Password = null);
        }

        [Test]
        public void TestPasswordSet_WhenPasswordGiven_ThenPasswordIsSet()
        {
            HashedPassword newPassword = HashedPassword.Hash(new Password("MyPassword2"));
            User user = this.GetUser();

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
            User user = this.GetUser();

            user.ObtainPermission(permissionId);

            Assert.Multiple(() =>
            {
                Assert.That(userPermissionObtained.UserId, Is.EqualTo(user.Id.ToGuid()));
                Assert.That(userPermissionObtained.ObtainedPermissionId, Is.EqualTo(permissionId.ToString()));
            });
        }

        [Test]
        public void TestObtainPermission_WhenPermissionGiven_ThenUserHasPermission()
        {
            PermissionId permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            User user = this.GetUser();

            user.ObtainPermission(permissionId);

            Assert.That(user.IsPermittedTo(permissionId), Is.True);
        }

        [Test]
        public void TestRevokePermission_WhenRovekingPermission_ThenUserPermissionRevokedIsNotified()
        {
            UserPermissionRevoked userPermissionRevokedEvent = null;
            PermissionId permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<UserPermissionRevoked>()))
                .Callback((UserPermissionRevoked p) => userPermissionRevokedEvent = p);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            User user = this.GetUser();
            user.ObtainPermission(permissionId);

            user.RevokePermission(permissionId);

            Assert.Multiple(() =>
            {
                Assert.That(userPermissionRevokedEvent.UserId, Is.EqualTo(user.Id.ToGuid()));
                Assert.That(userPermissionRevokedEvent.RevokedPermissionId, Is.EqualTo(permissionId.ToString()));
            });
        }

        [Test]
        public void TestRevokePermission_WhenPermissionIdGiven_ThenPermissionIsRevoked()
        {
            var permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            User user = this.GetUser();
            user.ObtainPermission(permissionId);

            user.RevokePermission(permissionId);

            Assert.That(user.IsPermittedTo(permissionId), Is.False);
        }

        [Test]
        public void TestHasRole_WhenNullRoleIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            User user = this.GetUser();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("roleId"),
                () => user.HasRole(null));
        }

        [Test]
        public void TestHasRole_WhenUserHasRole_ThenTrueIsReturned()
        {
            RoleId roleId = RoleId.Generate();
            var roles = new RoleId[]
            {
                roleId
            };
            User user = this.GetUser(
                roles: roles);

            bool result = user.HasRole(roleId);

            Assert.That(result, Is.True);
        }

        [Test]
        public void TestHasRole_WhenUserHasntRole_ThenFalseIsReturned()
        {
            RoleId roleId = RoleId.Generate();
            User user = this.GetUser();

            bool result = user.HasRole(roleId);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TestAssumeRole_WhenNullRoleIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            User user = this.GetUser();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("roleId"),
                () => user.AssumeRole(null));
        }

        [Test]
        public void TestAssumeRole_WhenHolderWasRoleWasAlreadyAssumed_ThenInvalidOperationIsThrown()
        {
            RoleId roleId = RoleId.Generate();
            var roles = new RoleId[]
            {
                roleId
            };

            User user = this.GetUser(roles: roles);

            Assert.Throws(
                Is.InstanceOf<InvalidOperationException>()
                    .And.Message
                    .EqualTo("Role was already assumed."),
                () => user.AssumeRole(roleId));
        }

        [Test]
        public void TestAssumeRole_WhenRoleGiven_ThenUserHasRole()
        {
            RoleId roleId = RoleId.Generate();
            User user = this.GetUser();

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
            RoleId roleId = RoleId.Generate();
            User user = this.GetUser();

            user.AssumeRole(roleId);

            Assert.Multiple(() =>
            {
                Assert.That(userRoleAssumed.UserId, Is.EqualTo(user.Id.ToGuid()));
                Assert.That(userRoleAssumed.AssumedRoleId, Is.EqualTo(roleId.ToGuid()));
            });
        }

        [Test]
        public void TestRevokeRole_WhenNullRoleIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            User user = this.GetUser();

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("roleId"),
                () => user.RevokeRole(null));
        }

        [Test]
        public void TestRevokeRole_WhenUserRoleWasAlreadyRevoked_ThenInvalidOperationIsThrown()
        {
            RoleId roleId = RoleId.Generate();

            User user = this.GetUser();

            Assert.Throws(
                Is.InstanceOf<InvalidOperationException>()
                    .And.Message
                    .EqualTo("Role wasn't assumed."),
                () => user.RevokeRole(roleId));
        }

        [Test]
        public void TestRevokeRole_WhenRoleIdGiven_ThenUserRoleIsRevoked()
        {
            RoleId roleId = RoleId.Generate();
            User user = this.GetUser();
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
            RoleId roleId = RoleId.Generate();
            User user = this.GetUser();
            user.AssumeRole(roleId);

            user.RevokeRole(roleId);

            Assert.Multiple(() =>
            {
                Assert.That(userRoleRevoked.UserId, Is.EqualTo(user.Id.ToGuid()));
                Assert.That(userRoleRevoked.RevokedRoleId, Is.EqualTo(roleId.ToGuid()));
            });
        }

        [Test]
        public void TestCreateApplication_WhenCreatingApplication_ThenNewApplicationIsReturned()
        {
            UserId userId = UserId.Generate();
            User user = this.GetUser(userId);

            Application application = user.CreateApplication(
                name: "MyApp",
                homepageUrl: new Url("https://www.example.com"),
                callbackUrl: new Url("https://www.example.com/1"));

            Assert.Multiple(() =>
            {
                Assert.That(application.UserId, Is.EqualTo(userId));
                Assert.That(application.Name, Is.EqualTo("MyApp"));
                Assert.That(application.HomepageUrl, Is.EqualTo(new Url("https://www.example.com")));
                Assert.That(application.CallbackUrl, Is.EqualTo(new Url("https://www.example.com/1")));
            });
        }

        [Test]
        public void TestCreateApplication_WhenCreatingApplication_ThenApplicationCreatedIsNotified()
        {
            UserId userId = UserId.Generate();
            ApplicationCreated applicationCreated = null;
            var eventDispatcherMock = new Mock<IEventDispatcher>();
            eventDispatcherMock
                .Setup(e => e.Dispatch(It.IsAny<ApplicationCreated>()))
                .Callback((ApplicationCreated p) => applicationCreated = p);
            EventManager.Instance.EventDispatcher = eventDispatcherMock.Object;
            User user = this.GetUser(userId);

            Application application = user.CreateApplication(
                name: "MyApp",
                homepageUrl: new Url("https://www.example.com"),
                callbackUrl: new Url("https://www.example.com/1"));

            Assert.Multiple(() =>
            {
                Assert.That(applicationCreated.ApplicationId, Is.EqualTo(application.Id.ToGuid()));
                Assert.That(applicationCreated.ApplicationUserId, Is.EqualTo(application.UserId.ToGuid()));
                Assert.That(applicationCreated.ApplicationName, Is.EqualTo(application.Name));
                Assert.That(applicationCreated.ApplicationHomepageUrl, Is.EqualTo(application.HomepageUrl.ToString()));
                Assert.That(applicationCreated.ApplicationCallbackUrl, Is.EqualTo(application.CallbackUrl.ToString()));
            });
        }
    }
}