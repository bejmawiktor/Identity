using DDD.Domain.Persistence;
using Identity.Persistence.MSSQL;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    using PermissionDto = Identity.Core.Application.PermissionDto;

    [TestFixture]
    public class PermissionRepositoryTest : DatabaseTestBase
    {
        private static PermissionDto[] PermissionsTestData => new PermissionDto[]
        {
            new PermissionDto(
                resourceId: "MyResource",
                name: "MyPermission1",
                description: "My permission 1 description."
            ),
            new PermissionDto(
                resourceId: "MyResource",
                name: "MyPermission2",
                description: "My permission 2 description."
            ),
            new PermissionDto(
                resourceId: "MyResource",
                name: "MyPermission3",
                description: "My permission 3 description."
            ),
            new PermissionDto(
                resourceId: "MyResource2",
                name: "MyPermission4",
                description: "My permission 4 description."
            ),
            new PermissionDto(
                resourceId: "MyResource2",
                name: "MyPermission5",
                description: "My permission 5 description."
            ),
        };

        public static IEnumerable<TestCaseData> PaginatedAsyncGetTestData
        {
            get
            {
                yield return new TestCaseData(new object[]
                {
                    PermissionRepositoryTest.PermissionsTestData,
                    new Pagination(0, 5),
                    PermissionRepositoryTest.PermissionsTestData
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenPermissionsAreReturned)}(1)");
                yield return new TestCaseData(new object[]
                {
                    PermissionRepositoryTest.PermissionsTestData,
                    new Pagination(0, 2),
                    new PermissionDto[]
                    {
                        PermissionRepositoryTest.PermissionsTestData[0],
                        PermissionRepositoryTest.PermissionsTestData[1],
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenPermissionsAreReturned)}(2)");
                yield return new TestCaseData(new object[]
                {
                    PermissionRepositoryTest.PermissionsTestData,
                    new Pagination(1, 2),
                    new PermissionDto[]
                    {
                        PermissionRepositoryTest.PermissionsTestData[2],
                        PermissionRepositoryTest.PermissionsTestData[3],
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenPermissionsAreReturned)}(3)");
                yield return new TestCaseData(new object[]
                {
                    PermissionRepositoryTest.PermissionsTestData,
                    new Pagination(2, 2),
                    new PermissionDto[]
                    {
                        PermissionRepositoryTest.PermissionsTestData[4],
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenPermissionsAreReturned)}(4)");
                yield return new TestCaseData(new object[]
                {
                    PermissionRepositoryTest.PermissionsTestData,
                    new Pagination(3, 2),
                    Enumerable.Empty<PermissionDto>()
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenPermissionsAreReturned)}(5)");
            }
        }

        [Test]
        public async Task TestAddAsync_WhenPermissionGiven_ThenPermissionIsStored()
        {
            var permissionDto = new PermissionDto(
                resourceId: "MyResource",
                name: "MyPermission",
                description: "My permission description.");
            var permissionRepository = new PermissionsRepository(this.IdentityContext);

            await permissionRepository.AddAsync(permissionDto);

            PermissionDto result = await permissionRepository.GetAsync(("MyResource", "MyPermission"));

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(("MyResource", "MyPermission")));
                Assert.That(result.Description, Is.EqualTo("My permission description."));
            });
        }

        [Test]
        public async Task TestUpdateAsync_WhenPermissionGiven_ThenPermissionIsUpdated()
        {
            var permissionDto = new PermissionDto(
                resourceId: "MyResource",
                name: "MyPermission",
                description: "My permission description.");
            var permissionRepository = new PermissionsRepository(this.IdentityContext);
            await permissionRepository.AddAsync(permissionDto);
            permissionDto = new PermissionDto(
                resourceId: "MyResource",
                name: "MyPermission",
                description: "My permission description 2.");

            await permissionRepository.UpdateAsync(permissionDto);

            PermissionDto result = await permissionRepository.GetAsync(("MyResource", "MyPermission"));

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(("MyResource", "MyPermission")));
                Assert.That(result.Description, Is.EqualTo("My permission description 2."));
            });
        }

        [Test]
        public async Task TestRemoveAsync_WhenPermissionGiven_ThenPermissionIsRemoved()
        {
            var permissionDto = new PermissionDto(
                resourceId: "MyResource",
                name: "MyPermission",
                description: "My permission description.");
            var permissionRepository = new PermissionsRepository(this.IdentityContext);
            await permissionRepository.AddAsync(permissionDto);

            await permissionRepository.RemoveAsync(permissionDto);

            PermissionDto result = await permissionRepository.GetAsync(("MyResource", "MyPermission"));

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TestGetAsync_WhenPermissionIdGiven_ThenPermissionIsReturned()
        {
            var permissionDto = new PermissionDto(
                resourceId: "MyResource",
                name: "MyPermission",
                description: "My permission description.");
            var permissionRepository = new PermissionsRepository(this.IdentityContext);
            await permissionRepository.AddAsync(permissionDto);

            PermissionDto result = await permissionRepository.GetAsync(("MyResource", "MyPermission"));

            Assert.That(result, Is.EqualTo(permissionDto));
        }

        [TestCaseSource(nameof(PaginatedAsyncGetTestData))]
        public async Task TestGetAsync_WhenPaginationGiven_ThenPermissionsAreReturned(
            IEnumerable<PermissionDto> permissions,
            Pagination pagination,
            IEnumerable<PermissionDto> expectedPermissions)
        {
            var permissionDto = new PermissionDto(
                resourceId: "MyResource",
                name: "MyPermission",
                description: "My permission description.");
            var permissionRepository = new PermissionsRepository(this.IdentityContext);
            permissions.ToList().ForEach(r => permissionRepository.AddAsync(r).Wait());

            IEnumerable<PermissionDto> result = await permissionRepository.GetAsync(pagination);

            Assert.That(result, Is.EquivalentTo(expectedPermissions));
        }
    }
}