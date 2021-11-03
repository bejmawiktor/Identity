using DDD.Domain.Persistence;
using Identity.Application;
using Identity.Domain;
using Identity.Persistence.MSSQL;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    using ApplicationId = Identity.Domain.ApplicationId;

    [TestFixture]
    public class AuthorizationCodesRepositoryTest : DatabaseTestBase
    {
        private static ApplicationId ApplicationId = ApplicationId.Generate();
        private static readonly (string ResourceId, string Name)[] TestPermissions = new (string ResourceId, string Name)[]
        {
            ("MyResource1", "Add"),
            ("MyResource2", "Add")
        };

        private static readonly AuthorizationCodeDto[] AuthorizationCodesTestData = new AuthorizationCodeDto[]
        {
            new AuthorizationCodeDto(
                code: AuthorizationCodeId.Generate(AuthorizationCodesRepositoryTest.ApplicationId, out Code code).Code.ToString(),
                applicationId: ApplicationId.ToGuid(),
                expiresAt: DateTime.Now,
                used: true,
                permissions: TestPermissions),
            new AuthorizationCodeDto(
                code: AuthorizationCodeId.Generate(AuthorizationCodesRepositoryTest.ApplicationId, out Code code2).Code.ToString(),
                applicationId: ApplicationId.ToGuid(),
                expiresAt: DateTime.Now,
                used: true,
                permissions: TestPermissions),
            new AuthorizationCodeDto(
                code: AuthorizationCodeId.Generate(AuthorizationCodesRepositoryTest.ApplicationId, out Code code3).Code.ToString(),
                applicationId: ApplicationId.ToGuid(),
                expiresAt: DateTime.Now,
                used: false,
                permissions: TestPermissions),
            new AuthorizationCodeDto(
                code: AuthorizationCodeId.Generate(AuthorizationCodesRepositoryTest.ApplicationId, out Code code4).Code.ToString(),
                applicationId: ApplicationId.ToGuid(),
                expiresAt: DateTime.Now,
                used: true,
                permissions: TestPermissions),
            new AuthorizationCodeDto(
                code: AuthorizationCodeId.Generate(AuthorizationCodesRepositoryTest.ApplicationId, out Code code5).Code.ToString(),
                applicationId: ApplicationId.ToGuid(),
                expiresAt: DateTime.Now,
                used: false,
                permissions: TestPermissions)
        };

        public static IEnumerable<TestCaseData> PaginatedAsyncGetTestData
        {
            get
            {
                yield return new TestCaseData(new object[]
                {
                    AuthorizationCodesRepositoryTest.AuthorizationCodesTestData,
                    new Pagination(0, 5),
                    AuthorizationCodesRepositoryTest.AuthorizationCodesTestData
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenAuthorizationCodesAreReturned)}(1)");
                yield return new TestCaseData(new object[]
                {
                    AuthorizationCodesRepositoryTest.AuthorizationCodesTestData,
                    new Pagination(0, 2),
                    new AuthorizationCodeDto[]
                    {
                        AuthorizationCodesRepositoryTest.AuthorizationCodesTestData[0],
                        AuthorizationCodesRepositoryTest.AuthorizationCodesTestData[1],
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenAuthorizationCodesAreReturned)}(2)");
                yield return new TestCaseData(new object[]
                {
                    AuthorizationCodesRepositoryTest.AuthorizationCodesTestData,
                    new Pagination(1, 2),
                    new AuthorizationCodeDto[]
                    {
                        AuthorizationCodesRepositoryTest.AuthorizationCodesTestData[2],
                        AuthorizationCodesRepositoryTest.AuthorizationCodesTestData[3],
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenAuthorizationCodesAreReturned)}(3)");
                yield return new TestCaseData(new object[]
                {
                    AuthorizationCodesRepositoryTest.AuthorizationCodesTestData,
                    new Pagination(2, 2),
                    new AuthorizationCodeDto[]
                    {
                        AuthorizationCodesRepositoryTest.AuthorizationCodesTestData[4],
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenAuthorizationCodesAreReturned)}(4)");
                yield return new TestCaseData(new object[]
                {
                    AuthorizationCodesRepositoryTest.AuthorizationCodesTestData,
                    new Pagination(3, 2),
                    Enumerable.Empty<AuthorizationCodeDto>()
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenAuthorizationCodesAreReturned)}(5)");
            }
        }

        [Test]
        public async Task TestAddAsync_WhenAuthorizationCodeGiven_ThenAuthorizationCodeIsStored()
        {
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(AuthorizationCodesRepositoryTest.ApplicationId, out _);
            DateTime now = DateTime.Now;
            var authorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: authorizationCodeId.ApplicationId.ToGuid(),
                expiresAt: now,
                used: true,
                permissions: TestPermissions);
            var authorizationCodesRepository = new AuthorizationCodesRepository(this.IdentityContext);

            await authorizationCodesRepository.AddAsync(authorizationCodeDto);

            AuthorizationCodeDto result = await authorizationCodesRepository.GetAsync((authorizationCodeId.ApplicationId.ToGuid(), authorizationCodeId.Code.ToString()));

            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(authorizationCodeDto.Code));
                Assert.That(result.ApplicationId, Is.EqualTo(AuthorizationCodesRepositoryTest.ApplicationId.ToGuid()));
                Assert.That(result.ExpiresAt, Is.EqualTo(now));
                Assert.That(result.Used, Is.True);
            });
        }

        [Test]
        public async Task TestUpdateAsync_WhenAuthorizationCodeGiven_ThenAuthorizationCodeIsUpdated()
        {
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(AuthorizationCodesRepositoryTest.ApplicationId, out _);
            DateTime now = DateTime.Now;
            var authorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: authorizationCodeId.ApplicationId.ToGuid(),
                expiresAt: now,
                used: false,
                permissions: TestPermissions);
            var authorizationCodesRepository = new AuthorizationCodesRepository(this.IdentityContext);
            await authorizationCodesRepository.AddAsync(authorizationCodeDto);
            authorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: authorizationCodeId.ApplicationId.ToGuid(),
                expiresAt: now,
                used: true,
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource1", "Add")
                });

            await authorizationCodesRepository.UpdateAsync(authorizationCodeDto);

            AuthorizationCodeDto result = await authorizationCodesRepository.GetAsync((authorizationCodeId.ApplicationId.ToGuid(), authorizationCodeId.Code.ToString()));

            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(authorizationCodeDto.Code));
                Assert.That(result.ApplicationId, Is.EqualTo(AuthorizationCodesRepositoryTest.ApplicationId.ToGuid()));
                Assert.That(result.ExpiresAt, Is.EqualTo(now));
                Assert.That(result.Used, Is.True);
            });
        }

        [Test]
        public async Task TestRemoveAsync_WhenAuthorizationCodeGiven_ThenAuthorizationCodeIsRemoved()
        {
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(AuthorizationCodesRepositoryTest.ApplicationId, out _);
            DateTime now = DateTime.Now;
            var authorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: authorizationCodeId.ApplicationId.ToGuid(),
                expiresAt: now,
                used: false,
                permissions: TestPermissions);
            var authorizationCodesRepository = new AuthorizationCodesRepository(this.IdentityContext);
            await authorizationCodesRepository.AddAsync(authorizationCodeDto);

            await authorizationCodesRepository.RemoveAsync(authorizationCodeDto);

            AuthorizationCodeDto result = await authorizationCodesRepository.GetAsync((authorizationCodeId.ApplicationId.ToGuid(), authorizationCodeId.Code.ToString()));

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TestGetAsync_WhenAuthorizationCodeIdGiven_ThenAuthorizationCodeIsReturned()
        {
            AuthorizationCodeId authorizationCodeId = AuthorizationCodeId.Generate(AuthorizationCodesRepositoryTest.ApplicationId, out _);
            DateTime now = DateTime.Now;
            var authorizationCodeDto = new AuthorizationCodeDto(
                code: authorizationCodeId.Code.ToString(),
                applicationId: authorizationCodeId.ApplicationId.ToGuid(),
                expiresAt: now,
                used: false,
                permissions: TestPermissions);
            var authorizationCodesRepository = new AuthorizationCodesRepository(this.IdentityContext);
            await authorizationCodesRepository.AddAsync(authorizationCodeDto);

            AuthorizationCodeDto result = await authorizationCodesRepository.GetAsync((authorizationCodeId.ApplicationId.ToGuid(), authorizationCodeId.Code.ToString()));

            Assert.That(result, Is.EqualTo(authorizationCodeDto));
        }

        [TestCaseSource(nameof(PaginatedAsyncGetTestData))]
        public async Task TestGetAsync_WhenPaginationGiven_ThenAuthorizationCodesAreReturned(
            IEnumerable<AuthorizationCodeDto> authorizationCodes,
            Pagination pagination,
            IEnumerable<AuthorizationCodeDto> expectedAuthorizationCodes)
        {
            var authorizationCodesRepository = new AuthorizationCodesRepository(this.IdentityContext);
            authorizationCodes.ToList().ForEach(r => authorizationCodesRepository.AddAsync(r).Wait());

            IEnumerable<AuthorizationCodeDto> result = await authorizationCodesRepository.GetAsync(pagination);

            Assert.That(result, Is.EquivalentTo(expectedAuthorizationCodes));
        }
    }
}
