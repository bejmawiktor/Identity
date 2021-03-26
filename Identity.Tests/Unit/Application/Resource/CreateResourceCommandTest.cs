using Identity.Application;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Application
{
    [TestFixture]
    public class CreateResourceCommandTest
    {
        [Test]
        public void TestConstructing_WhenResourceIdGiven_ThenResourceIdIsSet()
        {
            var createResourceCommand = new CreateResourceCommand(
                resourceId: "MyResource",
                resourceDescription: "Resource description.",
                userId: Guid.NewGuid());

            Assert.That(createResourceCommand.ResourceId, Is.EqualTo("MyResource"));
        }

        [Test]
        public void TestConstructing_WhenResourceDescriptionGiven_ThenResourceDescriptionIsSet()
        {
            var createResourceCommand = new CreateResourceCommand(
                resourceId: "MyResource",
                resourceDescription: "Resource description.",
                userId: Guid.NewGuid());

            Assert.That(createResourceCommand.ResourceDescription, Is.EqualTo("Resource description."));
        }

        [Test]
        public void TestConstructing_WhenUserIdGiven_ThenUserIdIsSet()
        {
            var userId = Guid.NewGuid();
            var createResourceCommand = new CreateResourceCommand(
                resourceId: "MyResource",
                resourceDescription: "Resource description.",
                userId: userId);

            Assert.That(createResourceCommand.UserId, Is.EqualTo(userId));
        }
    }
}