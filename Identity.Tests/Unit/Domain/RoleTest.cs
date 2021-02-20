using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class RoleTest
    {
        [Test]
        public void TestConstructor_WhenNullNameGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("name"),
                () => new Role(
                    id: RoleId.Generate(),
                    name: null,
                    description: "My role description."));
        }

        [Test]
        public void TestConstructor_WhenEmptyNameGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Name can't be empty."),
                () => new Role(
                    id: RoleId.Generate(),
                    name: string.Empty,
                    description: "My role description."));
        }

        [Test]
        public void TestConstructor_WhenNameGiven_ThenNameIsSet()
        {
            var role = new Role(
                id: RoleId.Generate(),
                name: "Role name",
                description: "My role description.");

            Assert.That(role.Name, Is.EqualTo("Role name"));
        }

        [Test]
        public void TestConstructor_WhenNullDescriptionGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("description"),
                () => new Role(
                    id: RoleId.Generate(),
                    name: "Role name",
                    description: null));
        }

        [Test]
        public void TestConstructor_WhenEmptyDescriptionGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Description can't be empty."),
                () => new Role(
                    id: RoleId.Generate(),
                    name: "Role name",
                    description: string.Empty));
        }

        [Test]
        public void TestConstructor_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            var role = new Role(
                id: RoleId.Generate(),
                name: "Role name",
                description: "My role description.");

            Assert.That(role.Description, Is.EqualTo("My role description."));
        }

        [Test]
        public void TestNameSet_WhenNullNameGiven_ThenArgumentNullExceptionIsThrown()
        {
            var role = new Role(
                id: RoleId.Generate(),
                name: "My role.",
                description: "My role description.");

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("name"),
                () => role.Name = null);
        }

        [Test]
        public void TestDescriptionSet_WhenEmptyNameGiven_ThenArgumentExceptionIsThrown()
        {
            var role = new Role(
                id: RoleId.Generate(),
                name: "My role.",
                description: "My role description.");

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Name can't be empty."),
                () => role.Name = string.Empty);
        }

        [Test]
        public void TestDescriptionSet_WhenNameGiven_ThenNameIsSet()
        {
            var role = new Role(
                id: RoleId.Generate(),
                name: "Role name",
                description: "My role description.");

            role.Name = "My role";

            Assert.That(role.Name, Is.EqualTo("My role"));
        }

        [Test]
        public void TestDescriptionSet_WhenNullDescriptionGiven_ThenArgumentNullExceptionIsThrown()
        {
            var role = new Role(
                id: RoleId.Generate(),
                name: "Role name",
                description: "My role description.");

            Assert.Throws(
                Is.InstanceOf<ArgumentNullException>()
                    .And.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("description"),
                () => role.Description = null);
        }

        [Test]
        public void TestDescriptionSet_WhenEmptyDescriptionGiven_ThenArgumentExceptionIsThrown()
        {
            var role = new Role(
                id: RoleId.Generate(),
                name: "Role name",
                description: "My role description.");

            Assert.Throws(
                Is.InstanceOf<ArgumentException>()
                    .And.Message
                    .EqualTo("Description can't be empty."),
                () => role.Description = string.Empty);
        }

        [Test]
        public void TestDescriptionSet_WhenDescriptionGiven_ThenDescriptionIsSet()
        {
            var role = new Role(
                id: RoleId.Generate(),
                name: "Role name",
                description: "My role description.");

            role.Description = "My changed description.";

            Assert.That(role.Description, Is.EqualTo("My changed description."));
        }
    }
}