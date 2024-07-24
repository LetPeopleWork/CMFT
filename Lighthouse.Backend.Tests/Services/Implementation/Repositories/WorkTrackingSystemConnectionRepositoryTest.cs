﻿using Lighthouse.Backend.Models;
using Lighthouse.Backend.Services.Implementation.Repositories;
using Lighthouse.Backend.Services.Interfaces;
using Lighthouse.Backend.Tests.TestHelpers;
using Lighthouse.Backend.WorkTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Lighthouse.Backend.Tests.Services.Implementation.Repositories
{
    public class WorkTrackingSystemConnectionRepositoryTest : IntegrationTestBase
    {
        public WorkTrackingSystemConnectionRepositoryTest() : base(new TestWebApplicationFactory<Program>())
        {
        }

        [Test]
        public async Task GetWorkTrackingSystemConnectionById_ExistingId_RetunrsCorrectTeam()
        {
            var subject = CreateSubject();
            var workTrackingOption = new WorkTrackingSystemConnectionOption { Key = "key", Value = "value", IsSecret = false };
            var connection = new WorkTrackingSystemConnection { Name = "Test", WorkTrackingSystem = WorkTrackingSystems.AzureDevOps };
            connection.Options.Add(workTrackingOption);

            subject.Add(connection);
            await subject.Save();

            var foundConnection = subject.GetById(connection.Id);

            Assert.That(foundConnection, Is.EqualTo(connection));
            CollectionAssert.AreEquivalent(foundConnection.Options, new List<WorkTrackingSystemConnectionOption> { workTrackingOption });
        }

        [Test]
        public async Task DoesNotEncryptNonSecretOptionsOnSave()
        {
            var optionValue = "This you can know";

            var subject = CreateSubject();
            var nonSecretOption = new WorkTrackingSystemConnectionOption { Key = "NotSecret", Value = optionValue, IsSecret = false };
            var connection = new WorkTrackingSystemConnection { Name = "Test", WorkTrackingSystem = WorkTrackingSystems.AzureDevOps };
            connection.Options.Add(nonSecretOption);

            subject.Add(connection);
            await subject.Save();

            var storedOption = DatabaseContext.WorkTrackingSystemConnections.Include(w => w.Options).SelectMany(w => w.Options).Single();

            Assert.That(storedOption.Value, Is.EqualTo(optionValue));
        }

        [Test]
        public async Task ShouldEncryptSecretOptionsOnSave()
        {
            var optionValue = "you never will find out";

            var subject = CreateSubject();
            var option = new WorkTrackingSystemConnectionOption { Key = "secret", Value = optionValue, IsSecret = true };
            var connection = new WorkTrackingSystemConnection { Name = "Test", WorkTrackingSystem = WorkTrackingSystems.AzureDevOps };
            connection.Options.Add(option);

            subject.Add(connection);
            await subject.Save();

            var storedOption = DatabaseContext.WorkTrackingSystemConnections.Include(w => w.Options).SelectMany(w => w.Options).Single();

            Assert.That(storedOption.Value, Is.Not.EqualTo(optionValue));
        }

        private WorkTrackingSystemConnectionRepository CreateSubject()
        {
            var cryptoService = ServiceProvider.GetService<ICryptoService>() ?? throw new InvalidOperationException("CryptoService is not configured.");
            return new WorkTrackingSystemConnectionRepository(DatabaseContext, cryptoService, Mock.Of<ILogger<WorkTrackingSystemConnectionRepository>>());
        }
    }
}
