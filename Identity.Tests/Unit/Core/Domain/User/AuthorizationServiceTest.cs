using DDD.Domain.Persistence;
using Identity.Core.Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Core.Domain
{
    using Application = Identity.Core.Domain.Application;
    using ApplicationId = Identity.Core.Domain.ApplicationId;
    using IUnitOfWork = Identity.Core.Domain.IUnitOfWork;

    [TestFixture]
    internal class AuthorizationServiceTest
    {
        private static readonly HashedPassword TestPassword = HashedPassword.Hash(new Password("MyPassword"));

        public static IEnumerable<TestCaseData> IncorrectPermissionsTestData
        {
            get
            {
                yield return new TestCaseData(new object[]
                {
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource1"), "Add"),
                        new PermissionId(new ResourceId("MyResource1"), "Remove")
                    },
                    new PermissionId[]
                    {
                    },
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource1"), "Add"),
                        new PermissionId(new ResourceId("MyResource1"), "Remove"),
                        new PermissionId(new ResourceId("MyResource2"), "Remove")
                    }
                }).SetName($"{nameof(TestGenerateAuthorizationCode_WhenGivenPermissionsAreIncompatibleWithUserPermissions_ThenArgumentExceptionIsThrown)}(1)");
                yield return new TestCaseData(new object[]
                {
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource1"), "Add"),
                        new PermissionId(new ResourceId("MyResource1"), "Remove")
                    },
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource3"), "Remove")
                    },
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource1"), "Add"),
                        new PermissionId(new ResourceId("MyResource3"), "Remove"),
                        new PermissionId(new ResourceId("MyResource2"), "Remove")
                    }
                }).SetName($"{nameof(TestGenerateAuthorizationCode_WhenGivenPermissionsAreIncompatibleWithUserPermissions_ThenArgumentExceptionIsThrown)}(2)");
                yield return new TestCaseData(new object[]
                {
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource1"), "Add"),
                    },
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource3"), "Remove")
                    },
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource1"), "Add"),
                        new PermissionId(new ResourceId("MyResource2"), "Add"),
                        new PermissionId(new ResourceId("MyResource2"), "Remove")
                    }
                }).SetName($"{nameof(TestGenerateAuthorizationCode_WhenGivenPermissionsAreIncompatibleWithUserPermissions_ThenArgumentExceptionIsThrown)}(3)");
                yield return new TestCaseData(new object[]
                {
                    new PermissionId[]
                    {
                    },
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource3"), "Remove")
                    },
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource1"), "Add"),
                        new PermissionId(new ResourceId("MyResource2"), "Add"),
                        new PermissionId(new ResourceId("MyResource3"), "Remove")
                    }
                }).SetName($"{nameof(TestGenerateAuthorizationCode_WhenGivenPermissionsAreIncompatibleWithUserPermissions_ThenArgumentExceptionIsThrown)}(4)");
            }
        }

        public static IEnumerable<TestCaseData> CorrectPermissionsTestData
        {
            get
            {
                yield return new TestCaseData(new object[]
                {
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource1"), "Add"),
                        new PermissionId(new ResourceId("MyResource1"), "Remove")
                    },
                    new PermissionId[]
                    {
                    },
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource1"), "Add")
                    }
                }).SetName($"{nameof(TestGenerateAuthorizationCode_WhenGivenPermissionsAreCompatibleWithUserPermissions_ThenCodeIsReturnedAndAuhtorizationCodeIsPersisted)}(1)");
                yield return new TestCaseData(new object[]
                {
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource1"), "Add"),
                        new PermissionId(new ResourceId("MyResource1"), "Remove")
                    },
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource3"), "Remove")
                    },
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource1"), "Add"),
                        new PermissionId(new ResourceId("MyResource3"), "Remove"),
                        new PermissionId(new ResourceId("MyResource1"), "Remove")
                    }
                }).SetName($"{nameof(TestGenerateAuthorizationCode_WhenGivenPermissionsAreCompatibleWithUserPermissions_ThenCodeIsReturnedAndAuhtorizationCodeIsPersisted)}(2)");
                yield return new TestCaseData(new object[]
                {
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource1"), "Add"),
                    },
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource3"), "Remove")
                    },
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource1"), "Add"),
                        new PermissionId(new ResourceId("MyResource3"), "Remove")
                    }
                }).SetName($"{nameof(TestGenerateAuthorizationCode_WhenGivenPermissionsAreCompatibleWithUserPermissions_ThenCodeIsReturnedAndAuhtorizationCodeIsPersisted)}(3)");
                yield return new TestCaseData(new object[]
                {
                    new PermissionId[]
                    {
                    },
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource3"), "Remove")
                    },
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource3"), "Remove")
                    }
                }).SetName($"{nameof(TestGenerateAuthorizationCode_WhenGivenPermissionsAreCompatibleWithUserPermissions_ThenCodeIsReturnedAndAuhtorizationCodeIsPersisted)}(4)");
                yield return new TestCaseData(new object[]
                {
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource1"), "Add"),
                        new PermissionId(new ResourceId("MyResource1"), "Remove")
                    },
                    new PermissionId[]
                    {
                    },
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource1"), "Add")
                    }
                }).SetName($"{nameof(TestGenerateAuthorizationCode_WhenGivenPermissionsAreCompatibleWithUserPermissions_ThenCodeIsReturnedAndAuhtorizationCodeIsPersisted)}(5)");
                yield return new TestCaseData(new object[]
                {
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource1"), "Add"),
                        new PermissionId(new ResourceId("MyResource1"), "Remove")
                    },
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource2"), "Remove")
                    },
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource2"), "Remove"),
                        new PermissionId(new ResourceId("MyResource1"), "Remove")
                    }
                }).SetName($"{nameof(TestGenerateAuthorizationCode_WhenGivenPermissionsAreCompatibleWithUserPermissions_ThenCodeIsReturnedAndAuhtorizationCodeIsPersisted)}(6)");
            }
        }

        [Test]
        public void TestConstructor_WhenUnitOfWorkGiven_ThenUnitOfWorkIsSet()
        {
            IUnitOfWork unitOfWork = this.GetUnitOfWork();

            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            Assert.That(authorizationService.UnitOfWork, Is.EqualTo(unitOfWork));
        }

        private IUnitOfWork GetUnitOfWork(
            IApplicationsRepository applicationsRepository = null,
            IAuthorizationCodesRepository authorizationCodesRepository = null,
            IPermissionsRepository permissionsRepository = null,
            IRefreshTokensRepository refreshTokensRepository = null,
            IResourcesRepository resourcesRepository = null,
            IRolesRepository rolesRepository = null,
            IUsersRepository usersRepository = null)
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock
                .Setup(u => u.ApplicationsRepository)
                .Returns(applicationsRepository ?? new Mock<IApplicationsRepository>().Object);
            unitOfWorkMock
                .Setup(u => u.AuthorizationCodesRepository)
                .Returns(authorizationCodesRepository ?? new Mock<IAuthorizationCodesRepository>().Object);
            unitOfWorkMock
                .Setup(u => u.PermissionsRepository)
                .Returns(permissionsRepository ?? new Mock<IPermissionsRepository>().Object);
            unitOfWorkMock
                .Setup(u => u.RefreshTokensRepository)
                .Returns(refreshTokensRepository ?? new Mock<IRefreshTokensRepository>().Object);
            unitOfWorkMock
                .Setup(u => u.ResourcesRepository)
                .Returns(resourcesRepository ?? new Mock<IResourcesRepository>().Object);
            unitOfWorkMock
                .Setup(u => u.RolesRepository)
                .Returns(rolesRepository ?? new Mock<IRolesRepository>().Object);
            unitOfWorkMock
                .Setup(u => u.UsersRepository)
                .Returns(usersRepository ?? new Mock<IUsersRepository>().Object);

            return unitOfWorkMock.Object;
        }

        [Test]
        public void TestConstructor_WhenNullUnitOfWorkGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("unitOfWork"),
               () => new AuthorizationService(null));
        }

        [Test]
        public void TestCheckUserAccess_WhenNullUserIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            IUnitOfWork unitOfWork = this.GetUnitOfWork();
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(
               async () => await authorizationService.CheckUserAccess(
                   userId: null,
                   permissionId: new PermissionId(new ResourceId("MyResource"), "MyPermission")));

            Assert.That(exception, Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("userId"));
        }

        [Test]
        public void TestCheckUserAccess_WhenNullPermissionIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            IUnitOfWork unitOfWork = this.GetUnitOfWork();
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(
               async () => await authorizationService.CheckUserAccess(
                   userId: UserId.Generate(),
                   permissionId: null));

            Assert.That(exception, Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("permissionId"));
        }

        [Test]
        public void TestCheckUserAccess_WhenNotFoundUserIdGiven_ThenUserNotFoundExceptionIsThrown()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock.Setup(u => u.GetAsync(It.IsAny<UserId>())).Returns(Task.FromResult((User)null));
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            UserId userId = UserId.Generate();
            IUnitOfWork unitOfWork = this.GetUnitOfWork(usersRepository: usersRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            UserNotFoundException exception = Assert.ThrowsAsync<UserNotFoundException>(
               async () => await authorizationService.CheckUserAccess(
                   userId: userId,
                   permissionId: new PermissionId(new ResourceId("MyResource"), "MyPermission")));

            Assert.That(exception, Is.InstanceOf<UserNotFoundException>()
                .And.Message
                .EqualTo($"User {userId} not found."));
        }

        [Test]
        public async Task TestCheckUserAccess_WhenUserPermittedByRoleGiven_ThenTrueIsReturned()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            RoleId permittedRoleId = RoleId.Generate();
            RoleId notPermittedRoleId = RoleId.Generate();
            var user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("example@example.com"),
                password: AuthorizationServiceTest.TestPassword,
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
            usersRepositoryMock.Setup(u => u.GetAsync(It.IsAny<UserId>())).Returns(Task.FromResult(user));
            rolesRepositoryMock.Setup(u => u.GetAsync(permittedRoleId)).Returns(Task.FromResult(permittedRole));
            rolesRepositoryMock.Setup(u => u.GetAsync(notPermittedRoleId)).Returns(Task.FromResult(notPermittedRole));
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                usersRepository: usersRepository,
                rolesRepository: rolesRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            bool userIsPermitted = await authorizationService.CheckUserAccess(user.Id, permissionId);

            Assert.That(userIsPermitted, Is.True);
        }

        [Test]
        public async Task TestCheckUserAccess_WhenUserPermittedBySinglePermission_ThenTrueIsReturned()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            var user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("example@example.com"),
                password: AuthorizationServiceTest.TestPassword,
                permissions: new PermissionId[]
                {
                    permissionId,
                    new PermissionId(new ResourceId("MyResource"), "MyPermission2")
                });
            usersRepositoryMock.Setup(u => u.GetAsync(It.IsAny<UserId>())).Returns(Task.FromResult(user));
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(usersRepository: usersRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            bool userIsPermitted = await authorizationService.CheckUserAccess(user.Id, permissionId);

            Assert.That(userIsPermitted, Is.True);
        }

        [Test]
        public async Task TestCheckUserAccess_WhenUserPermittedFromRoleAndSinglePermission_ThenTrueIsReturned()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            var user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("example@example.com"),
                password: AuthorizationServiceTest.TestPassword,
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
            usersRepositoryMock.Setup(u => u.GetAsync(It.IsAny<UserId>())).Returns(Task.FromResult(user));
            rolesRepositoryMock.Setup(u => u.GetAsync(It.IsAny<RoleId>())).Returns(Task.FromResult(role));
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                usersRepository: usersRepository,
                rolesRepository: rolesRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            bool userIsPermitted = await authorizationService.CheckUserAccess(user.Id, permissionId);

            Assert.That(userIsPermitted, Is.True);
        }

        [Test]
        public async Task TestCheckUserAccess_WhenUserIsntPermittedFromRoleAndSinglePermission_ThenFalseIsReturned()
        {
            var permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            var user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("example@example.com"),
                password: AuthorizationServiceTest.TestPassword);
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock.Setup(u => u.GetAsync(It.IsAny<UserId>())).Returns(Task.FromResult(user));
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(usersRepository: usersRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            bool userIsPermitted = await authorizationService.CheckUserAccess(user.Id, permissionId);

            Assert.That(userIsPermitted, Is.False);
        }

        [Test]
        public void TestGenerateAuthorizationCode_WhenNullApplicationIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            var permissions = new PermissionId[] { new PermissionId(new ResourceId("MyResource"), "Add") };
            IUnitOfWork unitOfWork = this.GetUnitOfWork();
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(
                async () => await authorizationService.GenerateAuthorizationCode(
                    null,
                    new Url("http://example1.com"),
                    permissions));

            Assert.That(exception, Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("applicationId"));
        }

        [Test]
        public void TestGenerateAuthorizationCode_WhenNullCallbackUrlGiven_ThenArgumentNullExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[] { new PermissionId(new ResourceId("MyResource"), "Add") };
            IUnitOfWork unitOfWork = this.GetUnitOfWork();
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(
                async () => await authorizationService.GenerateAuthorizationCode(
                    applicationId,
                    null,
                    permissions));

            Assert.That(exception, Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("callbackUrl"));
        }

        [Test]
        public void TestGenerateAuthorizationCode_WhenApplicationWasNotFoundInRepository_ThenApplicationNotFoundExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[] { new PermissionId(new ResourceId("MyResource"), "Add") };
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult((Application)null));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(applicationsRepository: applicationsRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ApplicationNotFoundException exception = Assert.ThrowsAsync<ApplicationNotFoundException>(
                async () => await authorizationService.GenerateAuthorizationCode(
                    applicationId,
                    new Url("http://example.com"),
                    permissions));

            Assert.That(exception, Is.InstanceOf<ApplicationNotFoundException>()
                .And.Message
                .EqualTo($"Application {applicationId} not found."));
        }

        [Test]
        public void TestGenerateAuthorizationCode_WhenApplicationCallbackUrlIsNotSameAsRequested_ThenArgumentExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[] { new PermissionId(new ResourceId("MyResource"), "Add") };
            var application = new Application(
                ApplicationId.Generate(),
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(SecretKey.Generate()),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(applicationsRepository: applicationsRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(
                async () => await authorizationService.GenerateAuthorizationCode(
                    applicationId,
                    new Url("http://example1.com"),
                    permissions));

            Assert.That(exception, Is.InstanceOf<ArgumentException>()
                .And.Message
                .EqualTo("Wrong callback url given."));
        }

        [Test]
        public void TestGenerateAuthorizationCode_WhenPermissionsAreNull_ThenArgumentNullExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(new Application(
                    ApplicationId.Generate(),
                    UserId.Generate(),
                    "MyApp1",
                    EncryptedSecretKey.Encrypt(SecretKey.Generate()),
                    new Url("http://example.com"),
                    new Url("http://example.com/1"))));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(applicationsRepository: applicationsRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(
                async () => await authorizationService.GenerateAuthorizationCode(
                    applicationId,
                    new Url("http://example1.com"),
                    null));

            Assert.That(exception, Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("permissions"));
        }

        [Test]
        public void TestGenerateAuthorizationCode_WhenPermissionsAreEmpty_ThenArgumentExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(new Application(
                    ApplicationId.Generate(),
                    UserId.Generate(),
                    "MyApp1",
                    EncryptedSecretKey.Encrypt(SecretKey.Generate()),
                    new Url("http://example.com"),
                    new Url("http://example.com/1"))));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(applicationsRepository: applicationsRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(
                async () => await authorizationService.GenerateAuthorizationCode(
                    applicationId,
                    new Url("http://example1.com"),
                    Enumerable.Empty<PermissionId>()));

            Assert.That(exception, Is.InstanceOf<ArgumentException>()
                .And.Message
                .EqualTo("Permissions can't be empty."));
        }

        [TestCaseSource(nameof(IncorrectPermissionsTestData))]
        public void TestGenerateAuthorizationCode_WhenGivenPermissionsAreIncompatibleWithUserPermissions_ThenArgumentExceptionIsThrown(
            PermissionId[] userPermissions,
            PermissionId[] rolePermissions,
            PermissionId[] requestedPermissions)
        {
            RoleId roleId = RoleId.Generate();
            Role role = new Role(roleId, "MyRole", "My role description.", rolePermissions);
            ApplicationId applicationId = ApplicationId.Generate();
            IEnumerable<RoleId> roles = rolePermissions.Count() == 0
                ? Enumerable.Empty<RoleId>()
                : new RoleId[] { roleId };
            var user = new User(
                UserId.Generate(),
                new EmailAddress("example@example.com"),
                HashedPassword.Hash(new Password("myexamplepassword")),
                roles,
                userPermissions);
            var application = new Application(
                ApplicationId.Generate(),
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(SecretKey.Generate()),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(user));
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            rolesRepositoryMock
                .Setup(r => r.GetAsync(roleId))
                .Returns(Task.FromResult(role));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                usersRepository: usersRepository,
                rolesRepository: rolesRepository,
                applicationsRepository: applicationsRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(
                async () => await authorizationService.GenerateAuthorizationCode(
                    applicationId,
                    new Url("http://example.com/1"),
                    requestedPermissions));

            Assert.That(exception, Is.InstanceOf<ArgumentException>()
                .And.Message
                .EqualTo("One or more permissions are incorrect for given application."));
        }

        [TestCaseSource(nameof(CorrectPermissionsTestData))]
        public async Task TestGenerateAuthorizationCode_WhenGivenPermissionsAreCompatibleWithUserPermissions_ThenCodeIsReturnedAndAuhtorizationCodeIsPersisted(
            PermissionId[] userPermissions,
            PermissionId[] rolePermissions,
            PermissionId[] requestedPermissions)
        {
            RoleId roleId = RoleId.Generate();
            Role role = new Role(roleId, "MyRole", "My role description.", rolePermissions);
            IEnumerable<RoleId> roles = rolePermissions.Count() == 0
                ? Enumerable.Empty<RoleId>()
                : new RoleId[] { roleId };
            ApplicationId applicationId = ApplicationId.Generate();
            var user = new User(
                UserId.Generate(),
                new EmailAddress("example@example.com"),
                HashedPassword.Hash(new Password("myexamplepassword")),
                roles,
                userPermissions);
            var application = new Application(
                ApplicationId.Generate(),
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(SecretKey.Generate()),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var usersRepositoryMock = new Mock<IUsersRepository>();
            usersRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(user));
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            rolesRepositoryMock
                .Setup(r => r.GetAsync(roleId))
                .Returns(Task.FromResult(role));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                usersRepository: usersRepository,
                rolesRepository: rolesRepository,
                applicationsRepository: applicationsRepository,
                authorizationCodesRepository: authorizationCodesRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            Code authorizationCode = await authorizationService.GenerateAuthorizationCode(
                applicationId,
                new Url("http://example.com/1"),
                requestedPermissions);

            Assert.That(authorizationCode, Is.Not.Null);
            authorizationCodesRepositoryMock.Verify(r => r.AddAsync(It.IsAny<AuthorizationCode>()), Times.Once);
        }

        [Test]
        public void TestGenerateTokens_WhenNullApplicationIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult((Application)null));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(applicationsRepository: applicationsRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(
                async () => await authorizationService.GenerateTokens(
                    null,
                    SecretKey.Generate(),
                    new Url("http://example.com/1"),
                    Code.Generate()));

            Assert.That(exception, Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("applicationId"));
        }

        [Test]
        public void TestGenerateTokens_WhenApplicationWasNotFoundInRepository_ThenApplicationNotFoundExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult((Application)null));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(applicationsRepository: applicationsRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ApplicationNotFoundException exception = Assert.ThrowsAsync<ApplicationNotFoundException>(
                async () => await authorizationService.GenerateTokens(
                    applicationId,
                    SecretKey.Generate(),
                    new Url("http://example.com/1"),
                    Code.Generate()));

            Assert.That(exception, Is.InstanceOf<ApplicationNotFoundException>()
                .And.Message
                .EqualTo($"Application {applicationId} not found."));
        }

        [Test]
        public void TestGenerateTokens_WhenNullSecretKeyGiven_ThenArgumentNullExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            var application = new Application(
                ApplicationId.Generate(),
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(applicationsRepository: applicationsRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(
                async () => await authorizationService.GenerateTokens(
                    applicationId,
                    null,
                    new Url("http://example.com/1"),
                    Code.Generate()));

            Assert.That(exception, Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("secretKey"));
        }

        [Test]
        public void TestGenerateTokens_WhenWrongSecretKeyGiven_ThenArgumentExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            var application = new Application(
                ApplicationId.Generate(),
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(SecretKey.Generate()),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult((Application)application));
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult((AuthorizationCode)null));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                applicationsRepository: applicationsRepository,
                authorizationCodesRepository: authorizationCodesRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(
                async () => await authorizationService.GenerateTokens(
                    applicationId,
                    SecretKey.Generate(),
                    new Url("http://example.com/1"),
                    Code.Generate()));

            Assert.That(exception, Is.InstanceOf<ArgumentException>()
                .And.Message
                .EqualTo("Wrong secret key given."));
        }

        [Test]
        public void TestGenerateTokens_WhenNullCallbackUrlGiven_ThenArgumentNullExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            var application = new Application(
                ApplicationId.Generate(),
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var authorizationCode = new AuthorizationCode(
                id: new AuthorizationCodeId(HashedCode.Hash(code), application.Id),
                permissions: new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource"), "Add")
                });
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult((Application)application));
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult(authorizationCode));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                applicationsRepository: applicationsRepository,
                authorizationCodesRepository: authorizationCodesRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(
                async () => await authorizationService.GenerateTokens(
                    applicationId,
                    secretKey,
                    null,
                    Code.Generate()));

            Assert.That(exception, Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("callbackUrl"));
        }

        [Test]
        public void TestGenerateTokens_WhenApplicationCallbackUrlIsNotSameAsRequested_ThenArgumentExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            var application = new Application(
                ApplicationId.Generate(),
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var authorizationCode = new AuthorizationCode(
                id: new AuthorizationCodeId(HashedCode.Hash(code), application.Id),
                permissions: new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource"), "Add")
                });
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult(authorizationCode));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                applicationsRepository: applicationsRepository,
                authorizationCodesRepository: authorizationCodesRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(
                async () => await authorizationService.GenerateTokens(
                    applicationId,
                    secretKey,
                    new Url("http://example.com/2"),
                    Code.Generate()));

            Assert.That(exception, Is.InstanceOf<ArgumentException>()
                .And.Message
                .EqualTo("Wrong callback url given."));
        }

        [Test]
        public void TestGenerateTokens_WhenNullCodeGiven_ThenArgumentNullExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            var application = new Application(
                ApplicationId.Generate(),
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                applicationsRepository: applicationsRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(
                async () => await authorizationService.GenerateTokens(
                    applicationId,
                    secretKey,
                    new Url("http://example.com/1"),
                    null));

            Assert.That(exception, Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("code"));
        }

        [Test]
        public void TestGenerateTokens_WhenAuthorizationCodeWasNotFoundInRepository_ThenAuthorizationCodeNotFoundExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            var application = new Application(
                ApplicationId.Generate(),
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult((Application)application));
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult((AuthorizationCode)null));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                applicationsRepository: applicationsRepository,
                authorizationCodesRepository: authorizationCodesRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            AuthorizationCodeNotFoundException exception = Assert.ThrowsAsync<AuthorizationCodeNotFoundException>(
                async () => await authorizationService.GenerateTokens(
                    applicationId,
                    secretKey,
                    new Url("http://example.com/1"),
                    Code.Generate()));

            Assert.That(exception, Is.InstanceOf<AuthorizationCodeNotFoundException>());
        }

        [Test]
        public void TestGenerateTokens_WhenAuthorizationCodeExpired_ThenInvalidOperationIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            var application = new Application(
                ApplicationId.Generate(),
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var authorizationCode = new AuthorizationCode(
                id: new AuthorizationCodeId(HashedCode.Hash(code), application.Id),
                DateTime.Now.AddMinutes(-2),
                false,
                permissions: new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource"), "Add")
                });
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult(authorizationCode));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                applicationsRepository: applicationsRepository,
                authorizationCodesRepository: authorizationCodesRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            InvalidOperationException exception = Assert.ThrowsAsync<InvalidOperationException>(
                async () => await authorizationService.GenerateTokens(
                    applicationId,
                    secretKey,
                    new Url("http://example.com/1"),
                    code));

            Assert.That(exception, Is.InstanceOf<InvalidOperationException>()
                .And.Message
                .EqualTo("Authorization code has expired."));
        }

        [Test]
        public void TestGenerateTokens_WhenAuthorizationCodeWasUsed_ThenInvalidOperationIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            var application = new Application(
                ApplicationId.Generate(),
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var authorizationCode = new AuthorizationCode(
                id: new AuthorizationCodeId(HashedCode.Hash(code), application.Id),
                DateTime.Now.AddMinutes(2),
                true,
                permissions: new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource"), "Add")
                });
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult(authorizationCode));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                applicationsRepository: applicationsRepository,
                authorizationCodesRepository: authorizationCodesRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            InvalidOperationException exception = Assert.ThrowsAsync<InvalidOperationException>(
                async () => await authorizationService.GenerateTokens(
                    applicationId,
                    secretKey,
                    new Url("http://example.com/1"),
                    code));

            Assert.That(exception, Is.InstanceOf<InvalidOperationException>()
                .And.Message
                .EqualTo("Authorization code was used."));
        }

        [Test]
        public async Task TestGenerateTokens_WhenGenerating_ThenTokenPairIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            var application = new Application(
                ApplicationId.Generate(),
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var authorizationCode = new AuthorizationCode(
                id: new AuthorizationCodeId(HashedCode.Hash(code), application.Id),
                permissions: new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource"), "Add")
                });
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult(authorizationCode));
            var refreshTokensMock = new Mock<IRefreshTokensRepository>();
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensMock.Object;
            var transactionScopeMock = new Mock<ITransactionScope>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.ApplicationsRepository).Returns(applicationsRepository);
            unitOfWorkMock.Setup(u => u.AuthorizationCodesRepository).Returns(authorizationCodesRepository);
            unitOfWorkMock.Setup(u => u.RefreshTokensRepository).Returns(refreshTokensRepository);
            unitOfWorkMock.Setup(u => u.BeginScope()).Returns(transactionScopeMock.Object);
            IUnitOfWork unitOfWork = unitOfWorkMock.Object;
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            TokenPair tokenPair = await authorizationService.GenerateTokens(
                applicationId,
                secretKey,
                new Url("http://example.com/1"),
                code);

            Assert.That(tokenPair, Is.Not.Null);
        }

        [Test]
        public async Task TestGenerateTokens_WhenGenerating_ThenAuthorizationCodeIsMarkedAsUsedAndStored()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            var application = new Application(
                ApplicationId.Generate(),
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var authorizationCode = new AuthorizationCode(
                id: new AuthorizationCodeId(HashedCode.Hash(code), application.Id),
                permissions: new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource"), "Add")
                });
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult(authorizationCode));
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            var transactionScopeMock = new Mock<ITransactionScope>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.ApplicationsRepository).Returns(applicationsRepository);
            unitOfWorkMock.Setup(u => u.AuthorizationCodesRepository).Returns(authorizationCodesRepository);
            unitOfWorkMock.Setup(u => u.RefreshTokensRepository).Returns(refreshTokensRepository);
            unitOfWorkMock.Setup(u => u.BeginScope()).Returns(transactionScopeMock.Object);
            var unitOfWork = unitOfWorkMock.Object;
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            TokenPair tokenPair = await authorizationService.GenerateTokens(
                applicationId,
                secretKey,
                new Url("http://example.com/1"),
                code);

            Assert.That(authorizationCode.Used, Is.True);
            authorizationCodesRepositoryMock.Verify(a => a.UpdateAsync(It.IsAny<AuthorizationCode>()), Times.Once());
        }

        [Test]
        public async Task TestGenerateTokens_WhenGenerating_ThenRefreshTokenIsStored()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            var application = new Application(
                ApplicationId.Generate(),
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var authorizationCode = new AuthorizationCode(
                id: new AuthorizationCodeId(HashedCode.Hash(code), application.Id),
                permissions: new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource"), "Add")
                });
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult(authorizationCode));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            var transactionScopeMock = new Mock<ITransactionScope>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.ApplicationsRepository).Returns(applicationsRepository);
            unitOfWorkMock.Setup(u => u.AuthorizationCodesRepository).Returns(authorizationCodesRepository);
            unitOfWorkMock.Setup(u => u.RefreshTokensRepository).Returns(refreshTokensRepository);
            unitOfWorkMock.Setup(u => u.BeginScope()).Returns(transactionScopeMock.Object);
            var unitOfWork = unitOfWorkMock.Object;
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            TokenPair tokenPair = await authorizationService.GenerateTokens(
                applicationId,
                secretKey,
                new Url("http://example.com/1"),
                code);

            refreshTokensRepositoryMock.Verify(a => a.AddAsync(It.IsAny<RefreshToken>()), Times.Once());
        }

        [Test]
        public async Task TestGenerateTokens_WhenAddRefreshTokenThrowsException_ThenAuthorizationCodeIsntUpdated()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            var application = new Application(
                ApplicationId.Generate(),
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var authorizationCode = new AuthorizationCode(
                id: new AuthorizationCodeId(HashedCode.Hash(code), application.Id),
                permissions: new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource"), "Add")
                });
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            refreshTokensRepositoryMock.Setup(r => r.AddAsync(It.IsAny<RefreshToken>())).Throws(new Exception());
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult(authorizationCode));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            var transactionScopeMock = new Mock<ITransactionScope>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.ApplicationsRepository).Returns(applicationsRepository);
            unitOfWorkMock.Setup(u => u.AuthorizationCodesRepository).Returns(authorizationCodesRepository);
            unitOfWorkMock.Setup(u => u.RefreshTokensRepository).Returns(refreshTokensRepository);
            unitOfWorkMock.Setup(u => u.BeginScope()).Returns(transactionScopeMock.Object);
            var unitOfWork = unitOfWorkMock.Object;
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            try
            {
                TokenPair tokenPair = await authorizationService.GenerateTokens(
                    applicationId,
                    secretKey,
                    new Url("http://example.com/1"),
                    code);
            }
            catch(Exception)
            {
            }

            transactionScopeMock.Verify(t => t.Complete(), Times.Never);
        }

        [Test]
        public async Task TestGenerateTokens_WhenGenerating_ThenTransactionIsCompleted()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            var application = new Application(
                ApplicationId.Generate(),
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var authorizationCode = new AuthorizationCode(
                id: new AuthorizationCodeId(HashedCode.Hash(code), application.Id),
                permissions: new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyResource"), "Add")
                });
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult(authorizationCode));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            var transactionScopeMock = new Mock<ITransactionScope>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.ApplicationsRepository).Returns(applicationsRepository);
            unitOfWorkMock.Setup(u => u.AuthorizationCodesRepository).Returns(authorizationCodesRepository);
            unitOfWorkMock.Setup(u => u.RefreshTokensRepository).Returns(refreshTokensRepository);
            unitOfWorkMock.Setup(u => u.BeginScope()).Returns(transactionScopeMock.Object);
            var unitOfWork = unitOfWorkMock.Object;
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            TokenPair tokenPair = await authorizationService.GenerateTokens(
                applicationId,
                secretKey,
                new Url("http://example.com/1"),
                code);

            transactionScopeMock.Verify(a => a.Complete(), Times.Once());
        }

        [Test]
        public void TestRefreshTokens_WhenNullRefreshTokenValueGiven_ThenArgumentNullExceptionIsThrown()
        {
            IUnitOfWork unitOfWork = this.GetUnitOfWork();
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(
                async () => await authorizationService.RefreshTokens(null, new Url("http://example.com/1")));

            Assert.That(exception, Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("refreshTokenValue"));
        }

        [Test]
        public void TestRefreshTokens_WhenNullCallbackUrlGiven_ThenArgumentNullExceptionIsThrown()
        {
            IUnitOfWork unitOfWork = this.GetUnitOfWork();
            TokenValue refreshTokenValue = TokenValue.GenerateRefreshToken(
                ApplicationId.Generate(), 
                new PermissionId[] 
                { 
                    new PermissionId(new ResourceId("MyRes"), "Add")
                },
                DateTime.Now.AddDays(1));
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(
                async () => await authorizationService.RefreshTokens(refreshTokenValue, null));

            Assert.That(exception, Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("callbackUrl"));
        }

        [Test]
        public void TestRefreshTokens_WhenApplicationWasNotFoundInRepository_ThenApplicationNotFoundExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            TokenValue refreshTokenValue = TokenValue.GenerateRefreshToken(
                applicationId,
                new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyRes"), "Add")
                },
                DateTime.Now.AddDays(1));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult((Application)null));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(applicationsRepository: applicationsRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ApplicationNotFoundException exception = Assert.ThrowsAsync<ApplicationNotFoundException>(
                async () => await authorizationService.RefreshTokens(refreshTokenValue, new Url("http://example.com/1")));

            Assert.That(exception, Is.InstanceOf<ApplicationNotFoundException>()
                .And.Message
                .EqualTo($"Application {applicationId} not found."));
        }

        [Test]
        public void TestRefreshTokens_WhenApplicationCallbackUrlIsNotSameAsRequested_ThenArgumentExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            var application = new Application(
                ApplicationId.Generate(),
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            TokenValue refreshTokenValue = TokenValue.GenerateRefreshToken(
                applicationId,
                new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyRes"), "Add")
                },
                DateTime.Now.AddDays(1));
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                applicationsRepository: applicationsRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(
                async () => await authorizationService.RefreshTokens(refreshTokenValue, new Url("http://example.com/2")));

            Assert.That(exception, Is.InstanceOf<ArgumentException>()
                .And.Message
                .EqualTo("Wrong callback url given."));
        }

        [Test]
        public void TestRefreshTokens_WhenRefreshTokenNotFound_ThenRefreshTokenNotFoundExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            var application = new Application(
                applicationId,
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            refreshTokensRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<TokenId>()))
                .Returns(Task.FromResult<RefreshToken>(null));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            TokenValue refreshTokenValue = TokenValue.GenerateRefreshToken(
                applicationId,
                new PermissionId[]
                {
                    new PermissionId(new ResourceId("MyRes"), "Add")
                },
                DateTime.Now.AddDays(1));
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                applicationsRepository: applicationsRepository,
                refreshTokensRepository: refreshTokensRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            RefreshTokenNotFoundException exception = Assert.ThrowsAsync<RefreshTokenNotFoundException>(
                async () => await authorizationService.RefreshTokens(refreshTokenValue, new Url("http://example.com/1")));
            
            Assert.That(exception, Is.InstanceOf<RefreshTokenNotFoundException>()
                .And.Message
                .EqualTo($"Refresh token {new TokenId(TokenValueEncrypter.Encrypt(refreshTokenValue))} not found."));
        }

        [Test]
        public void TestRefreshTokens_WhenRefreshTokenFoundAndHasBeenUsed_ThenInvalidTokenExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            var application = new Application(
                applicationId,
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application)); 
            TokenValue refreshTokenValue = TokenValue.GenerateRefreshToken(
                 applicationId,
                 new PermissionId[]
                 {
                    new PermissionId(new ResourceId("MyRes"), "Add")
                 },
                 DateTime.Now.AddDays(1));
            var refreshToken = new RefreshToken(
                new TokenId(TokenValueEncrypter.Encrypt(refreshTokenValue)),
                true);
            refreshTokensRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<TokenId>()))
                .Returns(Task.FromResult(refreshToken));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                applicationsRepository: applicationsRepository,
                refreshTokensRepository: refreshTokensRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            InvalidTokenException exception = Assert.ThrowsAsync<InvalidTokenException>(
                async () => await authorizationService.RefreshTokens(refreshTokenValue, new Url("http://example.com/1")));

            Assert.That(exception, Is.InstanceOf<InvalidTokenException>()
                .And.Message
                .EqualTo("Token was used before."));
        }

        [Test]
        public void TestRefreshTokens_WhenRefreshTokenFoundAndExpired_ThenInvalidTokenExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            var application = new Application(
                applicationId,
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            TokenValue refreshTokenValue = TokenValue.GenerateRefreshToken(
                 applicationId,
                 new PermissionId[]
                 {
                    new PermissionId(new ResourceId("MyRes"), "Add")
                 },
                 DateTime.Now.AddDays(-1));
            var refreshToken = new RefreshToken(
                new TokenId(TokenValueEncrypter.Encrypt(refreshTokenValue)),
                false);
            refreshTokensRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<TokenId>()))
                .Returns(Task.FromResult(refreshToken));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            IUnitOfWork unitOfWork = this.GetUnitOfWork(
                applicationsRepository: applicationsRepository,
                refreshTokensRepository: refreshTokensRepository);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWork);

            InvalidTokenException exception = Assert.ThrowsAsync<InvalidTokenException>(
                async () => await authorizationService.RefreshTokens(refreshTokenValue, new Url("http://example.com/1")));

            Assert.That(exception, Is.InstanceOf<InvalidTokenException>()
                .And.Message
                .EqualTo("Token has expired."));
        }

        [Test]
        public async Task TestRefreshTokens_WhenRefreshTokenFoundAndVerifiedSucessful_ThenTokenPairIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            var application = new Application(
                applicationId,
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            TokenValue refreshTokenValue = TokenValue.GenerateRefreshToken(
                 applicationId,
                 new PermissionId[]
                 {
                    new PermissionId(new ResourceId("MyRes"), "Add")
                 },
                 DateTime.Now.AddDays(1));
            var refreshToken = new RefreshToken(
                new TokenId(TokenValueEncrypter.Encrypt(refreshTokenValue)),
                false);
            refreshTokensRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<TokenId>()))
                .Returns(Task.FromResult(refreshToken));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            Mock<ITransactionScope> transactionScopeMock = new Mock<ITransactionScope>();
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.ApplicationsRepository).Returns(applicationsRepository);
            unitOfWorkMock.Setup(u => u.RefreshTokensRepository).Returns(refreshTokensRepository);
            unitOfWorkMock.Setup(u => u.BeginScope()).Returns(transactionScopeMock.Object);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWorkMock.Object);

            TokenPair tokenPair = await authorizationService.RefreshTokens(
                refreshTokenValue, 
                new Url("http://example.com/1"));

            Assert.Multiple(() =>
            {
                Assert.That(tokenPair.AccessToken, Is.Not.Null);
                Assert.That(tokenPair.RefreshToken, Is.Not.EqualTo(refreshTokenValue));
                Assert.That(tokenPair.RefreshToken, Is.Not.Null);
            });
        }

        [Test]
        public async Task TestRefreshTokens_WhenRefreshTokenFoundAndVerifiedSucessful_ThenRefreshTokenIsMarkedAsUsed()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            var application = new Application(
                applicationId,
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            TokenValue refreshTokenValue = TokenValue.GenerateRefreshToken(
                 applicationId,
                 new PermissionId[]
                 {
                    new PermissionId(new ResourceId("MyRes"), "Add")
                 },
                 DateTime.Now.AddDays(1));
            var refreshToken = new RefreshToken(
                new TokenId(TokenValueEncrypter.Encrypt(refreshTokenValue)),
                false);
            refreshTokensRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<TokenId>()))
                .Returns(Task.FromResult(refreshToken));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            Mock<ITransactionScope> transactionScopeMock = new Mock<ITransactionScope>();
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.ApplicationsRepository).Returns(applicationsRepository);
            unitOfWorkMock.Setup(u => u.RefreshTokensRepository).Returns(refreshTokensRepository);
            unitOfWorkMock.Setup(u => u.BeginScope()).Returns(transactionScopeMock.Object);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWorkMock.Object);

            TokenPair tokenPair = await authorizationService.RefreshTokens(
                refreshTokenValue,
                new Url("http://example.com/1"));

            Assert.That(refreshToken.Used, Is.True);
        }

        [Test]
        public async Task TestRefreshTokens_WhenRefreshTokenFoundAndVerifiedSucessful_ThenRefreshTokenIsUpdated()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            var application = new Application(
                applicationId,
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            TokenValue refreshTokenValue = TokenValue.GenerateRefreshToken(
                 applicationId,
                 new PermissionId[]
                 {
                    new PermissionId(new ResourceId("MyRes"), "Add")
                 },
                 DateTime.Now.AddDays(1));
            var refreshToken = new RefreshToken(
                new TokenId(TokenValueEncrypter.Encrypt(refreshTokenValue)),
                false);
            refreshTokensRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<TokenId>()))
                .Returns(Task.FromResult(refreshToken));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            Mock<ITransactionScope> transactionScopeMock = new Mock<ITransactionScope>();
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.ApplicationsRepository).Returns(applicationsRepository);
            unitOfWorkMock.Setup(u => u.RefreshTokensRepository).Returns(refreshTokensRepository);
            unitOfWorkMock.Setup(u => u.BeginScope()).Returns(transactionScopeMock.Object);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWorkMock.Object);

            TokenPair tokenPair = await authorizationService.RefreshTokens(
                refreshTokenValue,
                new Url("http://example.com/1"));

            refreshTokensRepositoryMock.Verify(r => r.UpdateAsync(refreshToken), Times.Once);
        }

        [Test]
        public async Task TestRefreshTokens_WhenRefreshTokenFoundAndVerifiedSucessful_ThenNewRefreshTokenIsAdded()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            var application = new Application(
                applicationId,
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            TokenValue refreshTokenValue = TokenValue.GenerateRefreshToken(
                 applicationId,
                 new PermissionId[]
                 {
                    new PermissionId(new ResourceId("MyRes"), "Add")
                 },
                 DateTime.Now.AddDays(1));
            var refreshToken = new RefreshToken(
                new TokenId(TokenValueEncrypter.Encrypt(refreshTokenValue)),
                false);
            refreshTokensRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<TokenId>()))
                .Returns(Task.FromResult(refreshToken));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            Mock<ITransactionScope> transactionScopeMock = new Mock<ITransactionScope>();
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.ApplicationsRepository).Returns(applicationsRepository);
            unitOfWorkMock.Setup(u => u.RefreshTokensRepository).Returns(refreshTokensRepository);
            unitOfWorkMock.Setup(u => u.BeginScope()).Returns(transactionScopeMock.Object);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWorkMock.Object);

            TokenPair tokenPair = await authorizationService.RefreshTokens(
                refreshTokenValue,
                new Url("http://example.com/1"));

            var newRefreshToken = new RefreshToken(
                new TokenId(TokenValueEncrypter.Encrypt(tokenPair.RefreshToken)),
                false);

            refreshTokensRepositoryMock.Verify(r => r.AddAsync(newRefreshToken), Times.Once);
        }

        [Test]
        public async Task TestRefreshTokens_WhenUpdateRefreshTokenThrowsException_ThenTransactionIsntCompleted()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            var application = new Application(
                applicationId,
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            TokenValue refreshTokenValue = TokenValue.GenerateRefreshToken(
                 applicationId,
                 new PermissionId[]
                 {
                    new PermissionId(new ResourceId("MyRes"), "Add")
                 },
                 DateTime.Now.AddDays(1));
            var refreshToken = new RefreshToken(
                new TokenId(TokenValueEncrypter.Encrypt(refreshTokenValue)),
                false);
            refreshTokensRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<TokenId>()))
                .Returns(Task.FromResult(refreshToken));
            refreshTokensRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<RefreshToken>())).Throws(new Exception());
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            Mock<ITransactionScope> transactionScopeMock = new Mock<ITransactionScope>();
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.ApplicationsRepository).Returns(applicationsRepository);
            unitOfWorkMock.Setup(u => u.RefreshTokensRepository).Returns(refreshTokensRepository);
            unitOfWorkMock.Setup(u => u.BeginScope()).Returns(transactionScopeMock.Object);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWorkMock.Object);

            try
            {
                TokenPair tokenPair = await authorizationService.RefreshTokens(
                    refreshTokenValue,
                    new Url("http://example.com/1"));
            }
            catch(Exception)
            {
            }

            transactionScopeMock.Verify(t => t.Complete(), Times.Never);
        }

        [Test]
        public async Task TestRefreshTokens_WhenRefreshTokenFoundAndVerifiedSucessful_ThenTransactionIsCompleted()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            var application = new Application(
                applicationId,
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(secretKey),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var refreshTokensRepositoryMock = new Mock<IRefreshTokensRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            TokenValue refreshTokenValue = TokenValue.GenerateRefreshToken(
                 applicationId,
                 new PermissionId[]
                 {
                    new PermissionId(new ResourceId("MyRes"), "Add")
                 },
                 DateTime.Now.AddDays(1));
            var refreshToken = new RefreshToken(
                new TokenId(TokenValueEncrypter.Encrypt(refreshTokenValue)),
                false);
            refreshTokensRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<TokenId>()))
                .Returns(Task.FromResult(refreshToken));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            Mock<ITransactionScope> transactionScopeMock = new Mock<ITransactionScope>();
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.ApplicationsRepository).Returns(applicationsRepository);
            unitOfWorkMock.Setup(u => u.RefreshTokensRepository).Returns(refreshTokensRepository);
            unitOfWorkMock.Setup(u => u.BeginScope()).Returns(transactionScopeMock.Object);
            AuthorizationService authorizationService = new AuthorizationService(unitOfWorkMock.Object);

            try
            {
                TokenPair tokenPair = await authorizationService.RefreshTokens(
                    refreshTokenValue,
                    new Url("http://example.com/1"));
            }
            catch(Exception)
            {
            }

            transactionScopeMock.Verify(t => t.Complete(), Times.Once);
        }
    }
}