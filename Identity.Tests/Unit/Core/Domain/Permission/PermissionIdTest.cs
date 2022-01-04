using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class PermissionIdTest
    {
        [Test]
        public void TestConstructor_WhenEmptyNameGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentException>()
                  .And.Message
                  .EqualTo("Name can't be empty."),
              () => new PermissionId(new ResourceId("MyResource"), string.Empty));
        }

        [Test]
        public void TestConstructor_WhenNullResourceIdGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentNullException>()
                  .And.Property(nameof(ArgumentNullException.ParamName))
                  .EqualTo("resourceId"),
              () => new PermissionId(null, "Permission"));
        }

        [Test]
        public void TestConstructor_WhenNullNameGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentNullException>()
                  .And.Property(nameof(ArgumentNullException.ParamName))
                  .EqualTo("name"),
              () => new PermissionId(new ResourceId("MyResource"), null));
        }

        [Test]
        public void TestConstructor_WhenCorrectNameGiven_ThenNameIsSet()
        {
            PermissionId permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");

            Assert.That(permissionId.Name, Is.EqualTo("MyPermission"));
        }

        [Test]
        public void TestConstructor_WhenCorrectResourceIdGiven_ThenResourceIdIsSet()
        {
            PermissionId permissionId = new PermissionId(new ResourceId("MyResource"), "MyPermission");

            Assert.That(permissionId.ResourceId, Is.EqualTo(new ResourceId("MyResource")));
        }

        [TestCase("My permission")]
        [TestCase("My$permission")]
        [TestCase("$%^MyPermission!@#")]
        [TestCase("My permission 1")]
        [TestCase("My permission 1 !@")]
        public void TestConstructor_WhenIncorrectNameGiven_ThenArgumentExceptionIsThrown(string name)
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentException>()
                  .And.Message
                  .EqualTo("Name must contain only alphanumeric characters without spaces."),
              () => new PermissionId(new ResourceId("MyResource"), name));
        }

        [TestCase("MyResource", "MyPermission")]
        [TestCase("MyResource2", "example1permision")]
        [TestCase("MyResource3", "GetProducts")]
        [TestCase("MyResource4", "asdghxcbnm123456890asdggh")]
        public void TestToString_WhenCorrectNameAndResourceIdGiven_ThenResourceIdAndNameWithDotSeperationStringIsReturned(
            string resourceName,
            string name)
        {
            PermissionId permissionId = new PermissionId(new ResourceId(resourceName), name);

            Assert.That(permissionId.ToString(), Is.EqualTo($"{resourceName}.{name}"));
        }
    }
}