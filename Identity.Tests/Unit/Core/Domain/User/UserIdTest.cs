using Identity.Core.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Core.Domain
{
    [TestFixture]
    public class UserIdTest
    {
        [Test]
        public void TestConstructor_WhenEmptyGuidGiven_ThenArgumentExceptionIsThrown()
        {
            Assert.Throws(
              Is.InstanceOf<ArgumentException>()
                  .And.Message
                  .EqualTo("Guid can't be empty."),
              () => new UserId(Guid.Empty));
        }

        [Test]
        public void TestToGuid_WhenConvertingToGuid_ThenGuidIsReturned()
        {
            Guid guid = Guid.NewGuid();
            var userId = new UserId(guid);

            Assert.That(userId.ToGuid(), Is.EqualTo(guid));
        }

        [Test]
        public void TestGenerate_WhenGeneratingUserId_ThenNewUserIdIsReturned()
        {
            UserId userId = UserId.Generate();

            Assert.That(userId, Is.Not.Null);
        }

        [Test]
        public void TestToString_WhenConvertingToString_ThenGuidStringIsReturned()
        {
            Guid guid = Guid.NewGuid();
            var userId = new UserId(guid);

            string userIdString = userId.ToString();

            Assert.That(userIdString, Is.EqualTo(guid.ToString()));
        }
    }
}