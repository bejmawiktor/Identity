using Identity.Domain;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Domain
{
    [TestFixture]
    public class UserIdTest
    {
        [Test]
        public void TestConstructing_WhenEmptyGuidGiven_ThenArgumentExceptionIsThrown()
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
            var guid = Guid.NewGuid();
            var userId = new UserId(guid);

            Assert.That(userId.ToGuid(), Is.EqualTo(guid));
        }

        [Test]
        public void TestGenerate_WhenGeneratingUserId_ThenNewUserIdIsReturned()
        {
            var userId = UserId.Generate();

            Assert.That(userId, Is.Not.Null);
        }

        [Test]
        public void TestToString_WhenConvertingToString_ThenGuidStringIsReturned()
        {
            var guid = Guid.NewGuid();
            var userId = new UserId(guid);

            string userIdString = userId.ToString();

            Assert.That(userIdString, Is.EqualTo(guid.ToString()));
        }
    }
}