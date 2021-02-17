using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class PermissionIdTest
    {
        [Test]
        public void TestConstructing_WhenEmptyNameGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentException>()
                  .And.Message
                  .EqualTo("Permision id can't be empty."),
              () => new PermissionId(string.Empty));
        }

        [TestCase("My permission")]
        [TestCase("My$permission")]
        [TestCase("$%^MyPermission!@#")]
        [TestCase("My permission 1")]
        [TestCase("My permission 1 !@")]
        public void TestConstructing_WhenIncorrectNameGiven_ThenArgumentExceptionIsThrown(string name)
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentException>()
                  .And.Message
                  .EqualTo("Permision id must contain only alphanumeric characters without spaces."),
              () => new PermissionId(name));
        }

        [TestCase("MyPermission")]
        [TestCase("example1permision")]
        [TestCase("GetProducts")]
        [TestCase("asdghxcbnm123456890asdggh")]
        public void TestConstructing_WhenCorrectNameGiven_ThenNameIsSet(string name)
        {
            var permissionId = new PermissionId(name);

            Assert.That(permissionId.ToString(), Is.EqualTo(name));
        }
    }
}