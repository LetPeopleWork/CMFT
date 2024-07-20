﻿using Lighthouse.Backend.API.DTO;
using Lighthouse.Backend.Factories;
using Lighthouse.Backend.Models;
using Lighthouse.Backend.Services.Factories;
using Lighthouse.Backend.Services.Interfaces;
using Lighthouse.Backend.WorkTracking;
using Microsoft.AspNetCore.Mvc;

namespace Lighthouse.Backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkTrackingSystemConnectionsController : ControllerBase
    {
        private readonly IWorkTrackingSystemFactory workTrackingSystemFactory;
        private readonly IRepository<WorkTrackingSystemConnection> repository;
        private readonly IWorkItemServiceFactory workItemServiceFactory;

        public WorkTrackingSystemConnectionsController(IWorkTrackingSystemFactory workTrackingSystemFactory, IRepository<WorkTrackingSystemConnection> repository, IWorkItemServiceFactory workItemServiceFactory)
        {
            this.workTrackingSystemFactory = workTrackingSystemFactory;
            this.repository = repository;
            this.workItemServiceFactory = workItemServiceFactory;
        }

        [HttpGet("supported")]
        public ActionResult<IEnumerable<WorkTrackingSystemConnectionDto>> GetSupportedWorkTrackingSystemConnections()
        {
            var supportedSystems = new List<WorkTrackingSystemConnectionDto>();

            foreach (WorkTrackingSystems system in Enum.GetValues(typeof(WorkTrackingSystems)))
            {
                AddConnectionForWorkTrackingSystem(supportedSystems, system);
            }

            return Ok(supportedSystems);
        }

        [HttpGet]
        public ActionResult<IEnumerable<WorkTrackingSystemConnectionDto>> GetWorkTrackingSystemConnections()
        {
            var existingConnections = repository.GetAll();

            var connectionDtos = existingConnections.Select(c => new WorkTrackingSystemConnectionDto(c));
            return Ok(connectionDtos);
        }

        [HttpPost]
        public async Task<ActionResult<WorkTrackingSystemConnectionDto>> CreateNewWorkTrackingSystemConnectionAsync([FromBody] WorkTrackingSystemConnectionDto newConnection)
        {
            var connection = CreateConnectionFromDto(newConnection);

            repository.Add(connection);
            await repository.Save();

            var createdConnectionDto = new WorkTrackingSystemConnectionDto(connection);
            return Ok(createdConnectionDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WorkTrackingSystemConnectionDto>> UpdateWorkTrackingSystemConnectionAsync(int id, [FromBody] WorkTrackingSystemConnectionDto updatedConnection)
        {
            var existingConnection = repository.GetById(id);
            if (existingConnection == null)
            {
                return NotFound();
            }

            existingConnection.Name = updatedConnection.Name;

            foreach (var option in updatedConnection.Options)
            {
                var existingOption = existingConnection.Options.Single(o => o.Key == option.Key);
                existingOption.Value = option.Value;
            }

            repository.Update(existingConnection);
            await repository.Save();

            var updatedConnectionDto = new WorkTrackingSystemConnectionDto(existingConnection);
            return Ok(updatedConnectionDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWorkTrackingSystemConnectionAsync(int id)
        {
            if (!repository.Exists(id))
            {
                return NotFound();
            }

            repository.Remove(id);
            await repository.Save();

            return Ok();
        }

        [HttpPost("validate")]
        public async Task<ActionResult<bool>> ValidateConnection(WorkTrackingSystemConnectionDto connectionDto)
        {
            var workItemService = workItemServiceFactory.GetWorkItemServiceForWorkTrackingSystem(connectionDto.WorkTrackingSystem);

            var connection = CreateConnectionFromDto(connectionDto);

            var isConnectionValid = await workItemService.ValidateConnection(connection);

            return Ok(isConnectionValid);
        }


        private void AddConnectionForWorkTrackingSystem(List<WorkTrackingSystemConnectionDto> supportedSystems, WorkTrackingSystems system)
        {
            var defaultConnection = workTrackingSystemFactory.CreateDefaultConnectionForWorkTrackingSystem(system);

            supportedSystems.Add(new WorkTrackingSystemConnectionDto(defaultConnection));
        }

        private WorkTrackingSystemConnection CreateConnectionFromDto(WorkTrackingSystemConnectionDto connectionDto)
        {
            var connection = new WorkTrackingSystemConnection()
            {
                Id = connectionDto.Id,
                Name = connectionDto.Name,
                WorkTrackingSystem = connectionDto.WorkTrackingSystem,
            };

            foreach (var option in connectionDto.Options)
            {
                connection.Options.Add(
                    new WorkTrackingSystemConnectionOption
                    {
                        Key = option.Key,
                        Value = option.Value,
                        IsSecret = option.IsSecret
                    }
                    );
            }

            return connection;
        }
    }
}
