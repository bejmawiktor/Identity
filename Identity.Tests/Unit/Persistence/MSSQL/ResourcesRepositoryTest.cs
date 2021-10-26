using DDD.Domain.Persistence;
using Identity.Application;
using Identity.Persistence.MSSQL;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    [TestFixture]
    public class ResourcesRepositoryTest : DatabaseTestBase
    {
        private static ResourceDto[] ResourcesTestData => new ResourceDto[]
        {
            new ResourceDto("MyResource1", "My resource description."),
            new ResourceDto("MyResource2", "My resource description."),
            new ResourceDto("MyResource3", "My resource description."),
            new ResourceDto("MyResource4", "My resource description."),
            new ResourceDto("MyResource5", "My resource description.")
        };

        public static IEnumerable<TestCaseData> PaginatedAsyncGetTestData
        {
            get
            {
                yield return new TestCaseData(new object[]
                {
                    ResourcesRepositoryTest.ResourcesTestData,
                    new Pagination(0, 5),
                    ResourcesRepositoryTest.ResourcesTestData
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenResourcesAreReturned)}(1)");
                yield return new TestCaseData(new object[]
                {
                    ResourcesRepositoryTest.ResourcesTestData,
                    new Pagination(0, 2),
                    new ResourceDto[]
                    {
                        ResourcesRepositoryTest.ResourcesTestData[0],
                        ResourcesRepositoryTest.ResourcesTestData[1]
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenResourcesAreReturned)}(2)");
                yield return new TestCaseData(new object[]
                {
                    ResourcesRepositoryTest.ResourcesTestData,
                    new Pagination(1, 2),
                    new ResourceDto[]
                    {
                        ResourcesRepositoryTest.ResourcesTestData[2],
                        ResourcesRepositoryTest.ResourcesTestData[3]
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenResourcesAreReturned)}(3)");
                yield return new TestCaseData(new object[]
                {
                    ResourcesRepositoryTest.ResourcesTestData,
                    new Pagination(2, 2),
                    new ResourceDto[]
                    {
                        ResourcesRepositoryTest.ResourcesTestData[4]
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenResourcesAreReturned)}(4)");
                yield return new TestCaseData(new object[]
                {
                    ResourcesRepositoryTest.ResourcesTestData,
                    new Pagination(3, 2),
                    Enumerable.Empty<ResourceDto>()
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenResourcesAreReturned)}(5)");
            }
        }

        [Test]
        public async Task TestAddAsync_WhenResourceGiven_ThenResourceIsStored()
        {
            var resourceDto = new ResourceDto(
                id: "MyResource",
                description: "My test resource");
            var resourceRepository = new ResourcesRepository(this.IdentityContext);

            await resourceRepository.AddAsync(resourceDto);

            ResourceDto result = await resourceRepository.GetAsync("MyResource");

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo("MyResource"));
                Assert.That(result.Description, Is.EqualTo("My test resource"));
            });
        }

        [Test]
        public async Task TestUpdateAsync_WhenResourceGiven_ThenResourceIsUpdated()
        {
            var resourceDto = new ResourceDto(
                id: "MyResource",
                description: "My test resource");
            var resourceRepository = new ResourcesRepository(this.IdentityContext);
            await resourceRepository.AddAsync(resourceDto);
            resourceDto = new ResourceDto(
                id: "MyResource",
                description: "My test resource 2");

            await resourceRepository.UpdateAsync(resourceDto);

            ResourceDto result = await resourceRepository.GetAsync("MyResource");

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo("MyResource"));
                Assert.That(result.Description, Is.EqualTo("My test resource 2"));
            });
        }

        [Test]
        public async Task TestRemoveAsync_WhenResourceGiven_ThenResourceIsRemoved()
        {
            var resourceDto = new ResourceDto(
                id: "MyResource",
                description: "My test resource");
            var resourceRepository = new ResourcesRepository(this.IdentityContext);
            await resourceRepository.AddAsync(resourceDto);

            await resourceRepository.RemoveAsync(resourceDto);

            ResourceDto result = await resourceRepository.GetAsync("MyResource");

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TestGetAsync_WhenResourceIdGiven_ThenResourceIsReturned()
        {
            var resourceDto = new ResourceDto(
                id: "MyResource",
                description: "My test resource");
            var resourceRepository = new ResourcesRepository(this.IdentityContext);
            await resourceRepository.AddAsync(resourceDto);

            ResourceDto result = await resourceRepository.GetAsync("MyResource");

            Assert.That(result, Is.EqualTo(resourceDto));
        }

        [TestCaseSource(nameof(PaginatedAsyncGetTestData))]
        public async Task TestGetAsync_WhenPaginationGiven_ThenResourcesAreReturned(
            IEnumerable<ResourceDto> resources,
            Pagination pagination,
            IEnumerable<ResourceDto> expectedResources)
        {
            var resourceDto = new ResourceDto(
                id: "MyResource",
                description: "My test resource");
            var resourceRepository = new ResourcesRepository(this.IdentityContext);
            resources.ToList().ForEach(r => resourceRepository.AddAsync(r).Wait());

            IEnumerable<ResourceDto> result = await resourceRepository.GetAsync(pagination);

            Assert.That(result, Is.EquivalentTo(expectedResources));
        }
    }
}