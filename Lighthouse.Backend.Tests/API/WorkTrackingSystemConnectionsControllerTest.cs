﻿using Lighthouse.Backend.API;
using Lighthouse.Backend.API.DTO;
using Lighthouse.Backend.Factories;
using Lighthouse.Backend.Models;
using Lighthouse.Backend.Services.Factories;
using Lighthouse.Backend.Services.Interfaces;
using Lighthouse.Backend.WorkTracking;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework.Internal;

namespace Lighthouse.Backend.Tests.API
{
    public class WorkTrackingSystemConnectionsControllerTest
    {
        private Mock<IWorkTrackingSystemFactory> workTrackingSystemsFactoryMock;
        
        private Mock<IRepository<WorkTrackingSystemConnection>> repositoryMock;

        private Mock<IWorkItemServiceFactory> workItemServiceFactoryMock;

        [SetUp]
        public void Setup()
        {
            workTrackingSystemsFactoryMock = new Mock<IWorkTrackingSystemFactory>();
            repositoryMock = new Mock<IRepository<WorkTrackingSystemConnection>>();
            workItemServiceFactoryMock = new Mock<IWorkItemServiceFactory>();
        }

        [Test]
        [TestCase(WorkTrackingSystems.Jira, WorkTrackingSystems.AzureDevOps)]
        public void GetSupportedWorkTrackingSystems_ReturnsDefaultSystemsFromFactory(params WorkTrackingSystems[] workTrackingSystems)
        {
            var subject = CreateSubject();

            foreach (var workTrackingSystem in workTrackingSystems)
            {
                workTrackingSystemsFactoryMock.Setup(x => x.CreateDefaultConnectionForWorkTrackingSystem(workTrackingSystem)).Returns(new WorkTrackingSystemConnection());
            }

            var result = subject.GetSupportedWorkTrackingSystemConnections();

            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult.StatusCode, Is.EqualTo(200));

                var supportedSystems = okResult.Value as IEnumerable<WorkTrackingSystemConnectionDto>;

