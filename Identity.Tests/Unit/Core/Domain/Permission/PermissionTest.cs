using Identity.Core.Domain;
using Identity.Tests.Unit.Core.Domain.Builders;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class PermissionTest
    {
        [Test]
        public void TestConstructor_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            Permission permission = new PermissionBuilder()
                .WithDescription("It allows user to grant permission to other users.")
                .Build();

            Assert.That(permission.Description, Is.EqualTo("It allows user to grant permission to other users."));
        }

        [Test]
        public void TestConstructor_WhenEmptyDescriptionGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Description can't be empty."),
                () => new Permission(
                    id: new PermissionId(
                        resourceId: new ResourceId("MyResource"),
                        name: "GrantPermission"),
                    description: string.Empty));
        }

        [Test]
        public void TestConstructor_WhenNullDescriptionGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("description"),
                () => new Permission(
                    id: new PermissionId(
                        resourceId: new ResourceId("MyResource"),
                        name: "GrantPermission"),
                    description: null));
        }
    }
}