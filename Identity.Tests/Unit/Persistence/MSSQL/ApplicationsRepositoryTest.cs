﻿using DDD.Domain.Persistence;
using Identity.Application;
using Identity.Persistence.MSSQL;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    public class ApplicationsRepositoryTest : DatabaseTestBase
    {
        private static readonly string SecretKey = Identity.Domain.SecretKey.Generate().ToString();

        private static readonly ApplicationDto[] ApplicationsTestData = new ApplicationDto[]
        {
            new ApplicationDto(
                id: Guid.NewGuid(),
                userId: Guid.NewGuid(),
                name: "MyApplication1",
                secretKey: Identity.Domain.SecretKey.Generate().ToString(),
                homepageUrl: "http://www.example1.com",
                callbackUrl: "http://www.example1.com/1"
            ),
            new ApplicationDto(
                id: Guid.NewGuid(),
                userId: Guid.NewGuid(),
                name: "MyApplication2",
                secretKey: Identity.Domain.SecretKey.Generate().ToString(),
                homepageUrl: "http://www.example2.com",
                callbackUrl: "http://www.example2.com/1"
            ),
            new ApplicationDto(
                id: Guid.NewGuid(),
                userId: Guid.NewGuid(),
                name: "MyApplication3",
                secretKey: Identity.Domain.SecretKey.Generate().ToString(),
                homepageUrl: "http://www.example3.com",
                callbackUrl: "http://www.example3.com/1"
            ),
            new ApplicationDto(
                id: Guid.NewGuid(),
                userId: Guid.NewGuid(),
                name: "MyApplication4",
                secretKey: Identity.Domain.SecretKey.Generate().ToString(),
                homepageUrl: "http://www.example4.com",
                callbackUrl: "http://www.example4.com/1"
            ),
            new ApplicationDto(
                id: Guid.NewGuid(),
                userId: Guid.NewGuid(),
                name: "MyApplication5",
                secretKey: Identity.Domain.SecretKey.Generate().ToString(),
                homepageUrl: "http://www.example5.com",
                callbackUrl: "http://www.example5.com/1"
            )
        };

        public static IEnumerable<TestCaseData> PaginatedAsyncGetTestData
        {
            get
            {
                yield return new TestCaseData(new object[]
                {
                    ApplicationsRepositoryTest.ApplicationsTestData,
                    new Pagination(0, 5),
                    ApplicationsRepositoryTest.ApplicationsTestData
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenApplicationsAreReturned)}(1)");
                yield return new TestCaseData(new object[]
                {
                    ApplicationsRepositoryTest.ApplicationsTestData,
                    new Pagination(0, 2),
                    new ApplicationDto[]
                    {
                        ApplicationsRepositoryTest.ApplicationsTestData[0],
                        ApplicationsRepositoryTest.ApplicationsTestData[1],
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenApplicationsAreReturned)}(2)");
                yield return new TestCaseData(new object[]
                {
                    ApplicationsRepositoryTest.ApplicationsTestData,
                    new Pagination(1, 2),
                    new ApplicationDto[]
                    {
                        ApplicationsRepositoryTest.ApplicationsTestData[2],
                        ApplicationsRepositoryTest.ApplicationsTestData[3],
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenApplicationsAreReturned)}(3)");
                yield return new TestCaseData(new object[]
                {
                    ApplicationsRepositoryTest.ApplicationsTestData,
                    new Pagination(2, 2),
                    new ApplicationDto[]
                    {
                        ApplicationsRepositoryTest.ApplicationsTestData[4],
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenApplicationsAreReturned)}(4)");
                yield return new TestCaseData(new object[]
                {
                    ApplicationsRepositoryTest.ApplicationsTestData,
                    new Pagination(3, 2),
                    Enumerable.Empty<ApplicationDto>()
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenApplicationsAreReturned)}(5)");
            }
        }

        [Test]
        public async Task TestAddAsync_WhenApplicationGiven_ThenApplicationIsStored()
        {
            Guid applicationId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            var applicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApplication1",
                secretKey: SecretKey,
                homepageUrl: "http://www.example1.com",
                callbackUrl: "http://www.example1.com/1");
            var applicationRepository = new ApplicationsRepository(this.IdentityContext);

            await applicationRepository.AddAsync(applicationDto);

            ApplicationDto result = await applicationRepository.GetAsync(applicationId);

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
        public async Task TestUpdateAsync_WhenApplicationGiven_ThenApplicationIsUpdated()
        {
            Guid applicationId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            var applicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApplication1",
                secretKey: SecretKey,
                homepageUrl: "http://www.example1.com",
                callbackUrl: "http://www.example1.com/1");
            var applicationRepository = new ApplicationsRepository(this.IdentityContext);
            await applicationRepository.AddAsync(applicationDto);
            applicationDto = new ApplicationDto(
                id: applicationId,
                userId: userId,
                name: "MyApplication2",
                secretKey: SecretKey,
                homepageUrl: "http://www.example1.com",
                callbackUrl: "http://www.example1.com/1");

            await applicationRepository.UpdateAsync(applicationDto);

            ApplicationDto result = await applicationRepository.GetAsync(applicationId);

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
        public async Task TestRemoveAsync_WhenApplicationGiven_ThenApplicationIsRemoved()
        {
            Guid applicationId = Guid.NewGuid();
            var applicationDto = new ApplicationDto(
                id: applicationId,
                userId: Guid.NewGuid(),
                name: "MyApplication1",
                secretKey: SecretKey,
                homepageUrl: "http://www.example1.com",
                callbackUrl: "http://www.example1.com/1");
            var applicationRepository = new ApplicationsRepository(this.IdentityContext);
            await applicationRepository.AddAsync(applicationDto);

            await applicationRepository.RemoveAsync(applicationDto);

            ApplicationDto result = await applicationRepository.GetAsync(applicationId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TestGetAsync_WhenApplicationIdGiven_ThenApplicationIsReturned()
        {
            Guid applicationId = Guid.NewGuid();
            var applicationDto = new ApplicationDto(
                id: applicationId,
                userId: Guid.NewGuid(),
                name: "MyApplication1",
                secretKey: SecretKey,
                homepageUrl: "http://www.example1.com",
                callbackUrl: "http://www.example1.com/1");
            var applicationRepository = new ApplicationsRepository(this.IdentityContext);
            await applicationRepository.AddAsync(applicationDto);

            ApplicationDto result = await applicationRepository.GetAsync(applicationId);

            Assert.That(result, Is.EqualTo(applicationDto));
        }

        [TestCaseSource(nameof(PaginatedAsyncGetTestData))]
        public async Task TestGetAsync_WhenPaginationGiven_ThenApplicationsAreReturned(
            IEnumerable<ApplicationDto> applications,
            Pagination pagination,
            IEnumerable<ApplicationDto> expectedApplications)
        {
            var applicationRepository = new ApplicationsRepository(this.IdentityContext);
            applications.ToList().ForEach(r => applicationRepository.AddAsync(r).Wait());

            IEnumerable<ApplicationDto> result = await applicationRepository.GetAsync(pagination);

            Assert.That(result, Is.EquivalentTo(expectedApplications));
        }
    }
}