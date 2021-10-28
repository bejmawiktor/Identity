using Identity.Application;
using NUnit.Framework;
using System;

namespace Identity.Tests.Unit.Application
{
    [TestFixture]
    public class CreateResourceCommandTest
    {
        [Test]
        public void TestConstructor_WhenResourceIdGiven_ThenResourceIdIsSet()
        {
            var createResourceCommand = new CreateResourceCommand(
                resourceId: "MyResource",
                resourceDescription: "Resource description.",
                userId: Guid.NewGuid());

            Assert.That(createResourceCommand.ResourceId, Is.EqualTo("MyResource"));
        }

        [Test]
        public void TestConstructor_WhenResourceDescriptionGiven_ThenResourceDescriptionIsSet()
        {
            var createResourceCommand = new CreateResourceCommand(
                resourceId: "MyResource",
                resourceDescription: "Resource description.",
                userId: Guid.NewGuid());

            Assert.That(createResourceCommand.ResourceDescription, Is.EqualTo("Resource description."));
        }

        [Test]
        public void TestConstructor_WhenUserIdGiven_ThenUserIdIsSet()
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