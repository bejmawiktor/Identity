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
    public class ResourceRepositoryTest : DatabaseTestBase
    {
        private static IEnumerable<ResourceDto> Resources1 => new ResourceDto[]
        {
            new ResourceDto("MyResource1", "My resource description."),
            new ResourceDto("MyResource2", "My resource description."),
            new ResourceDto("MyResource3", "My resource description."),
            new ResourceDto("MyResource4", "My resource description."),
            new ResourceDto("MyResource5", "My resource description.")
        };

        public static IEnumerable<TestCaseData> PaginatedGetTestData
        {
            get
            {
                yield return new TestCaseData(new object[]
                {
                    ResourceRepositoryTest.Resources1,
                    new Pagination(0, 5),
                    ResourceRepositoryTest.Resources1
                }).SetName($"{nameof(TestGet_WhenPaginationGiven_ThenResourcesAreReturned)}(1)");
                yield return new TestCaseData(new object[]
                {
                    ResourceRepositoryTest.Resources1,
                    new Pagination(0, 2),
                    new ResourceDto[]
                    {
                        new ResourceDto("MyResource1", "My resource description."),
                        new ResourceDto("MyResource2", "My resource description."),
                    }
                }).SetName($"{nameof(TestGet_WhenPaginationGiven_ThenResourcesAreReturned)}(2)");
                yield return new TestCaseData(new object[]
                {
                    ResourceRepositoryTest.Resources1,
                    new Pagination(1, 2),
                    new ResourceDto[]
                    {
                        new ResourceDto("MyResource3", "My resource description."),
                        new ResourceDto("MyResource4", "My resource description."),
                    }
                }).SetName($"{nameof(TestGet_WhenPaginationGiven_ThenResourcesAreReturned)}(3)");
                yield return new TestCaseData(new object[]
                {
                    ResourceRepositoryTest.Resources1,
                    new Pagination(2, 2),
                    new ResourceDto[]
                    {
                        new ResourceDto("MyResource5", "My resource description.")
                    }
                }).SetName($"{nameof(TestGet_WhenPaginationGiven_ThenResourcesAreReturned)}(4)");
                yield return new TestCaseData(new object[]
                {
                    ResourceRepositoryTest.Resources1,
                    new Pagination(3, 2),
                    Enumerable.Empty<ResourceDto>()
                }).SetName($"{nameof(TestGet_WhenPaginationGiven_ThenResourcesAreReturned)}(5)");
            }
        }

        public static IEnumerable<TestCaseData> PaginatedAsyncGetTestData
        {
            get
            {
                yield return new TestCaseData(new object[]
                {
                    ResourceRepositoryTest.Resources1,
                    new Pagination(0, 5),
                    ResourceRepositoryTest.Resources1
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenResourcesAreReturned)}(1)");
                yield return new TestCaseData(new object[]
                {
                    ResourceRepositoryTest.Resources1,
                    new Pagination(0, 2),
                    new ResourceDto[]
                    {
                        new ResourceDto("MyResource1", "My resource description."),
                        new ResourceDto("MyResource2", "My resource description."),
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenResourcesAreReturned)}(2)");
                yield return new TestCaseData(new object[]
                {
                    ResourceRepositoryTest.Resources1,
                    new Pagination(1, 2),
                    new ResourceDto[]
                    {
                        new ResourceDto("MyResource3", "My resource description."),
                        new ResourceDto("MyResource4", "My resource description."),
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenResourcesAreReturned)}(3)");
                yield return new TestCaseData(new object[]
                {
                    ResourceRepositoryTest.Resources1,
                    new Pagination(2, 2),
                    new ResourceDto[]
                    {
                        new ResourceDto("MyResource5", "My resource description.")
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenResourcesAreReturned)}(4)");
                yield return new TestCaseData(new object[]
                {
                    ResourceRepositoryTest.Resources1,
                    new Pagination(3, 2),
                    Enumerable.Empty<ResourceDto>()
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenResourcesAreReturned)}(5)");
            }
        }

        [Test]
        public void TestAdd_WhenResourceGiven_ThenResourceIsStored()
        {
            var resourceDto = new ResourceDto(
                id: "MyResource",
                description: "My test resource");
            var resourceRepository = new ResourcesRepository(this.IdentityContext);

            resourceRepository.Add(resourceDto);

            ResourceDto result = resourceRepository.Get("MyResource");

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo("MyResource"));
                Assert.That(result.Description, Is.EqualTo("My test resource"));
            });
        }

        [Test]
        public async Task TestAddAsync_WhenResourceGiven_ThenResourceIsStored()
        {
            var resourceDto = new ResourceDto(
                id: "MyResource",
                description: "My test resource");
            var resourceRepository = new ResourcesRepository(this.IdentityContext);

            await resourceRepository.AddAsync(resourceDto);

            ResourceDto result = resourceRepository.Get("MyResource");

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo("MyResource"));
                Assert.That(result.Description, Is.EqualTo("My test resource"));
            });
        }

        [Test]
        public void TestUpdate_WhenResourceGiven_ThenResourceIsUpdated()
        {
            var resourceDto = new ResourceDto(
                id: "MyResource",
                description: "My test resource");
            var resourceRepository = new ResourcesRepository(this.IdentityContext);
            resourceRepository.Add(resourceDto);
            resourceDto = new ResourceDto(
                id: "MyResource",
                description: "My test resource 2");

            resourceRepository.Update(resourceDto);

            ResourceDto result = resourceRepository.Get("MyResource");

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo("MyResource"));
                Assert.That(result.Description, Is.EqualTo("My test resource 2"));
            });
        }

        [Test]
        public async Task TestUpdateAsync_WhenResourceGiven_ThenResourceIsUpdated()
        {
            var resourceDto = new ResourceDto(
                id: "MyResource",
                description: "My test resource");
            var resourceRepository = new ResourcesRepository(this.IdentityContext);
            resourceRepository.Add(resourceDto);
            resourceDto = new ResourceDto(
                id: "MyResource",
                description: "My test resource 2");

            await resourceRepository.UpdateAsync(resourceDto);

            ResourceDto result = resourceRepository.Get("MyResource");

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo("MyResource"));
                Assert.That(result.Description, Is.EqualTo("My test resource 2"));
            });
        }

        [Test]
        public void TestRemove_WhenResourceGiven_ThenResourceIsRemoved()
        {
            var resourceDto = new ResourceDto(
                id: "MyResource",
                description: "My test resource");
            var resourceRepository = new ResourcesRepository(this.IdentityContext);
            resourceRepository.Add(resourceDto);
            resourceDto = new ResourceDto(
                id: "MyResource",
                description: "My test resource 2");

            resourceRepository.Remove(resourceDto);

            ResourceDto result = resourceRepository.Get("MyResource");

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TestRemoveAsync_WhenResourceGiven_ThenResourceIsRemoved()
        {
            var resourceDto = new ResourceDto(
                id: "MyResource",
                description: "My test resource");
            var resourceRepository = new ResourcesRepository(this.IdentityContext);
            resourceRepository.Add(resourceDto);
            resourceDto = new ResourceDto(
                id: "MyResource",
                description: "My test resource 2");

            await resourceRepository.RemoveAsync(resourceDto);

            ResourceDto result = resourceRepository.Get("MyResource");

            Assert.That(result, Is.Null);
        }

        [Test]
        public void TestGet_WhenResourceIdGiven_ThenResourceIsReturned()
        {
            var resourceDto = new ResourceDto(
                id: "MyResource",
                description: "My test resource");
            var resourceRepository = new ResourcesRepository(this.IdentityContext);
            resourceRepository.Add(resourceDto);

            ResourceDto result = resourceRepository.Get("MyResource");

            Assert.That(result, Is.EqualTo(resourceDto));
        }

        [Test]
        public async Task TestGetAsync_WhenResourceIdGiven_ThenResourceIsReturned()
        {
            var resourceDto = new ResourceDto(
                id: "MyResource",
                description: "My test resource");
            var resourceRepository = new ResourcesRepository(this.IdentityContext);
            resourceRepository.Add(resourceDto);

            ResourceDto result = await resourceRepository.GetAsync("MyResource");

            Assert.That(result, Is.EqualTo(resourceDto));
        }

        [TestCaseSource(nameof(PaginatedGetTestData))]
        public void TestGet_WhenPaginationGiven_ThenResourcesAreReturned(
            IEnumerable<ResourceDto> resources,
            Pagination pagination,
            IEnumerable<ResourceDto> expectedResources)
        {
            var resourceDto = new ResourceDto(
                id: "MyResource",
                description: "My test resource");
            var resourceRepository = new ResourcesRepository(this.IdentityContext);
            resources.ToList().ForEach(r => resourceRepository.Add(r));

            IEnumerable<ResourceDto> result = resourceRepository.Get(pagination);

            Assert.That(result, Is.EquivalentTo(expectedResources));
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
            resources.ToList().ForEach(r => resourceRepository.Add(r));

            IEnumerable<ResourceDto> result = await resourceRepository.GetAsync(pagination);

            Assert.That(result, Is.EquivalentTo(expectedResources));
        }
    }
}