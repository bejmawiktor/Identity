using DDD.Domain.Persistence;
using Identity.Persistence.MSSQL;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Application;
using System;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    public class ApplicationRepositoryTest : DatabaseTestBase
    {
        private static readonly ApplicationDto[] ApplicationsTestData = new ApplicationDto[]
        {
            new ApplicationDto(
                id: Guid.NewGuid(),
                userId: Guid.NewGuid(),
                name: "MyApplication1",
                homepageUrl: "http://www.example1.com",
                callbackUrl: "http://www.example1.com/1"
            ),
            new ApplicationDto(
                id: Guid.NewGuid(),
                userId: Guid.NewGuid(),
                name: "MyApplication2",
                homepageUrl: "http://www.example2.com",
                callbackUrl: "http://www.example2.com/1"
            ),
            new ApplicationDto(
                id: Guid.NewGuid(),
                userId: Guid.NewGuid(),
                name: "MyApplication3",
                homepageUrl: "http://www.example3.com",
                callbackUrl: "http://www.example3.com/1"
            ),
            new ApplicationDto(
                id: Guid.NewGuid(),
                userId: Guid.NewGuid(),
                name: "MyApplication4",
                homepageUrl: "http://www.example4.com",
                callbackUrl: "http://www.example4.com/1"
            ),
            new ApplicationDto(
                id: Guid.NewGuid(),
                userId: Guid.NewGuid(),
                name: "MyApplication5",
                homepageUrl: "http://www.example5.com",
                callbackUrl: "http://www.example5.com/1"
            )
        };

        public static IEnumerable<TestCaseData> PaginatedGetTestData
        {
            get
            {
                yield return new TestCaseData(new object[]
                {
                    ApplicationRepositoryTest.ApplicationsTestData,
                    new Pagination(0, 5),
                    ApplicationRepositoryTest.ApplicationsTestData
                }).SetName($"{nameof(TestGet_WhenPaginationGiven_ThenApplicationsAreReturned)}(1)");
                yield return new TestCaseData(new object[]
                {
                    ApplicationRepositoryTest.ApplicationsTestData,
                    new Pagination(0, 2),
                    new ApplicationDto[]
                    {
                        ApplicationRepositoryTest.ApplicationsTestData[0],
                        ApplicationRepositoryTest.ApplicationsTestData[1],
                    }
                }).SetName($"{nameof(TestGet_WhenPaginationGiven_ThenApplicationsAreReturned)}(2)");
                yield return new TestCaseData(new object[]
                {
                    ApplicationRepositoryTest.ApplicationsTestData,
                    new Pagination(1, 2),
                    new ApplicationDto[]
                    {
                        ApplicationRepositoryTest.ApplicationsTestData[2],
                        ApplicationRepositoryTest.ApplicationsTestData[3],
                    }
                }).SetName($"{nameof(TestGet_WhenPaginationGiven_ThenApplicationsAreReturned)}(3)");
                yield return new TestCaseData(new object[]
                {
                    ApplicationRepositoryTest.ApplicationsTestData,
                    new Pagination(2, 2),
                    new ApplicationDto[]
                    {
                        ApplicationRepositoryTest.ApplicationsTestData[4]
                    }
                }).SetName($"{nameof(TestGet_WhenPaginationGiven_ThenApplicationsAreReturned)}(4)");
                yield return new TestCaseData(new object[]
                {
                    ApplicationRepositoryTest.ApplicationsTestData,
                    new Pagination(3, 2),
                    Enumerable.Empty<ApplicationDto>()
                }).SetName($"{nameof(TestGet_WhenPaginationGiven_ThenApplicationsAreReturned)}(5)");
            }
        }

        public static IEnumerable<TestCaseData> PaginatedAsyncGetTestData
        {
            get
            {
                yield return new TestCaseData(new object[]
                {
                    ApplicationRepositoryTest.ApplicationsTestData,
                    new Pagination(0, 5),
                    ApplicationRepositoryTest.ApplicationsTestData
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenApplicationsAreReturned)}(1)");
                yield return new TestCaseData(new object[]
                {
                    ApplicationRepositoryTest.ApplicationsTestData,
                    new Pagination(0, 2),
                    new ApplicationDto[]
                    {
                        ApplicationRepositoryTest.ApplicationsTestData[0],
                        ApplicationRepositoryTest.ApplicationsTestData[1],
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenApplicationsAreReturned)}(2)");
                yield return new TestCaseData(new object[]
                {
                    ApplicationRepositoryTest.ApplicationsTestData,
                    new Pagination(1, 2),
                    new ApplicationDto[]
                    {
                        ApplicationRepositoryTest.ApplicationsTestData[2],
                        ApplicationRepositoryTest.ApplicationsTestData[3],
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenApplicationsAreReturned)}(3)");
                yield return new TestCaseData(new object[]
                {
                    ApplicationRepositoryTest.ApplicationsTestData,
                    new Pagination(2, 2),
                    new ApplicationDto[]
                    {
                        ApplicationRepositoryTest.ApplicationsTestData[4],
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenApplicationsAreReturned)}(4)");
                yield return new TestCaseData(new object[]
                {
                    ApplicationRepositoryTest.ApplicationsTestData,
                    new Pagination(3, 2),
                    Enumerable.Empty<ApplicationDto>()
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenApplicationsAreReturned)}(5)");
            }
        }

        [Test]
        public void TestAdd_WhenApplicationGiven_ThenApplicationIsStored()
        {
            var applicationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var applicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApplication1",
                homepageUrl: "http://www.example1.com",
                callbackUrl: "http://www.example1.com/1");
            var applicationRepository = new ApplicationsRepository(this.IdentityContext);

            applicationRepository.Add(applicationDto);

            ApplicationDto result = applicationRepository.Get(applicationId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(applicationId));
                Assert.That(result.UserId, Is.EqualTo(userId));
                Assert.That(result.Name, Is.EqualTo("MyApplication1"));
                Assert.That(result.HomepageUrl, Is.EqualTo("http://www.example1.com"));
                Assert.That(result.CallbackUrl, Is.EqualTo("http://www.example1.com/1"));
            });
        }

        [Test]
        public async Task TestAddAsync_WhenApplicationGiven_ThenApplicationIsStored()
        {
            var applicationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var applicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApplication1",
                homepageUrl: "http://www.example1.com",
                callbackUrl: "http://www.example1.com/1");
            var applicationRepository = new ApplicationsRepository(this.IdentityContext);

            await applicationRepository.AddAsync(applicationDto);

            ApplicationDto result = applicationRepository.Get(applicationId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(applicationId));
                Assert.That(result.UserId, Is.EqualTo(userId));
                Assert.That(result.Name, Is.EqualTo("MyApplication1"));
                Assert.That(result.HomepageUrl, Is.EqualTo("http://www.example1.com"));
                Assert.That(result.CallbackUrl, Is.EqualTo("http://www.example1.com/1"));
            });
        }

        [Test]
        public void TestUpdate_WhenApplicationGiven_ThenApplicationIsUpdated()
        {
            var applicationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var applicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApplication1",
                homepageUrl: "http://www.example1.com",
                callbackUrl: "http://www.example1.com/1");
            var applicationRepository = new ApplicationsRepository(this.IdentityContext);
            applicationRepository.Add(applicationDto);
            applicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApplication2",
                homepageUrl: "http://www.example1.com",
                callbackUrl: "http://www.example1.com/1");

            applicationRepository.Update(applicationDto);

            ApplicationDto result = applicationRepository.Get(applicationId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(applicationId));
                Assert.That(result.UserId, Is.EqualTo(userId));
                Assert.That(result.Name, Is.EqualTo("MyApplication2"));
                Assert.That(result.HomepageUrl, Is.EqualTo("http://www.example1.com"));
                Assert.That(result.CallbackUrl, Is.EqualTo("http://www.example1.com/1"));
            });
        }

        [Test]
        public async Task TestUpdateAsync_WhenApplicationGiven_ThenApplicationIsUpdated()
        {
            var applicationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var applicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApplication1",
                homepageUrl: "http://www.example1.com",
                callbackUrl: "http://www.example1.com/1");
            var applicationRepository = new ApplicationsRepository(this.IdentityContext);
            applicationRepository.Add(applicationDto);
            applicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApplication2",
                homepageUrl: "http://www.example1.com",
                callbackUrl: "http://www.example1.com/1");

            await applicationRepository.UpdateAsync(applicationDto);

            ApplicationDto result = applicationRepository.Get(applicationId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(applicationId));
                Assert.That(result.UserId, Is.EqualTo(userId));
                Assert.That(result.Name, Is.EqualTo("MyApplication2"));
                Assert.That(result.HomepageUrl, Is.EqualTo("http://www.example1.com"));
                Assert.That(result.CallbackUrl, Is.EqualTo("http://www.example1.com/1"));
            });
        }

        [Test]
        public void TestRemove_WhenApplicationGiven_ThenApplicationIsRemoved()
        {
            var applicationId = Guid.NewGuid();
            var applicationDto = new ApplicationDto(
                id: applicationId,
                userId: Guid.NewGuid(),
                name: "MyApplication1",
                homepageUrl: "http://www.example1.com",
                callbackUrl: "http://www.example1.com/1");
            var applicationRepository = new ApplicationsRepository(this.IdentityContext);
            applicationRepository.Add(applicationDto);

            applicationRepository.Remove(applicationDto);

            ApplicationDto result = applicationRepository.Get(applicationId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TestRemoveAsync_WhenApplicationGiven_ThenApplicationIsRemoved()
        {
            var applicationId = Guid.NewGuid();
            var applicationDto = new ApplicationDto(
                id: applicationId,
                userId: Guid.NewGuid(),
                name: "MyApplication1",
                homepageUrl: "http://www.example1.com",
                callbackUrl: "http://www.example1.com/1");
            var applicationRepository = new ApplicationsRepository(this.IdentityContext);
            applicationRepository.Add(applicationDto);

            await applicationRepository.RemoveAsync(applicationDto);

            ApplicationDto result = applicationRepository.Get(applicationId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void TestGet_WhenApplicationIdGiven_ThenApplicationIsReturned()
        {
            var applicationId = Guid.NewGuid();
            var applicationDto = new ApplicationDto(
                id: applicationId,
                userId: Guid.NewGuid(),
                name: "MyApplication1",
                homepageUrl: "http://www.example1.com",
                callbackUrl: "http://www.example1.com/1");
            var applicationRepository = new ApplicationsRepository(this.IdentityContext);
            applicationRepository.Add(applicationDto);

            ApplicationDto result = applicationRepository.Get(applicationId);

            Assert.That(result, Is.EqualTo(applicationDto));
        }

        [Test]
        public async Task TestGetAsync_WhenApplicationIdGiven_ThenApplicationIsReturned()
        {
            var applicationId = Guid.NewGuid();
            var applicationDto = new ApplicationDto(
                id: applicationId,
                userId: Guid.NewGuid(),
                name: "MyApplication1",
                homepageUrl: "http://www.example1.com",
                callbackUrl: "http://www.example1.com/1");
            var applicationRepository = new ApplicationsRepository(this.IdentityContext);
            applicationRepository.Add(applicationDto);

            ApplicationDto result = await applicationRepository.GetAsync(applicationId);

            Assert.That(result, Is.EqualTo(applicationDto));
        }

        [TestCaseSource(nameof(PaginatedGetTestData))]
        public void TestGet_WhenPaginationGiven_ThenApplicationsAreReturned(
            IEnumerable<ApplicationDto> applications,
            Pagination pagination,
            IEnumerable<ApplicationDto> expectedApplications)
        {
            var applicationRepository = new ApplicationsRepository(this.IdentityContext);
            applications.ToList().ForEach(r => applicationRepository.Add(r));

            IEnumerable<ApplicationDto> result = applicationRepository.Get(pagination);

            Assert.That(result, Is.EquivalentTo(expectedApplications));
        }

        [TestCaseSource(nameof(PaginatedAsyncGetTestData))]
        public async Task TestGetAsync_WhenPaginationGiven_ThenApplicationsAreReturned(
            IEnumerable<ApplicationDto> applications,
            Pagination pagination,
            IEnumerable<ApplicationDto> expectedApplications)
        {
            var applicationRepository = new ApplicationsRepository(this.IdentityContext);
            applications.ToList().ForEach(r => applicationRepository.Add(r));

            IEnumerable<ApplicationDto> result = await applicationRepository.GetAsync(pagination);

            Assert.That(result, Is.EquivalentTo(expectedApplications));
        }
    }
}
