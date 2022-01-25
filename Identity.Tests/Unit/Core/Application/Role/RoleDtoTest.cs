using Identity.Core.Application;
using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Application.Builders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Tests.Unit.Core.Application
{
    [TestFixture]
    public class RoleDtoTest
    {
        [Test]
        public void TestConstructor_WhenIdGiven_ThenIdIsSet()
        {
            Guid id = Guid.NewGuid();
            RoleDto roleDto = new RoleDtoBuilder()
                .WithId(id)
                .Build();

            Assert.That(roleDto.Id, Is.EqualTo(id));
        }

        [Test]
        public void TestConstructor_WhenNameGiven_ThenNameIsSet()
        {
            RoleDto roleDto = new RoleDtoBuilder()
                .WithName("MyRole")
                .Build();

            Assert.That(roleDto.Name, Is.EqualTo("MyRole"));
        }

        [Test]
        public void TestConstructor_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            RoleDto roleDto = new RoleDtoBuilder()
                .WithDescription("My role description")
                .Build();

            Assert.That(roleDto.Description, Is.EqualTo("My role description"));
        }

        [Test]
        public void TestConstructor_WhenPermissionsGiven_ThenPermissionsAreSet()
        {
            (string ResourceId, string Name)[] permissions = new (string ResourceId, string Name)[]
            {
                ("MyResource", "MyPermission")
            };
            RoleDto roleDto = new RoleDtoBuilder()
                .WithPermissions(permissions)
                .Build();

            Assert.That(roleDto.Permissions, Is.EquivalentTo(permissions));
        }

        [Test]
        public void TestConstructor_WhenNullPermissionsGiven_ThenEmptyPermissionsIsSet()
        {
            RoleDto roleDto = new RoleDtoBuilder()
                .WithPermissions(null)
                .Build();

            Assert.That(roleDto.Permissions, Is.EquivalentTo(Enumerable.Empty<(string ResourceId, string Name)>()));
        }

        [Test]
        public void TestToRole_WhenConvertingToRole_ThenRoleIsReturned()
        {
            Guid id = Guid.NewGuid();
            RoleDto roleDto = RoleDtoBuilder.DefaultRoleDto;

            Role role = roleDto.ToRole();

            Assert.Multiple(() =>
            {
                Assert.That(role.Id, Is.EqualTo(new RoleId(roleDto.Id)));
                Assert.That(role.Name, Is.EqualTo(roleDto.Name));
                Assert.That(role.Description, Is.EqualTo(roleDto.Description));
                Assert.That(role.Permissions, Is.EquivalentTo(RoleDtoBuilder.DefaultPermissions));
            });
        }

        [Test]
        public void TestEquals_WhenTwoIdentitcalRolesDtosGiven_ThenTrueIsReturned()
        {
            Guid id = Guid.NewGuid();
            RoleDto leftRoleDto = new RoleDtoBuilder()
                .WithId(id)
                .Build();
            RoleDto rightRoleDto = new RoleDtoBuilder()
                .WithId(id)
                .Build();

            Assert.That(leftRoleDto.Equals(rightRoleDto), Is.True);
        }

        [Test]
        public void TestEquals_WhenTwoDifferentRolesDtosGiven_ThenFalseIsReturned()
        {
            Guid id = Guid.NewGuid();
            RoleDto leftRoleDto = new RoleDtoBuilder()
                .WithId(id)
                .WithPermissions(new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission"),
                    ("MyResource2", "MyPermission2")
                })
                .Build();
            RoleDto rightRoleDto = new RoleDtoBuilder()
                .WithId(id)
                .WithPermissions(new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission"),
                })
                .Build();

            Assert.That(leftRoleDto.Equals(rightRoleDto), Is.False);
        }

        [Test]
        public void TestGetHashCode_WhenTwoIdenticalRolesDtosGiven_ThenSameHashCodesAreReturned()
        {
            Guid id = Guid.NewGuid();
            RoleDto leftRoleDto = new RoleDtoBuilder()
                .WithId(id)
                .Build();
            RoleDto rightRoleDto = new RoleDtoBuilder()
                .WithId(id)
                .Build();

            Assert.That(leftRoleDto.GetHashCode(), Is.EqualTo(rightRoleDto.GetHashCode()));
        }

        [Test]
        public void TestGetHashCode_WhenTwoDifferentRolesDtosGiven_ThenDifferentHashCodesAreReturned()
        {
            Guid id = Guid.NewGuid();
            RoleDto leftRoleDto = new RoleDtoBuilder()
                .WithId(id)
                .WithPermissions(new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission"),
                    ("MyResource2", "MyPermission2")
                })
                .Build();
            RoleDto rightRoleDto = new RoleDtoBuilder()
                .WithId(id)
                .WithPermissions(new (string ResourceId, string Name)[]
                {
                    ("MyResource", "MyPermission"),
                })
                .Build();

            Assert.That(leftRoleDto.GetHashCode(), Is.Not.EqualTo(rightRoleDto.GetHashCode()));
        }
    }
}