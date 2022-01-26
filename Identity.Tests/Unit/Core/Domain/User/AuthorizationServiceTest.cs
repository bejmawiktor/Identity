using DDD.Domain.Persistence;
using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Domain.Builders;
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
            IUnitOfWork unitOfWork = UnitOfWorkBuilder.DefaultUnitOfWork;

            AuthorizationService authorizationService = new(unitOfWork);

            Assert.That(authorizationService.UnitOfWork, Is.EqualTo(unitOfWork));
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
            AuthorizationService authorizationService = new(UnitOfWorkBuilder.DefaultUnitOfWork);

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
            AuthorizationService authorizationService = new(UnitOfWorkBuilder.DefaultUnitOfWork);

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
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock.Setup(u => u.GetAsync(It.IsAny<UserId>())).Returns(Task.FromResult((User)null));
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            UserId userId = UserId.Generate();
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUsersRepository(usersRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

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
            Mock<IUsersRepository> usersRepositoryMock = new();
            Mock<IRolesRepository> rolesRepositoryMock = new();
            RoleId permittedRoleId = RoleId.Generate();
            RoleId notPermittedRoleId = RoleId.Generate();
            User user = new UserBuilder()
                .WithRoles(
                    new RoleId[]
                    {
                        notPermittedRoleId,
                        permittedRoleId,
                    })
                .Build();
            PermissionId permissionId = new(new ResourceId("MyResource"), "MyPermission");
            Role permittedRole = new RoleBuilder()
                .WithId(permittedRoleId)
                .WithPermissions(
                    new PermissionId[]
                    {
                        permissionId
                    })
                .Build();
            Role notPermittedRole = new RoleBuilder()
                .WithId(notPermittedRoleId)
                .Build();
            usersRepositoryMock.Setup(u => u.GetAsync(It.IsAny<UserId>())).Returns(Task.FromResult(user));
            rolesRepositoryMock.Setup(u => u.GetAsync(permittedRoleId)).Returns(Task.FromResult(permittedRole));
            rolesRepositoryMock.Setup(u => u.GetAsync(notPermittedRoleId)).Returns(Task.FromResult(notPermittedRole));
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUsersRepository(usersRepository)
                .WithRolesRepository(rolesRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

            bool userIsPermitted = await authorizationService.CheckUserAccess(user.Id, permissionId);

            Assert.That(userIsPermitted, Is.True);
        }

        [Test]
        public async Task TestCheckUserAccess_WhenUserPermittedBySinglePermission_ThenTrueIsReturned()
        {
            Mock<IUsersRepository> usersRepositoryMock = new();
            PermissionId permissionId = new(new ResourceId("MyResource"), "MyPermission");
            User user = new UserBuilder()
                .WithPermissions(
                    new PermissionId[]
                    {
                        permissionId,
                        new PermissionId(new ResourceId("MyResource"), "MyPermission2")
                    })
                .Build();
            usersRepositoryMock.Setup(u => u.GetAsync(It.IsAny<UserId>())).Returns(Task.FromResult(user));
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUsersRepository(usersRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

            bool userIsPermitted = await authorizationService.CheckUserAccess(user.Id, permissionId);

            Assert.That(userIsPermitted, Is.True);
        }

        [Test]
        public async Task TestCheckUserAccess_WhenUserPermittedFromRoleAndSinglePermission_ThenTrueIsReturned()
        {
            Mock<IUsersRepository> usersRepositoryMock = new();
            Mock<IRolesRepository> rolesRepositoryMock = new();
            PermissionId permissionId = new(new ResourceId("MyResource"), "MyPermission");
            User user = new UserBuilder()
                .WithPermissions(
                    new PermissionId[]
                    {
                        permissionId
                    })
                .WithRoles(
                    new RoleId[]
                    {
                        RoleId.Generate()
                    })
                .Build();
            Role role = new RoleBuilder()
                .WithPermissions(
                    new PermissionId[]
                    {
                        permissionId
                    })
                .Build();
            usersRepositoryMock.Setup(u => u.GetAsync(It.IsAny<UserId>())).Returns(Task.FromResult(user));
            rolesRepositoryMock.Setup(u => u.GetAsync(It.IsAny<RoleId>())).Returns(Task.FromResult(role));
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUsersRepository(usersRepository)
                .WithRolesRepository(rolesRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

            bool userIsPermitted = await authorizationService.CheckUserAccess(user.Id, permissionId);

            Assert.That(userIsPermitted, Is.True);
        }

        [Test]
        public async Task TestCheckUserAccess_WhenUserIsntPermittedFromRoleAndSinglePermission_ThenFalseIsReturned()
        {
            PermissionId permissionId = new(new ResourceId("MyResource"), "MyPermission");
            User user = UserBuilder.DefaultUser;
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock.Setup(u => u.GetAsync(It.IsAny<UserId>())).Returns(Task.FromResult(user));
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUsersRepository(usersRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

            bool userIsPermitted = await authorizationService.CheckUserAccess(user.Id, permissionId);

            Assert.That(userIsPermitted, Is.False);
        }

        [Test]
        public void TestGenerateAuthorizationCode_WhenNullApplicationIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            PermissionId[] permissions = new PermissionId[] { new PermissionId(new ResourceId("MyResource"), "Add") };
            AuthorizationService authorizationService = new(UnitOfWorkBuilder.DefaultUnitOfWork);

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
            PermissionId[] permissions = new PermissionId[] { new PermissionId(new ResourceId("MyResource"), "Add") };
            AuthorizationService authorizationService = new(UnitOfWorkBuilder.DefaultUnitOfWork);

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
            PermissionId[] permissions = new PermissionId[] { new PermissionId(new ResourceId("MyResource"), "Add") };
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult((Application)null));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

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
            PermissionId[] permissions = new PermissionId[] { new PermissionId(new ResourceId("MyResource"), "Add") };
            Application application = new(
                ApplicationId.Generate(),
                UserId.Generate(),
                "MyApp1",
                EncryptedSecretKey.Encrypt(SecretKey.Generate()),
                new Url("http://example.com"),
                new Url("http://example.com/1"));
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

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
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
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
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

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
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
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
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

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
            User user = new UserBuilder()
                .WithRoles(roles)
                .WithPermissions(userPermissions)
                .Build();
            Application application = ApplicationBuilder.DefaultApplication;
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(user));
            Mock<IRolesRepository> rolesRepositoryMock = new();
            rolesRepositoryMock
                .Setup(r => r.GetAsync(roleId))
                .Returns(Task.FromResult(role));
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUsersRepository(usersRepository)
                .WithRolesRepository(rolesRepository)
                .WithApplicationsRepository(applicationsRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(
                async () => await authorizationService.GenerateAuthorizationCode(
                    applicationId,
                    application.CallbackUrl,
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
            User user = new UserBuilder()
                .WithRoles(roles)
                .WithPermissions(userPermissions)
                .Build();
            Application application = ApplicationBuilder.DefaultApplication;
            Mock<IUsersRepository> usersRepositoryMock = new();
            usersRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(user));
            Mock<IRolesRepository> rolesRepositoryMock = new();
            rolesRepositoryMock
                .Setup(r => r.GetAsync(roleId))
                .Returns(Task.FromResult(role));
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            Mock<IAuthorizationCodesRepository> authorizationCodesRepositoryMock = new();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUsersRepository(usersRepository)
                .WithRolesRepository(rolesRepository)
                .WithApplicationsRepository(applicationsRepository)
                .WithAuthorizationCodesRepository(authorizationCodesRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

            Code authorizationCode = await authorizationService.GenerateAuthorizationCode(
                applicationId,
                application.CallbackUrl,
                requestedPermissions);

            Assert.That(authorizationCode, Is.Not.Null);
            authorizationCodesRepositoryMock.Verify(r => r.AddAsync(It.IsAny<AuthorizationCode>()), Times.Once);
        }

        [Test]
        public void TestGenerateTokens_WhenNullApplicationIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult((Application)null));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

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
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult((Application)null));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

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
            Application application = ApplicationBuilder.DefaultApplication;
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

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
            Application application = ApplicationBuilder.DefaultApplication;
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult((Application)application));
            Mock<IAuthorizationCodesRepository> authorizationCodesRepositoryMock = new();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult((AuthorizationCode)null));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepository)
                .WithAuthorizationCodesRepository(authorizationCodesRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

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
            Application application = ApplicationBuilder.DefaultApplication;
            AuthorizationCode authorizationCode = new AuthorizationCodeBuilder()
                .WithId(new AuthorizationCodeId(HashedCode.Hash(code), application.Id))
                .WithPermissions(
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource"), "Add")
                    })
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult((Application)application));
            Mock<IAuthorizationCodesRepository> authorizationCodesRepositoryMock = new();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult(authorizationCode));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepository)
                .WithAuthorizationCodesRepository(authorizationCodesRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(
                async () => await authorizationService.GenerateTokens(
                    applicationId,
                    secretKey,
                    null,
                    Code.Generate()));

            Assert.That(
                exception,
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("callbackUrl"));
        }

        [Test]
        public void TestGenerateTokens_WhenApplicationCallbackUrlIsNotSameAsRequested_ThenArgumentExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            Application application = new ApplicationBuilder()
                .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                .Build();
            AuthorizationCode authorizationCode = new AuthorizationCodeBuilder()
                .WithId(new AuthorizationCodeId(HashedCode.Hash(code), application.Id))
                .WithPermissions(
                    new PermissionId[]
                    {
                        new PermissionId(new ResourceId("MyResource"), "Add")
                    })
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            Mock<IAuthorizationCodesRepository> authorizationCodesRepositoryMock = new();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult(authorizationCode));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepository)
                .WithAuthorizationCodesRepository(authorizationCodesRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

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
            Application application = new ApplicationBuilder()
                .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(
                async () => await authorizationService.GenerateTokens(
                    applicationId,
                    secretKey,
                    application.CallbackUrl,
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
            Application application = new ApplicationBuilder()
                .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult((Application)application));
            Mock<IAuthorizationCodesRepository> authorizationCodesRepositoryMock = new();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult((AuthorizationCode)null));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepository)
                .WithAuthorizationCodesRepository(authorizationCodesRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

            AuthorizationCodeNotFoundException exception = Assert.ThrowsAsync<AuthorizationCodeNotFoundException>(
                async () => await authorizationService.GenerateTokens(
                    applicationId,
                    secretKey,
                    application.CallbackUrl,
                    Code.Generate()));

            Assert.That(exception, Is.InstanceOf<AuthorizationCodeNotFoundException>());
        }

        [Test]
        public void TestGenerateTokens_WhenAuthorizationCodeExpired_ThenInvalidOperationIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            Application application = new ApplicationBuilder()
                .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                .Build();
            AuthorizationCode authorizationCode = new AuthorizationCodeBuilder()
                .WithId(new AuthorizationCodeId(HashedCode.Hash(code), application.Id))
                .WithExpiresAt(DateTime.Now.AddMinutes(-2))
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            Mock<IAuthorizationCodesRepository> authorizationCodesRepositoryMock = new();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult(authorizationCode));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepository)
                .WithAuthorizationCodesRepository(authorizationCodesRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

            InvalidOperationException exception = Assert.ThrowsAsync<InvalidOperationException>(
                async () => await authorizationService.GenerateTokens(
                    applicationId,
                    secretKey,
                    application.CallbackUrl,
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
            Application application = new ApplicationBuilder()
                .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                .Build();
            AuthorizationCode authorizationCode = new AuthorizationCodeBuilder()
                .WithId(new AuthorizationCodeId(HashedCode.Hash(code), application.Id))
                .WithExpiresAt(DateTime.Now.AddMinutes(2))
                .WithUsed(true)
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            Mock<IAuthorizationCodesRepository> authorizationCodesRepositoryMock = new();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult(authorizationCode));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepository)
                .WithAuthorizationCodesRepository(authorizationCodesRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

            InvalidOperationException exception = Assert.ThrowsAsync<InvalidOperationException>(
                async () => await authorizationService.GenerateTokens(
                    applicationId,
                    secretKey,
                    application.CallbackUrl,
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
            Application application = new ApplicationBuilder()
                .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                .Build();
            AuthorizationCode authorizationCode = new AuthorizationCodeBuilder()
                .WithId(new AuthorizationCodeId(HashedCode.Hash(code), application.Id))
                .WithExpiresAt(DateTime.Now.AddDays(1))
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            Mock<IAuthorizationCodesRepository> authorizationCodesRepositoryMock = new();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult(authorizationCode));
            Mock<IRefreshTokensRepository> refreshTokensMock = new();
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensMock.Object;
            Mock<ITransactionScope> transactionScopeMock = new();
            Mock<IUnitOfWork> unitOfWorkMock = new();
            unitOfWorkMock.Setup(u => u.BeginScope()).Returns(transactionScopeMock.Object);
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUnitOfWorkMock(unitOfWorkMock)
                .WithApplicationsRepository(applicationsRepository)
                .WithAuthorizationCodesRepository(authorizationCodesRepository)
                .WithRefreshTokensRepository(refreshTokensRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

            TokenPair tokenPair = await authorizationService.GenerateTokens(
                applicationId,
                secretKey,
                application.CallbackUrl,
                code);

            Assert.That(tokenPair, Is.Not.Null);
        }

        [Test]
        public async Task TestGenerateTokens_WhenGenerating_ThenAuthorizationCodeIsMarkedAsUsedAndStored()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            Application application = new ApplicationBuilder()
                 .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                 .Build();
            AuthorizationCode authorizationCode = new AuthorizationCodeBuilder()
                .WithId(new AuthorizationCodeId(HashedCode.Hash(code), application.Id))
                .WithExpiresAt(DateTime.Now.AddDays(1))
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            Mock<IAuthorizationCodesRepository> authorizationCodesRepositoryMock = new();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult(authorizationCode));
            Mock<IRefreshTokensRepository> refreshTokensRepositoryMock = new();
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            Mock<ITransactionScope> transactionScopeMock = new();
            Mock<IUnitOfWork> unitOfWorkMock = new();
            unitOfWorkMock.Setup(u => u.BeginScope()).Returns(transactionScopeMock.Object);
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUnitOfWorkMock(unitOfWorkMock)
                .WithApplicationsRepository(applicationsRepository)
                .WithAuthorizationCodesRepository(authorizationCodesRepository)
                .WithRefreshTokensRepository(refreshTokensRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

            TokenPair tokenPair = await authorizationService.GenerateTokens(
                applicationId,
                secretKey,
                application.CallbackUrl,
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
            Application application = new ApplicationBuilder()
                .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                .Build();
            AuthorizationCode authorizationCode = new AuthorizationCodeBuilder()
                .WithId(new AuthorizationCodeId(HashedCode.Hash(code), application.Id))
                .WithExpiresAt(DateTime.Now.AddDays(1))
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            Mock<IAuthorizationCodesRepository> authorizationCodesRepositoryMock = new();
            Mock<IRefreshTokensRepository> refreshTokensRepositoryMock = new();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult(authorizationCode));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            Mock<ITransactionScope> transactionScopeMock = new();
            Mock<IUnitOfWork> unitOfWorkMock = new();
            unitOfWorkMock.Setup(u => u.BeginScope()).Returns(transactionScopeMock.Object);
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUnitOfWorkMock(unitOfWorkMock)
                .WithApplicationsRepository(applicationsRepository)
                .WithAuthorizationCodesRepository(authorizationCodesRepository)
                .WithRefreshTokensRepository(refreshTokensRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

            TokenPair tokenPair = await authorizationService.GenerateTokens(
                applicationId,
                secretKey,
                application.CallbackUrl,
                code);

            refreshTokensRepositoryMock.Verify(a => a.AddAsync(It.IsAny<RefreshToken>()), Times.Once());
        }

        [Test]
        public async Task TestGenerateTokens_WhenAddRefreshTokenThrowsException_ThenAuthorizationCodeIsntUpdated()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Code code = Code.Generate();
            Application application = new ApplicationBuilder()
                .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                .Build();
            AuthorizationCode authorizationCode = new AuthorizationCodeBuilder()
                .WithId(new AuthorizationCodeId(HashedCode.Hash(code), application.Id))
                .WithExpiresAt(DateTime.Now.AddDays(1))
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            Mock<IAuthorizationCodesRepository> authorizationCodesRepositoryMock = new();
            Mock<IRefreshTokensRepository> refreshTokensRepositoryMock = new();
            refreshTokensRepositoryMock.Setup(r => r.AddAsync(It.IsAny<RefreshToken>())).Throws(new Exception());
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult(authorizationCode));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            Mock<ITransactionScope> transactionScopeMock = new();
            Mock<IUnitOfWork> unitOfWorkMock = new();
            unitOfWorkMock.Setup(u => u.BeginScope()).Returns(transactionScopeMock.Object);
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUnitOfWorkMock(unitOfWorkMock)
                .WithApplicationsRepository(applicationsRepository)
                .WithAuthorizationCodesRepository(authorizationCodesRepository)
                .WithRefreshTokensRepository(refreshTokensRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

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
            Application application = new ApplicationBuilder()
                .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                .Build();
            AuthorizationCode authorizationCode = new AuthorizationCodeBuilder()
                .WithId(new AuthorizationCodeId(HashedCode.Hash(code), application.Id))
                .WithExpiresAt(DateTime.Now.AddDays(1))
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            Mock<IAuthorizationCodesRepository> authorizationCodesRepositoryMock = new();
            Mock<IRefreshTokensRepository> refreshTokensRepositoryMock = new();
            authorizationCodesRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<AuthorizationCodeId>()))
                .Returns(Task.FromResult(authorizationCode));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            Mock<ITransactionScope> transactionScopeMock = new();
            Mock<IUnitOfWork> unitOfWorkMock = new();
            unitOfWorkMock.Setup(u => u.BeginScope()).Returns(transactionScopeMock.Object);
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUnitOfWorkMock(unitOfWorkMock)
                .WithApplicationsRepository(applicationsRepository)
                .WithAuthorizationCodesRepository(authorizationCodesRepository)
                .WithRefreshTokensRepository(refreshTokensRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

            TokenPair tokenPair = await authorizationService.GenerateTokens(
                applicationId,
                secretKey,
                application.CallbackUrl,
                code);

            transactionScopeMock.Verify(a => a.Complete(), Times.Once());
        }

        [Test]
        public void TestRefreshTokens_WhenNullRefreshTokenValueGiven_ThenArgumentNullExceptionIsThrown()
        {
            AuthorizationService authorizationService = new(UnitOfWorkBuilder.DefaultUnitOfWork);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(
                async () => await authorizationService.RefreshTokens(null, new Url("http://example.com/1")));

            Assert.That(exception, Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("refreshTokenValue"));
        }

        [Test]
        public void TestRefreshTokens_WhenNullCallbackUrlGiven_ThenArgumentNullExceptionIsThrown()
        {
            TokenValue refreshTokenValue = new TokenValueBuilder()
                .WithType(TokenType.Refresh)
                .WithExpirationDate(DateTime.Now.AddDays(1))
                .Build();
            AuthorizationService authorizationService = new(UnitOfWorkBuilder.DefaultUnitOfWork);

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
            TokenValue refreshTokenValue = new TokenValueBuilder()
                .WithApplicationId(applicationId)
                .WithType(TokenType.Refresh)
                .WithExpirationDate(DateTime.Now.AddDays(1))
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult((Application)null));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

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
            Application application = new ApplicationBuilder()
                .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            TokenValue refreshTokenValue = new TokenValueBuilder()
                .WithApplicationId(applicationId)
                .WithType(TokenType.Refresh)
                .WithExpirationDate(DateTime.Now.AddDays(1))
                .Build();
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

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
            Application application = new ApplicationBuilder()
                .WithId(applicationId)
                .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            Mock<IRefreshTokensRepository> refreshTokensRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            refreshTokensRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<TokenId>()))
                .Returns(Task.FromResult<RefreshToken>(null));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            TokenValue refreshTokenValue = new TokenValueBuilder()
                .WithApplicationId(applicationId)
                .WithType(TokenType.Refresh)
                .WithExpirationDate(DateTime.Now.AddDays(1))
                .Build();
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepository)
                .WithRefreshTokensRepository(refreshTokensRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

            RefreshTokenNotFoundException exception = Assert.ThrowsAsync<RefreshTokenNotFoundException>(
                async () => await authorizationService.RefreshTokens(refreshTokenValue, application.CallbackUrl));

            Assert.That(exception, Is.InstanceOf<RefreshTokenNotFoundException>()
                .And.Message
                .EqualTo($"Refresh token {new TokenId(TokenValueEncrypter.Encrypt(refreshTokenValue))} not found."));
        }

        [Test]
        public void TestRefreshTokens_WhenRefreshTokenFoundAndHasBeenUsed_ThenInvalidTokenExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Application application = new ApplicationBuilder()
                .WithId(applicationId)
                .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            Mock<IRefreshTokensRepository> refreshTokensRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            TokenValue refreshTokenValue = new TokenValueBuilder()
                .WithApplicationId(applicationId)
                .WithType(TokenType.Refresh)
                .WithExpirationDate(DateTime.Now.AddDays(1))
                .Build();
            RefreshToken refreshToken = new(
                new TokenId(TokenValueEncrypter.Encrypt(refreshTokenValue)),
                true);
            refreshTokensRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<TokenId>()))
                .Returns(Task.FromResult(refreshToken));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepository)
                .WithRefreshTokensRepository(refreshTokensRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

            InvalidTokenException exception = Assert.ThrowsAsync<InvalidTokenException>(
                async () => await authorizationService.RefreshTokens(refreshTokenValue, application.CallbackUrl));

            Assert.That(exception, Is.InstanceOf<InvalidTokenException>()
                .And.Message
                .EqualTo("Token was used before."));
        }

        [Test]
        public void TestRefreshTokens_WhenRefreshTokenFoundAndExpired_ThenInvalidTokenExceptionIsThrown()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Application application = new ApplicationBuilder()
                .WithId(applicationId)
                .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            Mock<IRefreshTokensRepository> refreshTokensRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            TokenValue refreshTokenValue = new TokenValueBuilder()
                .WithApplicationId(applicationId)
                .WithType(TokenType.Refresh)
                .WithExpirationDate(DateTime.Now.AddDays(-1))
                .Build();
            RefreshToken refreshToken = new(
                new TokenId(TokenValueEncrypter.Encrypt(refreshTokenValue)),
                false);
            refreshTokensRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<TokenId>()))
                .Returns(Task.FromResult(refreshToken));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithApplicationsRepository(applicationsRepository)
                .WithRefreshTokensRepository(refreshTokensRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWork);

            InvalidTokenException exception = Assert.ThrowsAsync<InvalidTokenException>(
                async () => await authorizationService.RefreshTokens(refreshTokenValue, application.CallbackUrl));

            Assert.That(exception, Is.InstanceOf<InvalidTokenException>()
                .And.Message
                .EqualTo("Token has expired."));
        }

        [Test]
        public async Task TestRefreshTokens_WhenRefreshTokenFoundAndVerifiedSucessful_ThenTokenPairIsReturned()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Application application = new ApplicationBuilder()
                .WithId(applicationId)
                .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            Mock<IRefreshTokensRepository> refreshTokensRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            TokenValue refreshTokenValue = new TokenValueBuilder()
                .WithApplicationId(applicationId)
                .WithType(TokenType.Refresh)
                .WithExpirationDate(DateTime.Now.AddDays(1))
                .Build();
            RefreshToken refreshToken = new(
                new TokenId(TokenValueEncrypter.Encrypt(refreshTokenValue)),
                false);
            refreshTokensRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<TokenId>()))
                .Returns(Task.FromResult(refreshToken));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            Mock<ITransactionScope> transactionScopeMock = new();
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock
                .Setup(u => u.BeginScope())
                .Returns(transactionScopeMock.Object);
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUnitOfWorkMock(unitOfWorkMock)
                .WithApplicationsRepository(applicationsRepository)
                .WithRefreshTokensRepository(refreshTokensRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWorkMock.Object);

            TokenPair tokenPair = await authorizationService.RefreshTokens(
                refreshTokenValue,
                application.CallbackUrl);

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
            Application application = new ApplicationBuilder()
                .WithId(applicationId)
                .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            Mock<IRefreshTokensRepository> refreshTokensRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            TokenValue refreshTokenValue = new TokenValueBuilder()
                .WithApplicationId(applicationId)
                .WithType(TokenType.Refresh)
                .WithExpirationDate(DateTime.Now.AddDays(1))
                .Build();
            RefreshToken refreshToken = new(
                new TokenId(TokenValueEncrypter.Encrypt(refreshTokenValue)),
                false);
            refreshTokensRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<TokenId>()))
                .Returns(Task.FromResult(refreshToken));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            Mock<ITransactionScope> transactionScopeMock = new();
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock
                .Setup(u => u.BeginScope())
                .Returns(transactionScopeMock.Object);
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUnitOfWorkMock(unitOfWorkMock)
                .WithApplicationsRepository(applicationsRepository)
                .WithRefreshTokensRepository(refreshTokensRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWorkMock.Object);

            TokenPair tokenPair = await authorizationService.RefreshTokens(
                refreshTokenValue,
                application.CallbackUrl);

            Assert.That(refreshToken.Used, Is.True);
        }

        [Test]
        public async Task TestRefreshTokens_WhenRefreshTokenFoundAndVerifiedSucessful_ThenRefreshTokenIsUpdated()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Application application = new ApplicationBuilder()
                .WithId(applicationId)
                .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            Mock<IRefreshTokensRepository> refreshTokensRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            TokenValue refreshTokenValue = new TokenValueBuilder()
                .WithApplicationId(applicationId)
                .WithType(TokenType.Refresh)
                .WithExpirationDate(DateTime.Now.AddDays(1))
                .Build();
            RefreshToken refreshToken = new(
                new TokenId(TokenValueEncrypter.Encrypt(refreshTokenValue)),
                false);
            refreshTokensRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<TokenId>()))
                .Returns(Task.FromResult(refreshToken));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            Mock<ITransactionScope> transactionScopeMock = new();
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock
                .Setup(u => u.BeginScope())
                .Returns(transactionScopeMock.Object);
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUnitOfWorkMock(unitOfWorkMock)
                .WithApplicationsRepository(applicationsRepository)
                .WithRefreshTokensRepository(refreshTokensRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWorkMock.Object);

            TokenPair tokenPair = await authorizationService.RefreshTokens(
                refreshTokenValue,
                application.CallbackUrl);

            refreshTokensRepositoryMock.Verify(r => r.UpdateAsync(refreshToken), Times.Once);
        }

        [Test]
        public async Task TestRefreshTokens_WhenRefreshTokenFoundAndVerifiedSucessful_ThenNewRefreshTokenIsAdded()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Application application = new ApplicationBuilder()
                .WithId(applicationId)
                .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            Mock<IRefreshTokensRepository> refreshTokensRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            TokenValue refreshTokenValue = new TokenValueBuilder()
                .WithApplicationId(applicationId)
                .WithType(TokenType.Refresh)
                .WithExpirationDate(DateTime.Now.AddDays(1))
                .Build();
            RefreshToken refreshToken = new(
                new TokenId(TokenValueEncrypter.Encrypt(refreshTokenValue)),
                false);
            refreshTokensRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<TokenId>()))
                .Returns(Task.FromResult(refreshToken));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            Mock<ITransactionScope> transactionScopeMock = new();
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock
                .Setup(u => u.BeginScope())
                .Returns(transactionScopeMock.Object);
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUnitOfWorkMock(unitOfWorkMock)
                .WithApplicationsRepository(applicationsRepository)
                .WithRefreshTokensRepository(refreshTokensRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWorkMock.Object);

            TokenPair tokenPair = await authorizationService.RefreshTokens(
                refreshTokenValue,
                application.CallbackUrl);

            RefreshToken newRefreshToken = new(
                new TokenId(TokenValueEncrypter.Encrypt(tokenPair.RefreshToken)),
                false);

            refreshTokensRepositoryMock.Verify(r => r.AddAsync(newRefreshToken), Times.Once);
        }

        [Test]
        public async Task TestRefreshTokens_WhenUpdateRefreshTokenThrowsException_ThenTransactionIsntCompleted()
        {
            ApplicationId applicationId = ApplicationId.Generate();
            SecretKey secretKey = SecretKey.Generate();
            Application application = new ApplicationBuilder()
                .WithId(applicationId)
                .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            Mock<IRefreshTokensRepository> refreshTokensRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            TokenValue refreshTokenValue = new TokenValueBuilder()
                .WithApplicationId(applicationId)
                .WithType(TokenType.Refresh)
                .WithExpirationDate(DateTime.Now.AddDays(1))
                .Build();
            RefreshToken refreshToken = new(
                new TokenId(TokenValueEncrypter.Encrypt(refreshTokenValue)),
                false);
            refreshTokensRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<TokenId>()))
                .Returns(Task.FromResult(refreshToken));
            refreshTokensRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<RefreshToken>())).Throws(new Exception());
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            Mock<ITransactionScope> transactionScopeMock = new();
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock
                .Setup(u => u.BeginScope())
                .Returns(transactionScopeMock.Object);
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUnitOfWorkMock(unitOfWorkMock)
                .WithApplicationsRepository(applicationsRepository)
                .WithRefreshTokensRepository(refreshTokensRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWorkMock.Object);

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
            Application application = new ApplicationBuilder()
                .WithId(applicationId)
                .WithSecretKey(EncryptedSecretKey.Encrypt(secretKey))
                .Build();
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            Mock<IRefreshTokensRepository> refreshTokensRepositoryMock = new();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult(application));
            TokenValue refreshTokenValue = new TokenValueBuilder()
                .WithApplicationId(applicationId)
                .WithType(TokenType.Refresh)
                .WithExpirationDate(DateTime.Now.AddDays(1))
                .Build();
            RefreshToken refreshToken = new(
                new TokenId(TokenValueEncrypter.Encrypt(refreshTokenValue)),
                false);
            refreshTokensRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<TokenId>()))
                .Returns(Task.FromResult(refreshToken));
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IRefreshTokensRepository refreshTokensRepository = refreshTokensRepositoryMock.Object;
            Mock<ITransactionScope> transactionScopeMock = new();
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock
                .Setup(u => u.BeginScope())
                .Returns(transactionScopeMock.Object);
            IUnitOfWork unitOfWork = new UnitOfWorkBuilder()
                .WithUnitOfWorkMock(unitOfWorkMock)
                .WithApplicationsRepository(applicationsRepository)
                .WithRefreshTokensRepository(refreshTokensRepository)
                .Build();
            AuthorizationService authorizationService = new(unitOfWorkMock.Object);

            try
            {
                TokenPair tokenPair = await authorizationService.RefreshTokens(
                    refreshTokenValue,
                    application.CallbackUrl);
            }
            catch(Exception)
            {
            }

            transactionScopeMock.Verify(t => t.Complete(), Times.Once);
        }
    }
}