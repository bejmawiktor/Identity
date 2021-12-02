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
    public class RefreshTokensRepositoryTest : DatabaseTestBase
    {
        private static readonly RefreshTokenDto[] RefreshTokensTestData = new RefreshTokenDto[]
        {
            new RefreshTokenDto(RefreshTokensRepositoryTest.GetTokenId(), true),
            new RefreshTokenDto(RefreshTokensRepositoryTest.GetTokenId(), false),
            new RefreshTokenDto(RefreshTokensRepositoryTest.GetTokenId(), true),
            new RefreshTokenDto(RefreshTokensRepositoryTest.GetTokenId(), true),
            new RefreshTokenDto(RefreshTokensRepositoryTest.GetTokenId(), false)
        };

        private static string GetTokenId()
        {
            var permissions = new PermissionId[]
            {
                new PermissionId(new ResourceId("MyResource1"), "Add")
            };
            DateTime expiresAt = DateTime.Now;
            TokenId tokenId = TokenId.GenerateRefreshTokenId(ApplicationId.Generate(), permissions, expiresAt);

            return tokenId.ToString();
        }

        public static IEnumerable<TestCaseData> PaginatedAsyncGetTestData
        {
            get
            {
                yield return new TestCaseData(new object[]
                {
                    RefreshTokensRepositoryTest.RefreshTokensTestData,
                    new Pagination(0, 5),
                    RefreshTokensRepositoryTest.RefreshTokensTestData
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenRefreshTokensAreReturned)}(1)");
                yield return new TestCaseData(new object[]
                {
                    RefreshTokensRepositoryTest.RefreshTokensTestData,
                    new Pagination(0, 2),
                    new RefreshTokenDto[]
                    {
                        RefreshTokensRepositoryTest.RefreshTokensTestData[0],
                        RefreshTokensRepositoryTest.RefreshTokensTestData[1]
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenRefreshTokensAreReturned)}(2)");
                yield return new TestCaseData(new object[]
                {
                    RefreshTokensRepositoryTest.RefreshTokensTestData,
                    new Pagination(1, 2),
                    new RefreshTokenDto[]
                    {
                        RefreshTokensRepositoryTest.RefreshTokensTestData[2],
                        RefreshTokensRepositoryTest.RefreshTokensTestData[3]
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenRefreshTokensAreReturned)}(3)");
                yield return new TestCaseData(new object[]
                {
                    RefreshTokensRepositoryTest.RefreshTokensTestData,
                    new Pagination(2, 2),
                    new RefreshTokenDto[]
                    {
                        RefreshTokensRepositoryTest.RefreshTokensTestData[4]
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenRefreshTokensAreReturned)}(4)");
                yield return new TestCaseData(new object[]
                {
                    RefreshTokensRepositoryTest.RefreshTokensTestData,
                    new Pagination(3, 2),
                    Enumerable.Empty<RefreshTokenDto>()
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenRefreshTokensAreReturned)}(5)");
            }
        }

        [Test]
        public async Task TestAddAsync_WhenRefreshTokenGiven_ThenRefreshTokenIsStored()
        {
            string tokenId = RefreshTokensRepositoryTest.GetTokenId();
            var refreshTokenDto = new RefreshTokenDto(
                id: tokenId,
                used: false);
            var refreshTokensRepository = new RefreshTokensRepository(this.IdentityContext);

            await refreshTokensRepository.AddAsync(refreshTokenDto);

            RefreshTokenDto result = await refreshTokensRepository.GetAsync(tokenId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(tokenId));
                Assert.That(result.Used, Is.False);
            });
        }

        [Test]
        public async Task TestUpdateAsync_WhenRefreshTokenGiven_ThenRefreshTokenIsUpdated()
        {
            string tokenId = RefreshTokensRepositoryTest.GetTokenId();
            var refreshTokenDto = new RefreshTokenDto(
                id: tokenId,
                used: false);
            var refreshTokensRepository = new RefreshTokensRepository(this.IdentityContext);
            await refreshTokensRepository.AddAsync(refreshTokenDto);
            refreshTokenDto = new RefreshTokenDto(
                id: tokenId,
                used: true);

            await refreshTokensRepository.UpdateAsync(refreshTokenDto);

            RefreshTokenDto result = await refreshTokensRepository.GetAsync(tokenId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(tokenId));
                Assert.That(result.Used, Is.True);
            });
        }

        [Test]
        public async Task TestRemoveAsync_WhenRefreshTokenGiven_ThenRefreshTokenIsRemoved()
        {
            string tokenId = RefreshTokensRepositoryTest.GetTokenId();
            var refreshTokenDto = new RefreshTokenDto(
                id: RefreshTokensRepositoryTest.GetTokenId(),
                used: true);
            var refreshTokensRepository = new RefreshTokensRepository(this.IdentityContext);
            await refreshTokensRepository.AddAsync(refreshTokenDto);

            await refreshTokensRepository.RemoveAsync(refreshTokenDto);

            RefreshTokenDto result = await refreshTokensRepository.GetAsync(tokenId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TestGetAsync_WhenRefreshTokenIdGiven_ThenRefreshTokenIsReturned()
        {
            string tokenId = RefreshTokensRepositoryTest.GetTokenId();
            var refreshTokenDto = new RefreshTokenDto(
                id: tokenId,
                used: true);
            var refreshTokensRepository = new RefreshTokensRepository(this.IdentityContext);
            await refreshTokensRepository.AddAsync(refreshTokenDto);

            RefreshTokenDto result = await refreshTokensRepository.GetAsync(tokenId);

            Assert.That(result, Is.EqualTo(refreshTokenDto));
        }

        [TestCaseSource(nameof(PaginatedAsyncGetTestData))]
        public async Task TestGetAsync_WhenPaginationGiven_ThenRefreshTokensAreReturned(
            IEnumerable<RefreshTokenDto> refreshTokens,
            Pagination pagination,
            IEnumerable<RefreshTokenDto> expectedRefreshTokens)
        {
            var refreshTokensRepository = new RefreshTokensRepository(this.IdentityContext);
            refreshTokens.ToList().ForEach(r => refreshTokensRepository.AddAsync(r).Wait());

            IEnumerable<RefreshTokenDto> result = await refreshTokensRepository.GetAsync(pagination);

            Assert.That(result, Is.EquivalentTo(expectedRefreshTokens));
        }
    }
}