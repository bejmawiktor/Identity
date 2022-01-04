using Identity.Core.Application;
using Moq;
using NUnit.Framework;
using System;
using IApplicationsRepository = Identity.Core.Application.IApplicationsRepository;

namespace Identity.Tests.Unit.Core.Application
{
    [TestFixture]
    public class ApplicationsRepositoryAdapterTest
    {
        [Test]
        public void TestConstructor_WhenNullApplicationsRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("applicationsRepository"),
               () => new ApplicationsRepositoryAdapter(null));
        }

        [Test]
        public void TestConstructor_WhenApplicationsRepositoryGiven_ThenApplicationsRepositoryIsSet()
        {
            Mock<IApplicationsRepository> applicationsRepositoryMock = new();
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            ApplicationsRepositoryAdapter applicationsRepositoryAdapter = new(applicationsRepository);

            Assert.That(applicationsRepositoryAdapter.ApplicationsRepository, Is.EqualTo(applicationsRepository));
        }
    }
}