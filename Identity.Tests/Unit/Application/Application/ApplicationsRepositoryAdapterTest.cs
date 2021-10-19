using Identity.Application;
using Moq;
using NUnit.Framework;
using System;
using IApplicationsRepository = Identity.Application.IApplicationsRepository;

namespace Identity.Tests.Unit.Application
{
    [TestFixture]
    public class ApplicationsRepositoryAdapterTest
    {
        [Test]
        public void TestConstructing_WhenNullApplicationsRepositoryGiven_ThenArgumentNullExceptionIsThrown()
        {
            Assert.Throws(
               Is.InstanceOf<ArgumentNullException>()
                   .And.Property(nameof(ArgumentNullException.ParamName))
                   .EqualTo("applicationsRepository"),
               () => new ApplicationsRepositoryAdapter(null));
        }

        [Test]
        public void TestConstructing_WhenApplicationsRepositoryGiven_ThenApplicationsRepositoryIsSet()
        {
            var applicationsRepositoryMock = new Mock<IApplicationsRepository>();
            IApplicationsRepository applicationsRepository = applicationsRepositoryMock.Object;
            var applicationsRepositoryAdapter = new ApplicationsRepositoryAdapter(applicationsRepository);

            Assert.That(applicationsRepositoryAdapter.ApplicationsRepository, Is.EqualTo(applicationsRepository));
        }
    }
}