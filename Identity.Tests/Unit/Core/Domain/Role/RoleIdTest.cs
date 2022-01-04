using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class RoleIdTest
    {
        [Test]
        public void TestConstructor_WhenEmptyGuidGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentException>()
                  .And.Message
                  .EqualTo("Guid can't be empty."),
              () => new RoleId(Guid.Empty));
        }

        [Test]
        public void TestToGuid_WhenConvertingToGuid_ThenGuidIsReturned()
        {
            Guid guid = Guid.NewGuid();
            RoleId roleId = new(guid);

            Assert.That(roleId.ToGuid(), Is.EqualTo(guid));
        }

        [Test]
        public void TestGenerate_WhenGeneretingRoleId_ThenNewRoleIdIsReturned()
        {
            RoleId roleId = RoleId.Generate();

            Assert.That(roleId, Is.Not.Null);
        }
    }
}