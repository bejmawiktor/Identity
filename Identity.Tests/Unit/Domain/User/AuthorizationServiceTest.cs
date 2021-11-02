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
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;

            var authorizationService = new AuthorizationService(
                usersRepository,
                rolesRepository,
                applicationsRepository,
                authorizationCodesRepository);

            Assert.That(authorizationService.UsersRepository, Is.EqualTo(usersRepository));
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
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;

            var authorizationService = new AuthorizationService(
                usersRepository,
                rolesRepository,
                applicationsRepository,
                authorizationCodesRepository);

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
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;

            var authorizationService = new AuthorizationService(
                usersRepository,
                rolesRepository,
                applicationsRepository,
                authorizationCodesRepository);

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
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;

            var authorizationService = new AuthorizationService(
                usersRepository,
                rolesRepository,
                applicationsRepository,
                authorizationCodesRepository);

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
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            var authorizationService = new AuthorizationService(
                usersRepository,
                rolesRepository,
                applicationsRepository,
                authorizationCodesRepository);

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
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            var authorizationService = new AuthorizationService(
                usersRepository,
                rolesRepository,
                applicationsRepository,
                authorizationCodesRepository);

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
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            UserId userId = UserId.Generate();
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            var authorizationService = new AuthorizationService(
                usersRepository,
                rolesRepository,
                applicationsRepository,
                authorizationCodesRepository);

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
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
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
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            var authorizationService = new AuthorizationService(
                usersRepository,
                rolesRepository,
                applicationsRepository,
                authorizationCodesRepository);

            bool userIsPermitted = await authorizationService.CheckUserAccess(user.Id, permissionId);

            Assert.That(userIsPermitted, Is.True);
        }

        [Test]
        public async Task TestCheckUserAccess_WhenUserPermittedBySinglePermission_ThenTrueIsReturned()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
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
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            var authorizationService = new AuthorizationService(
                usersRepository,
                rolesRepository,
                applicationsRepository,
                authorizationCodesRepository);

            bool userIsPermitted = await authorizationService.CheckUserAccess(user.Id, permissionId);

            Assert.That(userIsPermitted, Is.True);
        }

        [Test]
        public async Task TestCheckUserAccess_WhenUserPermittedFromRoleAndSinglePermission_ThenTrueIsReturned()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
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
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            var authorizationService = new AuthorizationService(
                usersRepository,
                rolesRepository,
                applicationsRepository,
                authorizationCodesRepository);

            bool userIsPermitted = await authorizationService.CheckUserAccess(user.Id, permissionId);

            Assert.That(userIsPermitted, Is.True);
        }

        [Test]
        public async Task TestCheckUserAccess_WhenUserIsntPermittedFromRoleAndSinglePermission_ThenFalseIsReturned()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            var permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");
            var user = new User(
                id: UserId.Generate(),
                email: new EmailAddress("example@example.com"),
                password: AuthorizationServiceTest.TestPassword);
            usersRepositoryMock.Setup(u => u.GetAsync(It.IsAny<UserId>())).Returns(Task.FromResult(user));
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            var authorizationService = new AuthorizationService(
                usersRepository,
                rolesRepository,
                applicationsRepository,
                authorizationCodesRepository);

            bool userIsPermitted = await authorizationService.CheckUserAccess(user.Id, permissionId);

            Assert.That(userIsPermitted, Is.False);
        }

        [Test]
        public void TestGenerateAuthorizationCode_WhenApplicationWasNotFoundInRepository_ThenUnknownApplicationExceptionIsThrown()
        {
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            applicationsRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<ApplicationId>()))
                .Returns(Task.FromResult((Application)null));
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            var authorizationService = new AuthorizationService(
                usersRepository,
                rolesRepository,
                applicationsRepository,
                authorizationCodesRepository);
            var applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[] { new PermissionId(new ResourceId("MyResource"), "Add") };

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
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
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
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            var authorizationService = new AuthorizationService(
                usersRepository,
                rolesRepository,
                applicationsRepository,
                authorizationCodesRepository);
            var applicationId = ApplicationId.Generate();
            var permissions = new PermissionId[] { new PermissionId(new ResourceId("MyResource"), "Add") };

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
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
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
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            var authorizationService = new AuthorizationService(
                usersRepository,
                rolesRepository,
                applicationsRepository,
                authorizationCodesRepository);
            var applicationId = ApplicationId.Generate();

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
            var usersRepositoryMock = new Mock<IUsersRepository>();
            var rolesRepositoryMock = new Mock<IRolesRepository>();
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
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            var authorizationService = new AuthorizationService(
                usersRepository,
                rolesRepository,
                applicationsRepository,
                authorizationCodesRepository);
            var applicationId = ApplicationId.Generate();

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
            var authorizationCodesRepositoryMock = new Mock<IAuthorizationCodesRepository>();
            IUsersRepository usersRepository = usersRepositoryMock.Object;
            IRolesRepository rolesRepository = rolesRepositoryMock.Object;
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            IAuthorizationCodesRepository authorizationCodesRepository = authorizationCodesRepositoryMock.Object;
            var authorizationService = new AuthorizationService(
                usersRepository,
                rolesRepository,
                applicationsRepository,
                authorizationCodesRepository);
            var applicationId = ApplicationId.Generate();

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
            var authorizationService = new AuthorizationService(
                usersRepository,
                rolesRepository,
                applicationsRepository,
                authorizationCodesRepository);
            var applicationId = ApplicationId.Generate();

            Code authorizationCode = await authorizationService.GenerateAuthorizationCode(
                applicationId,
                new Url("http://example.com/1"),
                requestedPermissions);

            Assert.That(authorizationCode, Is.Not.Null);
            authorizationCodesRepositoryMock.Verify(r => r.AddAsync(It.IsAny<AuthorizationCode>()), Times.Once);
        }
    }
}