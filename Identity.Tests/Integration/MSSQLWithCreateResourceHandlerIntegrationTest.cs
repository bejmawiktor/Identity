﻿using DDD.Domain.Events;
using Identity.Application;
using Identity.Persistence.MSSQL;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Identity.Tests.Integration
{
    [TestFixture]
    public class MSSQLWithCreateResourceHandlerIntegrationTest
    {
        private IConfigurationRoot Config => new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory + @"\Integration\")
            .AddUserSecrets<MSSQLWithCreateResourceHandlerIntegrationTest>()
            .Build();

        private IdentityContext IdentityContext { get; set; }
        private IDbContextTransaction Transaction { get; set; }

        [SetUp]
        public void SetUp()
        {
            var eventDispatcher = new Mock<IEventDispatcher>();
            this.IdentityContext = new IdentityContext(this.Config.GetConnectionString("DevelopmentDatabase"));
            this.Transaction = this.IdentityContext.Database.BeginTransaction();
            EventManager.Instance.EventDispatcher = eventDispatcher.Object;
        }

        [TearDown]
        public void TearDown()
        {
            EventManager.Instance.EventDispatcher = null;
            this.Transaction.Rollback();
            this.IdentityContext.Dispose();
        }

        [Test]
        public void TestCreateResource_WhenResourceDataGiven_ThenResourceIsStored()
        {
            var createResourceCommand = new CreateResourceCommand("MyResource", "Resource description");
            var resourceRepository = new ResourcesRepository(this.IdentityContext);
            var createResourceCommandHandler = new CreateResourceCommandHandler(resourceRepository);

            createResourceCommandHandler.Handle(createResourceCommand);

            Assert.That(resourceRepository.Get("MyResource"), Is.EqualTo(new ResourceDto("MyResource", "Resource description")));
        }

        [Test]
        public async Task TestAsyncCreateResource_WhenResourceDataGiven_ThenResourceIsStored()
        {
            var createResourceCommand = new CreateResourceCommand("MyResource", "Resource description");
            var resourceRepository = new ResourcesRepository(this.IdentityContext);
            var createResourceCommandHandler = new CreateResourceCommandHandler(resourceRepository);

            await createResourceCommandHandler.HandleAsync(createResourceCommand);

            Assert.That(resourceRepository.Get("MyResource"), Is.EqualTo(new ResourceDto("MyResource", "Resource description")));
        }
    }
}