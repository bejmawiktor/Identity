using Identity.Domain;
using Moq;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class AuthorizationServiceTest
    {
        [Test]
        public void TestConstructing_WhenUsersRepositoryGiven_ThenUsersRepositoryIsSet()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;

            var authorizationService = new AuthorizationService(usersRepository, rolesRepository);

            Assert.That(authorizationService.UsersRepository, Is.EqualTo(usersRepository));
        }

        [Test]
        public void TestConstructing_WhenNullUsersRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var rolesRepositoryMock = new Mock<IRolesRepository>();

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("usersRepository"),
               () => new AuthorizationService(null, rolesRepositoryMock.Object));
        }

        [Test]
        public void TestConstructing_WhenResourcesRepositoryGiven_ThenResourcesRepositoryIsSet()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;

            var authorizationService = new AuthorizationService(usersRepository, rolesRepository);

            Assert.That(authorizationService.RolesRepository, Is.EqualTo(rolesRepository));
        }

        [Test]
        public void TestConstructing_WhenNullResourcesRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("rolesRepository"),
               () => new AuthorizationService(usersRepositoryMock.Object, null));
        }

        [Test]
        public void TestCheckUserAccess_WhenNullUserIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            var authorizationService = new AuthorizationService(usersRepository, rolesRepository);

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("userId"),
               () => authorizationService.CheckUserAccess(
                   userId: null,
                   permissionId: new PermissionId(new ResourceId("MyResource"), "MyPermission")));
        }

        [Test]
        public void TestCheckUserAccess_WhenNullPermissionIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            var authorizationService = new AuthorizationService(usersRepository, rolesRepository);

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("permissionId"),
               () => authorizationService.CheckUserAccess(
                   userId: UserId.Generate(),
                   permissionId: null));
        }

        [Test]
        public void TestCheckUserAccess_WhenUserPermittedByRoleGiven_ThenTrueIsReturned()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var permittedRoleId = RoleId.Generate();
            var notPermittedRoleId = RoleId.Generate();
            var user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("example@example.com"),
                password: HashedPassword.Hash("MyPassword"),
                roles: new RoleId[]
                {
                    notPermittedRoleId,
                    permittedRoleId,
                });
            var permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            var permittedRole = new Role(
                id: permittedRoleId,
                name: "MyRole",
                description: "My role description",
                permissions: new PermissionId[]
                {
                    permissionId
                });
            var notPermittedRole = new Role(
                id: notPermittedRoleId,
                name: "MyRole",
                description: "My role description");
            usersRepositoryMock.Setup(u => u.Get(It.IsAny<UserId>())).Returns(user);
            rolesRepositoryMock.Setup(u => u.Get(permittedRoleId)).Returns(permittedRole);
            rolesRepositoryMock.Setup(u => u.Get(notPermittedRoleId)).Returns(notPermittedRole);
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            var authorizationService = new AuthorizationService(usersRepository, rolesRepository);

            bool userIsPermitted = authorizationService.CheckUserAccess(user.Id, permissionId);

            Assert.That(userIsPermitted, Is.True);
        }

        [Test]
        public void TestCheckUserAccess_WhenUserPermittedBySinglePermissionGiven_ThenTrueIsReturned()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            var user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("example@example.com"),
                password: HashedPassword.Hash("MyPassword"),
                permissions: new PermissionId[]
                {
                    permissionId,
                    new PermissionId(new ResourceId("MyResource"), "MyPermission2")
        });
            usersRepositoryMock.Setup(u => u.Get(It.IsAny<UserId>())).Returns(user);
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            var authorizationService = new AuthorizationService(usersRepository, rolesRepository);

            bool userIsPermitted = authorizationService.CheckUserAccess(user.Id, permissionId);

            Assert.That(userIsPermitted, Is.True);
        }

        [Test]
        public void TestCheckUserAccess_WhenUserPermittedFromRoleAndSinglePermissionGiven_ThenTrueIsReturned()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            var user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("example@example.com"),
                password: HashedPassword.Hash("MyPassword"),
                roles: new RoleId[]
                {
                    RoleId.Generate()
                },
                permissions: new PermissionId[]
                {
                    permissionId
                });
            var role = new Role(
                id: RoleId.Generate(),
                name: "MyRole",
                description: "My role description",
                permissions: new PermissionId[]
                {
                    permissionId
                });
            usersRepositoryMock.Setup(u => u.Get(It.IsAny<UserId>())).Returns(user);
            rolesRepositoryMock.Setup(u => u.Get(It.IsAny<RoleId>())).Returns(role);
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            var authorizationService = new AuthorizationService(usersRepository, rolesRepository);

            bool userIsPermitted = authorizationService.CheckUserAccess(user.Id, permissionId);

            Assert.That(userIsPermitted, Is.True);
        }

        [Test]
        public void TestCheckUserAccess_WhenUserIsntPermittedFromRoleAndSinglePermissionGiven_ThenFalseIsReturned()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            var user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("example@example.com"),
                password: HashedPassword.Hash("MyPassword"));
            usersRepositoryMock.Setup(u => u.Get(It.IsAny<UserId>())).Returns(user);
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            var authorizationService = new AuthorizationService(usersRepository, rolesRepository);

            bool userIsPermitted = authorizationService.CheckUserAccess(user.Id, permissionId);

            Assert.That(userIsPermitted, Is.False);
        }
    }
}