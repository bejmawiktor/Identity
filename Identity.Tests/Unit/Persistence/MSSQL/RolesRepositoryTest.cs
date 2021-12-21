using DDD.Domain.Persistence;
using Identity.Persistence.MSSQL;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    using RoleDto = Identity.Core.Application.RoleDto;

    public class RolesRepositoryTest : DatabaseTestBase
    {
        private static readonly RoleDto[] RolesTestData = new RoleDto[]
        {
            new RoleDto(
                id: Guid.NewGuid(),
                name: "MyRole1",
                description: "My role 1 description."),
            new RoleDto(
                id: Guid.NewGuid(),
                name: "MyRole2",
                description: "My role 2 description."),
            new RoleDto(
                id: Guid.NewGuid(),
                name: "MyRole3",
                description: "My role 3 description."),
            new RoleDto(
                id: Guid.NewGuid(),
                name: "MyRole4",
                description: "My role 4 description."),
            new RoleDto(
                id: Guid.NewGuid(),
                name: "MyRole5",
                description: "My role 5 description."),
        };

        public static IEnumerable<TestCaseData> PaginatedAsyncGetTestData
        {
            get
            {
                yield return new TestCaseData(new object[]
                {
                    RolesRepositoryTest.RolesTestData,
                    new Pagination(0, 5),
                    RolesRepositoryTest.RolesTestData
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenRolesAreReturned)}(1)");
                yield return new TestCaseData(new object[]
                {
                    RolesRepositoryTest.RolesTestData,
                    new Pagination(0, 2),
                    new RoleDto[]
                    {
                        RolesRepositoryTest.RolesTestData[0],
                        RolesRepositoryTest.RolesTestData[1],
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenRolesAreReturned)}(2)");
                yield return new TestCaseData(new object[]
                {
                    RolesRepositoryTest.RolesTestData,
                    new Pagination(1, 2),
                    new RoleDto[]
                    {
                        RolesRepositoryTest.RolesTestData[2],
                        RolesRepositoryTest.RolesTestData[3],
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenRolesAreReturned)}(3)");
                yield return new TestCaseData(new object[]
                {
                    RolesRepositoryTest.RolesTestData,
                    new Pagination(2, 2),
                    new RoleDto[]
                    {
                        RolesRepositoryTest.RolesTestData[4],
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenRolesAreReturned)}(4)");
                yield return new TestCaseData(new object[]
                {
                    RolesRepositoryTest.RolesTestData,
                    new Pagination(3, 2),
                    Enumerable.Empty<RoleDto>()
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenRolesAreReturned)}(5)");
            }
        }

        [Test]
        public async Task TestAddAsync_WhenRoleGiven_ThenRoleIsStored()
        {
            var roleId = Guid.NewGuid();
            var roleDto = new RoleDto(
                id: roleId,
                name: "MyRole",
                description: "My role description.",
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                });
            var roleRepository = new RolesRepository(this.IdentityContext);

            await roleRepository.AddAsync(roleDto);

            RoleDto result = await roleRepository.GetAsync(roleId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(roleId));
                Assert.That(result.Name, Is.EqualTo("MyRole"));
                Assert.That(result.Description, Is.EqualTo("My role description."));
                Assert.That(result.Permissions, Is.EqualTo(new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                }));
            });
        }

        [Test]
        public async Task TestUpdateAsync_WhenRoleGiven_ThenRoleIsUpdated()
        {
            var roleId = Guid.NewGuid();
            var roleDto = new RoleDto(
                id: roleId,
                name: "MyRole",
                description: "My role description.",
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                });
            var roleRepository = new RolesRepository(this.IdentityContext);
            await roleRepository.AddAsync(roleDto);
            roleDto = new RoleDto(
                id: roleId,
                name: "MyRole2",
                description: "My role description 2.",
                permissions: Array.Empty<(string ResourceId, string Name)>());

            await roleRepository.UpdateAsync(roleDto);

            RoleDto result = await roleRepository.GetAsync(roleId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(roleId));
                Assert.That(result.Name, Is.EqualTo("MyRole2"));
                Assert.That(result.Description, Is.EqualTo("My role description 2."));
                Assert.That(result.Permissions, Is.Empty);
            });
        }

        [Test]
        public async Task TestRemoveAsync_WhenRoleGiven_ThenRoleIsRemoved()
        {
            var roleId = Guid.NewGuid();
            var roleDto = new RoleDto(
                id: roleId,
                name: "MyRole",
                description: "My role description.",
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                });
            var roleRepository = new RolesRepository(this.IdentityContext);
            await roleRepository.AddAsync(roleDto);

            await roleRepository.RemoveAsync(roleDto);

            RoleDto result = await roleRepository.GetAsync(roleId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TestGetAsync_WhenRoleIdGiven_ThenRoleIsReturned()
        {
            var roleId = Guid.NewGuid();
            var roleDto = new RoleDto(
                id: roleId,
                name: "MyRole",
                description: "My role description.",
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                });
            var roleRepository = new RolesRepository(this.IdentityContext);
            await roleRepository.AddAsync(roleDto);

            RoleDto result = await roleRepository.GetAsync(roleId);

            Assert.That(result, Is.EqualTo(roleDto));
        }

        [TestCaseSource(nameof(PaginatedAsyncGetTestData))]
        public async Task TestGetAsync_WhenPaginationGiven_ThenRolesAreReturned(
            IEnumerable<RoleDto> roles,
            Pagination pagination,
            IEnumerable<RoleDto> expectedRoles)
        {
            var roleRepository = new RolesRepository(this.IdentityContext);
            roles.ToList().ForEach(r => roleRepository.AddAsync(r).Wait());

            IEnumerable<RoleDto> result = await roleRepository.GetAsync(pagination);

            Assert.That(result, Is.EquivalentTo(expectedRoles));
        }
    }
}