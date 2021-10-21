using DDD.Domain.Persistence;
using Identity.Domain;
using Identity.Persistence.MSSQL;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Tests.Unit.Persistence.MSSQL
{
    using UserDto = Identity.Application.UserDto;

    [TestFixture]
    public class UsersRepositoryTest : DatabaseTestBase
    {
        private static readonly HashedPassword TestPassword = HashedPassword.Hash(new Password("MyPassword1"));

        private static readonly UserDto[] UsersTestData = new UserDto[]
        {
            new UserDto(
                id: Guid.NewGuid(),
                email: "example1@example.com",
                hashedPassword: UsersRepositoryTest.TestPassword.ToString()),
            new UserDto(
                id: Guid.NewGuid(),
                email: "example2@example.com",
                hashedPassword: UsersRepositoryTest.TestPassword.ToString()),
            new UserDto(
                id: Guid.NewGuid(),
                email: "example3@example.com",
                hashedPassword: UsersRepositoryTest.TestPassword.ToString()),
            new UserDto(
                id: Guid.NewGuid(),
                email: "example4@example.com",
                hashedPassword: UsersRepositoryTest.TestPassword.ToString()),
            new UserDto(
                id: Guid.NewGuid(),
                email: "example5@example.com",
                hashedPassword: UsersRepositoryTest.TestPassword.ToString()),
        };

        public static IEnumerable<TestCaseData> PaginatedGetTestData
        {
            get
            {
                yield return new TestCaseData(new object[]
                {
                    UsersRepositoryTest.UsersTestData,
                    new Pagination(0, 5),
                    UsersRepositoryTest.UsersTestData
                }).SetName($"{nameof(TestGet_WhenPaginationGiven_ThenUsersAreReturned)}(1)");
                yield return new TestCaseData(new object[]
                {
                    UsersRepositoryTest.UsersTestData,
                    new Pagination(0, 2),
                    new UserDto[]
                    {
                        UsersRepositoryTest.UsersTestData[0],
                        UsersRepositoryTest.UsersTestData[1],
                    }
                }).SetName($"{nameof(TestGet_WhenPaginationGiven_ThenUsersAreReturned)}(2)");
                yield return new TestCaseData(new object[]
                {
                    UsersRepositoryTest.UsersTestData,
                    new Pagination(1, 2),
                    new UserDto[]
                    {
                        UsersRepositoryTest.UsersTestData[2],
                        UsersRepositoryTest.UsersTestData[3],
                    }
                }).SetName($"{nameof(TestGet_WhenPaginationGiven_ThenUsersAreReturned)}(3)");
                yield return new TestCaseData(new object[]
                {
                    UsersRepositoryTest.UsersTestData,
                    new Pagination(2, 2),
                    new UserDto[]
                    {
                        UsersRepositoryTest.UsersTestData[4]
                    }
                }).SetName($"{nameof(TestGet_WhenPaginationGiven_ThenUsersAreReturned)}(4)");
                yield return new TestCaseData(new object[]
                {
                    UsersRepositoryTest.UsersTestData,
                    new Pagination(3, 2),
                    Enumerable.Empty<UserDto>()
                }).SetName($"{nameof(TestGet_WhenPaginationGiven_ThenUsersAreReturned)}(5)");
            }
        }

        public static IEnumerable<TestCaseData> PaginatedAsyncGetTestData
        {
            get
            {
                yield return new TestCaseData(new object[]
                {
                    UsersRepositoryTest.UsersTestData,
                    new Pagination(0, 5),
                    UsersRepositoryTest.UsersTestData
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenUsersAreReturned)}(1)");
                yield return new TestCaseData(new object[]
                {
                    UsersRepositoryTest.UsersTestData,
                    new Pagination(0, 2),
                    new UserDto[]
                    {
                        UsersRepositoryTest.UsersTestData[0],
                        UsersRepositoryTest.UsersTestData[1],
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenUsersAreReturned)}(2)");
                yield return new TestCaseData(new object[]
                {
                    UsersRepositoryTest.UsersTestData,
                    new Pagination(1, 2),
                    new UserDto[]
                    {
                        UsersRepositoryTest.UsersTestData[2],
                        UsersRepositoryTest.UsersTestData[3],
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenUsersAreReturned)}(3)");
                yield return new TestCaseData(new object[]
                {
                    UsersRepositoryTest.UsersTestData,
                    new Pagination(2, 2),
                    new UserDto[]
                    {
                        UsersRepositoryTest.UsersTestData[4],
                    }
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenUsersAreReturned)}(4)");
                yield return new TestCaseData(new object[]
                {
                    UsersRepositoryTest.UsersTestData,
                    new Pagination(3, 2),
                    Enumerable.Empty<UserDto>()
                }).SetName($"{nameof(TestGetAsync_WhenPaginationGiven_ThenUsersAreReturned)}(5)");
            }
        }

        [Test]
        public void TestAdd_WhenUserGiven_ThenUserIsStored()
        {
            var userId = Guid.NewGuid();
            var roles = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var userDto = new UserDto(
                id: userId,
                email: "example1@example.com",
                hashedPassword: UsersRepositoryTest.TestPassword.ToString(),
                roles: roles,
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                });
            var userRepository = new UsersRepository(this.IdentityContext);

            userRepository.Add(userDto);

            UserDto result = userRepository.Get(userId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(userId));
                Assert.That(result.Email, Is.EqualTo("example1@example.com"));
                Assert.That(result.HashedPassword, Is.EqualTo(userDto.HashedPassword));
                Assert.That(result.Roles, Is.EqualTo(roles));
                Assert.That(result.Permissions, Is.EqualTo(new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                }));
            });
        }

        [Test]
        public async Task TestAddAsync_WhenUserGiven_ThenUserIsStored()
        {
            var userId = Guid.NewGuid();
            var roles = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var userDto = new UserDto(
                id: userId,
                email: "example1@example.com",
                hashedPassword: UsersRepositoryTest.TestPassword.ToString(),
                roles: roles,
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                });
            var userRepository = new UsersRepository(this.IdentityContext);

            await userRepository.AddAsync(userDto);

            UserDto result = userRepository.Get(userId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(userId));
                Assert.That(result.Email, Is.EqualTo("example1@example.com"));
                Assert.That(result.HashedPassword, Is.EqualTo(userDto.HashedPassword));
                Assert.That(result.Roles, Is.EqualTo(roles));
                Assert.That(result.Permissions, Is.EqualTo(new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                }));
            });
        }

        [Test]
        public void TestUpdate_WhenUserGiven_ThenUserIsUpdated()
        {
            var userId = Guid.NewGuid();
            var roles = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var userDto = new UserDto(
                id: userId,
                email: "example1@example.com",
                hashedPassword: UsersRepositoryTest.TestPassword.ToString(),
                roles: roles,
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                });
            var userRepository = new UsersRepository(this.IdentityContext);
            userRepository.Add(userDto);
            userDto = new UserDto(
                id: userId,
                email: "example2@example.com",
                hashedPassword: HashedPassword.Hash(new Password("MyPassword2")).ToString(),
                roles: new Guid[]
                {
                    roles[0]
                },
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource2", "MyPermission2")
                });

            userRepository.Update(userDto);

            UserDto result = userRepository.Get(userId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(userId));
                Assert.That(result.Email, Is.EqualTo("example2@example.com"));
                Assert.That(result.HashedPassword, Is.EqualTo(userDto.HashedPassword));
                Assert.That(result.Roles, Is.EqualTo(new Guid[]
                {
                    roles[0]
                }));
                Assert.That(result.Permissions, Is.EqualTo(new (string ResourceId, string Name)[]
                {
                    ("MyResource2", "MyPermission2")
                }));
            });
        }

        [Test]
        public async Task TestUpdateAsync_WhenUserGiven_ThenUserIsUpdated()
        {
            var userId = Guid.NewGuid();
            var roles = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var userDto = new UserDto(
                id: userId,
                email: "example1@example.com",
                hashedPassword: UsersRepositoryTest.TestPassword.ToString(),
                roles: roles,
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                });
            var userRepository = new UsersRepository(this.IdentityContext);
            userRepository.Add(userDto);
            userDto = new UserDto(
                id: userId,
                email: "example2@example.com",
                hashedPassword: HashedPassword.Hash(new Password("MyPassword2")).ToString(),
                roles: new Guid[]
                {
                    roles[0]
                },
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource2", "MyPermission2")
                });

            await userRepository.UpdateAsync(userDto);

            UserDto result = userRepository.Get(userId);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(userId));
                Assert.That(result.Email, Is.EqualTo("example2@example.com"));
                Assert.That(result.HashedPassword, Is.EqualTo(userDto.HashedPassword));
                Assert.That(result.Roles, Is.EqualTo(new Guid[]
                {
                    roles[0]
                }));
                Assert.That(result.Permissions, Is.EqualTo(new (string ResourceId, string Name)[]
                {
                    ("MyResource2", "MyPermission2")
                }));
            });
        }

        [Test]
        public void TestRemove_WhenUserGiven_ThenUserIsRemoved()
        {
            var userId = Guid.NewGuid();
            var roles = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var userDto = new UserDto(
                id: userId,
                email: "example1@example.com",
                hashedPassword: UsersRepositoryTest.TestPassword.ToString(),
                roles: roles,
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                });
            var userRepository = new UsersRepository(this.IdentityContext);
            userRepository.Add(userDto);

            userRepository.Remove(userDto);

            UserDto result = userRepository.Get(userId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TestRemoveAsync_WhenUserGiven_ThenUserIsRemoved()
        {
            var userId = Guid.NewGuid();
            var roles = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var userDto = new UserDto(
                id: userId,
                email: "example1@example.com",
                hashedPassword: UsersRepositoryTest.TestPassword.ToString(),
                roles: roles,
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                });
            var userRepository = new UsersRepository(this.IdentityContext);
            userRepository.Add(userDto);

            await userRepository.RemoveAsync(userDto);

            UserDto result = userRepository.Get(userId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void TestGet_WhenUserIdGiven_ThenUserIsReturned()
        {
            var userId = Guid.NewGuid();
            var roles = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var userDto = new UserDto(
                id: userId,
                email: "example1@example.com",
                hashedPassword: UsersRepositoryTest.TestPassword.ToString(),
                roles: roles,
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                });
            var userRepository = new UsersRepository(this.IdentityContext);
            userRepository.Add(userDto);

            UserDto result = userRepository.Get(userId);

            Assert.That(result, Is.EqualTo(userDto));
        }

        [Test]
        public async Task TestGetAsync_WhenUserIdGiven_ThenUserIsReturned()
        {
            var userId = Guid.NewGuid();
            var roles = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var userDto = new UserDto(
                id: userId,
                email: "example1@example.com",
                hashedPassword: UsersRepositoryTest.TestPassword.ToString(),
                roles: roles,
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                });
            var userRepository = new UsersRepository(this.IdentityContext);
            userRepository.Add(userDto);

            UserDto result = await userRepository.GetAsync(userId);

            Assert.That(result, Is.EqualTo(userDto));
        }

        [TestCaseSource(nameof(PaginatedGetTestData))]
        public void TestGet_WhenPaginationGiven_ThenUsersAreReturned(
            IEnumerable<UserDto> users,
            Pagination pagination,
            IEnumerable<UserDto> expectedUsers)
        {
            var userRepository = new UsersRepository(this.IdentityContext);
            users.ToList().ForEach(r => userRepository.Add(r));

            IEnumerable<UserDto> result = userRepository.Get(pagination);

            Assert.That(result, Is.EquivalentTo(expectedUsers));
        }

        [TestCaseSource(nameof(PaginatedAsyncGetTestData))]
        public async Task TestGetAsync_WhenPaginationGiven_ThenUsersAreReturned(
            IEnumerable<UserDto> users,
            Pagination pagination,
            IEnumerable<UserDto> expectedUsers)
        {
            var userRepository = new UsersRepository(this.IdentityContext);
            users.ToList().ForEach(r => userRepository.Add(r));

            IEnumerable<UserDto> result = await userRepository.GetAsync(pagination);

            Assert.That(result, Is.EquivalentTo(expectedUsers));
        }

        [Test]
        public void TestGet_WhenEmailAddressGiven_ThenUserIsReturned()
        {
            var userId = Guid.NewGuid();
            var roles = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var userDto = new UserDto(
                id: userId,
                email: "example1@example.com",
                hashedPassword: UsersRepositoryTest.TestPassword.ToString(),
                roles: roles,
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                });
            var userRepository = new UsersRepository(this.IdentityContext);
            userRepository.Add(userDto);

            UserDto result = userRepository.Get("example1@example.com");

            Assert.That(result, Is.EqualTo(userDto));
        }

        [Test]
        public async Task TestGetAsync_WhenEmailAddressGiven_ThenUserIsReturned()
        {
            var userId = Guid.NewGuid();
            var roles = new Guid[]
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var userDto = new UserDto(
                id: userId,
                email: "example1@example.com",
                hashedPassword: UsersRepositoryTest.TestPassword.ToString(),
                roles: roles,
                permissions: new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission")
                });
            var userRepository = new UsersRepository(this.IdentityContext);
            userRepository.Add(userDto);

            UserDto result = await userRepository.GetAsync("example1@example.com");

            Assert.That(result, Is.EqualTo(userDto));
        }
    }
}