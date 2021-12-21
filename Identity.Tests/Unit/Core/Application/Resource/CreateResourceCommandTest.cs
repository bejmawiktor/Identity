using Identity.Core.Application;
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
            CreateResourceCommand createResourceCommand = this.GetCreateResourceCommand("MyResource");

            Assert.That(createResourceCommand.ResourceId, Is.EqualTo("MyResource"));
        }

        private CreateResourceCommand GetCreateResourceCommand(
            string resourceId = null, 
            string resourceDescription = null, 
            Guid? userId = null)
        {
            return new CreateResourceCommand(
                resourceId: resourceId ?? "MyResource",
                resourceDescription: resourceDescription ?? "Resource description.",
                userId: userId ?? Guid.NewGuid());
        }

        [Test]
        public void TestConstructor_WhenResourceDescriptionGiven_ThenResourceDescriptionIsSet()
        {
            CreateResourceCommand createResourceCommand = this.GetCreateResourceCommand(
                resourceDescription: "Resource description.");

            Assert.That(createResourceCommand.ResourceDescription, Is.EqualTo("Resource description."));
        }

        [Test]
        public void TestConstructor_WhenUserIdGiven_ThenUserIdIsSet()
        {
            var userId = Guid.NewGuid();
            CreateResourceCommand createResourceCommand = this.GetCreateResourceCommand(userId: userId);

            Assert.That(createResourceCommand.UserId, Is.EqualTo(userId));
        }
    }
}