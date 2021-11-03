using Identity.Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Domain
{
    using Application = Identity.Domain.Application;
    using ApplicationId = Identity.Domain.ApplicationId;

    [TestFixture]
    public class AuthorizationServiceTest
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
        public void TestConstructor_WhenUsersRepositoryGiven_ThenUsersRepositoryIsSet()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;

            AuthorizationService authorizationService = this.GetAuthorizationService(usersRepository: usersRepository);

            Assert.That(authorizationService.UsersRepository, Is.EqualTo(usersRepository));
        }

        private AuthorizationService GetAuthorizationService(
            IUsersRepository usersRepository = null,
            IRolesRepository rolesRepository = null,
            IApplicationsRepository applicationsRepository = null,
            IAuthorizationCodesRepository authorizationCodesRepository = null)
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();

            return new AuthorizationService(
                usersRepository ?? usersRepositoryMock.Object,
                rolesRepository ?? rolesRepositoryMock.Object,
                applicationsRepository ?? applicationsRepositoryMock.Object,
                authorizationCodesRepository ?? authorizationCodesRepositoryMock.Object);
        }

        [Test]
        public void TestConstructor_WhenNullUsersRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("usersRepository"),
               () => new AuthorizationService(
                   null,
                   rolesRepositoryMock.Object,
                   applicationsRepositoryMock.Object,
                   authorizationCodesRepositoryMock.Object));
        }

        [Test]
        public void TestConstructor_WhenResourcesRepositoryGiven_ThenResourcesRepositoryIsSet()
        {
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;

            AuthorizationService authorizationService = this.GetAuthorizationService(rolesRepository: rolesRepositoryMock.Object);

            Assert.That(authorizationService.RolesRepository, Is.EqualTo(rolesRepository));
        }

        [Test]
        public void TestConstructor_WhenNullResourcesRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("rolesRepository"),
               () => new AuthorizationService(
                   usersRepositoryMock.Object,
                   null,
                   applicationsRepositoryMock.Object,
                   authorizationCodesRepositoryMock.Object));
        }

        [Test]
        public void TestConstructor_WhenApplicationsRepositoryGiven_ThenApplicationsRepositoryIsSet()
        {
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;

            AuthorizationService authorizationService = this.GetAuthorizationService(applicationsRepository: applicationsRepository);

            Assert.That(authorizationService.ApplicationsRepository, Is.EqualTo(applicationsRepository));
        }

        [Test]
        public void TestConstructor_WhenNullApplicationsRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("applicationsRepository"),
               () => new AuthorizationService(
                   usersRepositoryMock.Object,
                   rolesRepositoryMock.Object,
                   null,
                   authorizationCodesRepositoryMock.Object));
        }

        [Test]
        public void TestConstructor_WhenAuthorizationCodesRepositoryGiven_ThenApplicationsRepositoryIsSet()
        {
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;

            AuthorizationService authorizationService = this.GetAuthorizationService(
                authorizationCodesRepository: authorizationCodesRepository);

            Assert.That(authorizationService.AuthorizationCodesRepository, Is.EqualTo(authorizationCodesRepository));
        }

        [Test]
        public void TestConstructor_WhenNullAuthorizationCodesRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();

            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("authorizationCodesRepository"),
               () => new AuthorizationService(
                   usersRepositoryMock.Object,
                   rolesRepositoryMock.Object,
                   applicationsRepositoryMock.Object,
                   null));
        }

        [Test]
        public void TestCheckUserAccess_WhenNullUserIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            AuthorizationService authorizationService = this.GetAuthorizationService();

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
            AuthorizationService authorizationService = this.GetAuthorizationService();

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
            AuthorizationService authorizationService = this.GetAuthorizationService(usersRepository: usersRepository);

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
            AuthorizationService authorizationService = this.GetAuthorizationService(
                usersRepository,
                rolesRepository);

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
            AuthorizationService authorizationService = this.GetAuthorizationService(usersRepository: usersRepository);

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
            AuthorizationService authorizationService = this.GetAuthorizationService(
                usersRepository: usersRepository,
                rolesRepository: rolesRepository);

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
            AuthorizationService authorizationService = this.GetAuthorizationService(
                usersRepository: usersRepository);

            bool userIsPermitted = await authorizationService.CheckUserAccess(user.Id, permissionId);

            Assert.That(userIsPermitted, Is.False);
        }

        [Test]
        public void TestGenerateAuthorizationCode_WhenNullApplicationIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            var permissions = new PermissionId[] { new PermissionId(new ResourceId("MyResource"), "Add") };
            AuthorizationService authorizationService = this.GetAuthorizationService();

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
            AuthorizationService authorizationService = this.GetAuthorizationService();

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
            AuthorizationService authorizationService = this.GetAuthorizationService(
                applicationsRepository: applicationsRepository);

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
            AuthorizationService authorizationService = this.GetAuthorizationService(
                applicationsRepository: applicationsRepository);

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
            AuthorizationService authorizationService = this.GetAuthorizationService(
                applicationsRepository: applicationsRepository);

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
            AuthorizationService authorizationService = this.GetAuthorizationService(
                applicationsRepository: applicationsRepository);

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
            AuthorizationService authorizationService = this.GetAuthorizationService(
                usersRepository: usersRepository,
                rolesRepository: rolesRepository,
                applicationsRepository: applicationsRepository);

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
            AuthorizationService authorizationService = this.GetAuthorizationService(
                usersRepository: usersRepository,
                rolesRepository: rolesRepository,
                applicationsRepository: applicationsRepository,
                authorizationCodesRepository: authorizationCodesRepository);

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
            AuthorizationService authorizationService = this.GetAuthorizationService(
                applicationsRepository: applicationsRepository);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(
                async () => await authorizationService.GenerateTokens(null, SecretKey.Generate(), new Url("http://example.com/1"), Code.Generate()));

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
            AuthorizationService authorizationService = this.GetAuthorizationService(
                applicationsRepository: applicationsRepository);

            ApplicationNotFoundException exception = Assert.ThrowsAsync<ApplicationNotFoundException>(
                async () => await authorizationService.GenerateTokens(applicationId, SecretKey.Generate(), new Url("http://example.com/1"), Code.Generate()));

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
            AuthorizationService authorizationService = this.GetAuthorizationService(
                applicationsRepository: applicationsRepository);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(
                async () => await authorizationService.GenerateTokens(applicationId, null, new Url("http://example.com/1"), Code.Generate()));

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
            AuthorizationService authorizationService = this.GetAuthorizationService(
                applicationsRepository: applicationsRepository,
                authorizationCodesRepository: authorizationCodesRepository);

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(
                async () => await authorizationService.GenerateTokens(applicationId, SecretKey.Generate(), new Url("http://example.com/1"), Code.Generate()));

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
            AuthorizationService authorizationService = this.GetAuthorizationService(
                applicationsRepository: applicationsRepository,
                authorizationCodesRepository: authorizationCodesRepository);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(
                async () => await authorizationService.GenerateTokens(applicationId, secretKey, null, Code.Generate()));

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
            AuthorizationService authorizationService = this.GetAuthorizationService(
                applicationsRepository: applicationsRepository,
                authorizationCodesRepository: authorizationCodesRepository);

            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(
                async () => await authorizationService.GenerateTokens(applicationId, secretKey, new Url("http://example.com/2"), Code.Generate()));

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
            AuthorizationService authorizationService = this.GetAuthorizationService(
                applicationsRepository: applicationsRepository);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(
                async () => await authorizationService.GenerateTokens(applicationId, secretKey, new Url("http://example.com/1"), null));

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
            AuthorizationService authorizationService = this.GetAuthorizationService(
                applicationsRepository: applicationsRepository,
                authorizationCodesRepository: authorizationCodesRepository);

            AuthorizationCodeNotFoundException exception = Assert.ThrowsAsync<AuthorizationCodeNotFoundException>(
                async () => await authorizationService.GenerateTokens(applicationId, secretKey, new Url("http://example.com/1"), Code.Generate()));

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
            AuthorizationService authorizationService = this.GetAuthorizationService(
                applicationsRepository: applicationsRepository,
                authorizationCodesRepository: authorizationCodesRepository);

            InvalidOperationException exception = Assert.ThrowsAsync<InvalidOperationException>(
                async () => await authorizationService.GenerateTokens(applicationId, secretKey, new Url("http://example.com/1"), code));

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
            AuthorizationService authorizationService = this.GetAuthorizationService(
                applicationsRepository: applicationsRepository,
                authorizationCodesRepository: authorizationCodesRepository);

            InvalidOperationException exception = Assert.ThrowsAsync<InvalidOperationException>(
                async () => await authorizationService.GenerateTokens(applicationId, secretKey, new Url("http://example.com/1"), code));

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
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            AuthorizationService authorizationService = this.GetAuthorizationService(
                applicationsRepository: applicationsRepository,
                authorizationCodesRepository: authorizationCodesRepository);

            TokenPair tokenPair = await authorizationService.GenerateTokens(applicationId, secretKey, new Url("http://example.com/1"), code);

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
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            AuthorizationService authorizationService = this.GetAuthorizationService(
                applicationsRepository: applicationsRepository,
                authorizationCodesRepository: authorizationCodesRepository);

            TokenPair tokenPair = await authorizationService.GenerateTokens(
                applicationId,
                secretKey,
                new Url("http://example.com/1"),
                code);

            Assert.That(authorizationCode.Used, Is.True);
            authorizationCodesRepositoryMock.Verify(a => a.UpdateAsync(It.IsAny<AuthorizationCode>()), Times.Once());
        }
    }
}