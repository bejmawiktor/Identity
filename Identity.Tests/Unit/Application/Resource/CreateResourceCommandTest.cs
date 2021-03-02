using Identity.Application;
using NUnit.Framework;

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
                resourceDescription: "Resource description.");

            Assert.That(createResourceCommand.ResourceId, Is.EqualTo("MyResource"));
        }

        [Test]
        public void TestConstructing_WhenResourceDescriptionGiven_ThenResourceDescriptionIsSet()
        {
            var createResourceCommand = new CreateResourceCommand(
                resourceId: "MyResource",
                resourceDescription: "Resource description.");

            Assert.That(createResourceCommand.ResourceDescription, Is.EqualTo("Resource description."));
        }
    }
}