                Assert.That(supportedSystems?.Count(), Is.EqualTo(workTrackingSystems.Length));
            });

            workTrackingSystemsFactoryMock.Verify(x => x.CreateDefaultConnectionForWorkTrackingSystem(It.IsAny<WorkTrackingSystems>()), Times.Exactly(2));
        }

        [Test]
        public void GetWorkTrackingSystemConnections_ReturnsAllConfiguredConnection()
        {
            var expectedConnections = new List<WorkTrackingSystemConnection>();
            expectedConnections.Add(new WorkTrackingSystemConnection());
            expectedConnections.Add(new WorkTrackingSystemConnection());

            repositoryMock.Setup(x => x.GetAll()).Returns(expectedConnections);

            var subject = CreateSubject();

            var result = subject.GetWorkTrackingSystemConnections();

            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult.StatusCode, Is.EqualTo(200));

                var connections = okResult.Value as IEnumerable<WorkTrackingSystemConnectionDto>;

                Assert.That(connections?.Count(), Is.EqualTo(expectedConnections.Count));
            });
        }

        [Test]
        public async Task CreateNewWorkTrackingSystemConnection_GivenConnectionDto_CreatesAndSavesNewConnectionAsync()
        {
            var newConnectionDto = new WorkTrackingSystemConnectionDto
            {
                Name = "Test",
                WorkTrackingSystem = WorkTrackingSystems.Jira,
            };
            newConnectionDto.Options.Add(new WorkTrackingSystemConnectionOptionDto { Key = "MyKey", Value = "MyValue", IsSecret = false });

            var subject = CreateSubject();

            var result = await subject.CreateNewWorkTrackingSystemConnectionAsync(newConnectionDto);

            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult.StatusCode, Is.EqualTo(200));

                var connection = okResult.Value as WorkTrackingSystemConnectionDto;
                Assert.That(connection.Name, Is.EqualTo("Test"));
                Assert.That(connection.WorkTrackingSystem, Is.EqualTo(WorkTrackingSystems.Jira));
                Assert.That(connection.Options, Has.Count.EqualTo(1));
                Assert.That(connection.Options.Single().Key, Is.EqualTo("MyKey"));
                Assert.That(connection.Options.Single().Value, Is.EqualTo("MyValue"));
                Assert.That(connection.Options.Single().IsSecret, Is.EqualTo(false));
            });

            repositoryMock.Verify(x => x.Add(It.IsAny<WorkTrackingSystemConnection>()));
            repositoryMock.Verify(x => x.Save());
        }

        [Test]
        public async Task UpdateWorkTrackingSystemConnection_ConnectionExists_SavesChangesAsync()
        {
            var existingConnection = new WorkTrackingSystemConnection { Name = "Boring Old Name" };
            existingConnection.Options.Add(new WorkTrackingSystemConnectionOption { Key = "Option", Value = "Old Option Value" });
            repositoryMock.Setup(x => x.GetById(12)).Returns(existingConnection);

            var subject = CreateSubject();

            var connectionDto = new WorkTrackingSystemConnectionDto { Id = 12, Name = "Fancy New Name" };
            connectionDto.Options.Add(new WorkTrackingSystemConnectionOptionDto { Key = "Option", Value = "Nobody expects the Spanish Inquisition" });
            var result = await subject.UpdateWorkTrackingSystemConnectionAsync(12, connectionDto);

            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                var okResult = result.Result as OkObjectResult;

                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                var connection = okResult.Value as WorkTrackingSystemConnectionDto;

                Assert.That(connection.Name, Is.EqualTo("Fancy New Name"));
                Assert.That(connection.Options, Has.Count.EqualTo(1));
                Assert.That(connection.Options.Single().Value, Is.EqualTo("Nobody expects the Spanish Inquisition"));
            });

            repositoryMock.Verify(x => x.Update(It.IsAny<WorkTrackingSystemConnection>()));
            repositoryMock.Verify(x => x.Save());
        }

        [Test]
        public async Task UpdateWorkTrackingSystemConnection_ConnectionDoesNotExist_ReturnsNotFoundAsync()
        {
            var subject = CreateSubject();

            var result = await subject.UpdateWorkTrackingSystemConnectionAsync(12, new WorkTrackingSystemConnectionDto());

            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
                var notFoundResult = result.Result as NotFoundResult;

                Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
            });
        }

        [Test]
        public async Task DeleteWorkTrackingSystemConnection_ConnectionExists_DeletesAsync()
        {
            repositoryMock.Setup(x => x.Exists(12)).Returns(true);

            var subject = CreateSubject();

            var result = await subject.DeleteWorkTrackingSystemConnectionAsync(12);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<OkResult>());
                var okResult = result as OkResult;

                Assert.That(okResult.StatusCode, Is.EqualTo(200));
            });

            repositoryMock.Verify(x => x.Remove(12));
            repositoryMock.Verify(x => x.Save());
        }

        [Test]
        public async Task DeleteWorkTrackingSystemConnection_ConnectionDoesNotExist_ReturnsNotFoundAsync()
        {
            var subject = CreateSubject();

            var result = await subject.DeleteWorkTrackingSystemConnectionAsync(12);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<NotFoundResult>());
                var notFoundResult = result as NotFoundResult;

                Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));
            });
        }

        [Test]
        [TestCase(WorkTrackingSystems.AzureDevOps)]
        [TestCase(WorkTrackingSystems.Jira)]
        public async Task ValidateConnection_InvokesWorkItemService_ReturnsResult(WorkTrackingSystems workTrackingSystem)
        {
            var subject = CreateSubject();

            var workItemServiceMock = new Mock<IWorkItemService>();
            workItemServiceMock.Setup(x => x.ValidateConnection(It.IsAny<WorkTrackingSystemConnection>())).ReturnsAsync(true);

            workItemServiceFactoryMock.Setup(x => x.GetWorkItemServiceForWorkTrackingSystem(workTrackingSystem)).Returns(workItemServiceMock.Object);

            var connectionDto = new WorkTrackingSystemConnectionDto { Id = 12, Name = "Connection", WorkTrackingSystem = workTrackingSystem };
            var result = await subject.ValidateConnection(connectionDto);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<ActionResult<bool>>());
                var okResult = result.Result as OkObjectResult;

                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(true));
            });
        }

        private WorkTrackingSystemConnectionsController CreateSubject()
        {
            return new WorkTrackingSystemConnectionsController(workTrackingSystemsFactoryMock.Object, repositoryMock.Object, workItemServiceFactoryMock.Object);
        }
    }
}
